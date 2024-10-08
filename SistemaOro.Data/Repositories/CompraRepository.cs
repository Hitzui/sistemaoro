using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class CompraRepository(
    IAdelantosRepository adelantoRepository, 
    IParametersRepository parametersRepository, 
    IMaestroCajaRepository maestroCajaRepository, 
    ICierrePrecioRepository cierrePrecioRepository, 
    ITipoCambioRepository tipoCambioRepository, 
    IMonedaRepository monedaRepository, 
    DataContext context)
    : ICompraRepository
{
    private string? _caja = ConfiguracionGeneral.Caja;
    private string? _agencia = ConfiguracionGeneral.Agencia;
    private readonly Usuario? _usuario = VariablesGlobales.Instance.Usuario;

    public void ImprimirCompra(string numeroCompra)
    {
        //imprimir la comprar con los datos implicados
    }

    public async Task<bool> Create(Compra compra, List<DetCompra> detaCompra, List<Adelanto>? listaAdelantos = null, List<CierrePrecio>? listaPreciosaCerrar = null)
    {

        var modCaja = await maestroCajaRepository.FindByCajaAndAgencia(_caja, _agencia);
        if (modCaja is null)
        {
            ErrorSms ="Debe especificar los datos de caja para continua";
            return false;
        }

        if (decimal.Compare(modCaja.Sfinal!.Value, decimal.Zero) <= 0)
        {
            ErrorSms = "No hay saldo disponible en caja para realizar la compra";
            return false;
        }

        if (decimal.Compare(modCaja.Sfinal.Value, compra.Efectivo) < 0)
        {
            ErrorSms = $"No hay saldo disponible en caja para realizar la compra, Saldo: {modCaja.Sfinal}";
            return false;
        }

        var validarCajaAbierta = await maestroCajaRepository.ValidarCajaAbierta(modCaja.Codcaja, modCaja.Codagencia!);
        if (!validarCajaAbierta)
        {
            ErrorSms = $"Debe aperturar caja para poder realizar movimientos, intente nuevamente";
            return false;
        }

        var numeroCompra = await CodigoCompra();
        if (string.IsNullOrWhiteSpace(numeroCompra))
        {
            ErrorSms = "No fue posible obtener el número de compra";
            return false;
        }

        var saldoDolares = decimal.Zero;
        var saldoCordobas = decimal.Zero;
        compra.Numcompra = numeroCompra;
        var param = await parametersRepository.RecuperarParametros();
        if (param is null)
        {
            ErrorSms = (VariablesGlobales.Instance.ConfigurationSection["ERROR_PARAM"]);
            return false;
        }

        compra.Nocontrato = param.Nocontrato;
        param.Nocontrato += 1;
        var tbTipoCambio = await tipoCambioRepository.FindByDateNow();
        var tipoCambioDia = decimal.One;
        var moneda = await monedaRepository.GetByIdAsync(compra.Codmoneda);
        if (moneda is null)
        {
            ErrorSms = VariablesGlobales.Instance.ConfigurationSection["ERROR_MONEDA"];
            return false;
        }

        if (tbTipoCambio is not null)
        {
            tipoCambioDia = tbTipoCambio.Tipocambio;
        }

        var adelantosPorClientes = await adelantoRepository.ListarAdelantosPorClientes(compra.Codcliente);
        if (adelantosPorClientes.Count > 0)
        {
            saldoCordobas = adelantosPorClientes.Where(adelanto => adelanto.Saldo > 0 && adelanto.Codmoneda == param.Cordobas).Select(adelanto => adelanto.Saldo).Sum();
            saldoDolares = adelantosPorClientes.Where(adelanto => adelanto.Saldo > 0 && adelanto.Codmoneda == param.Dolares).Select(adelanto => adelanto.Saldo).Sum();
        }
        var efectivo = compra.Efectivo;
        var transferencia = compra.Transferencia;
        var cheque = compra.Cheque;
        if (compra.Codmoneda == param.Dolares)
        {
            efectivo = compra.Efectivo* tipoCambioDia;
            transferencia = compra.Efectivo* tipoCambioDia;
            cheque = compra.Efectivo* tipoCambioDia;
        }
        //detalle de caja
        var detaCaja = new Detacaja
        {
            Tipocambio = tipoCambioDia,
            Concepto = $"***Compra: {numeroCompra}***",
            Referencia = $"Compra: {numeroCompra}; Moneda: {moneda.Descripcion}",
            Codcaja = _caja,
            Cheque = cheque,
            Efectivo = efectivo,
            Transferencia = transferencia,
            Fecha = compra.Fecha,
            Hora = compra.Fecha.ToShortTimeString(),
            Idcaja = modCaja.Idcaja,
            Idmov = param.Idcompras!.Value
        };
        var listTmpPrecios = await context.Tmpprecios.Where(tmpprecio => tmpprecio.Codcliente == compra.Codcliente).ToListAsync();
        if (listTmpPrecios.Count > 0)
        {
            var listDetacierre = new List<Detacierre>();
            foreach (var tmpprecio in listTmpPrecios)
            {
                var findByIdCierrePrecio = await cierrePrecioRepository.GetByIdAsync(tmpprecio.Codcierre);
                if (findByIdCierrePrecio is not null)
                {
                    var xSaldo = decimal.Subtract(findByIdCierrePrecio.SaldoOnzas, tmpprecio.Cantidad);
                    var detaCierre = new Detacierre
                    {
                        Codcierre = tmpprecio.Codcierre,
                        Cantidad = xSaldo,
                        Onzas = findByIdCierrePrecio.SaldoOnzas,
                        Saldo = tmpprecio.Cantidad,
                        Fecha = DateTime.Now,
                        Numcompra = compra.Numcompra,
                        Codagencia = _agencia
                    };
                    findByIdCierrePrecio.SaldoOnzas = tmpprecio.Cantidad;
                    if (decimal.Compare(tmpprecio.Cantidad, decimal.Zero) == 0)
                    {
                        findByIdCierrePrecio.Status = false;
                    }

                    listDetacierre.Add(detaCierre);
                }
            }

            if (listDetacierre.Count > 0)
            {
                await context.Detacierres.AddRangeAsync(listDetacierre);
                context.Tmpprecios.RemoveRange(listTmpPrecios);
            }
        }

        await context.Detacajas.AddAsync(detaCaja);
        if (decimal.Compare(compra.Adelantos, decimal.Zero) > 0 && listaAdelantos is not null)
        {
            var saldoAdelanto = compra.Adelantos;
            foreach (var adelanto in listaAdelantos)
            {
                if (decimal.Compare(saldoAdelanto, decimal.Zero) > 0)
                {
                    var comprasAdelantos = new ComprasAdelanto
                    {
                        Codcaja = _caja,
                        Codcliente = compra.Codcliente,
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.TimeOfDay,
                        Idadelanto = adelanto.Idsalida,
                        Numcompra = compra.Numcompra,
                        Usuario = _usuario.Username,
                        Codagencia = _agencia,
                        Codmoneda = adelanto.Codmoneda
                    };
                    var auxSaldo = adelanto.Saldo;
                    comprasAdelantos.Sinicial = adelanto.Saldo;
                    if (decimal.Compare(auxSaldo, saldoAdelanto) > 0)
                    {
                        comprasAdelantos.Monto = saldoAdelanto;
                        auxSaldo = decimal.Subtract(auxSaldo, saldoAdelanto);
                        saldoAdelanto = decimal.Zero;
                    }
                    else
                    {
                        comprasAdelantos.Monto = adelanto.Saldo;
                        saldoAdelanto = decimal.Subtract(saldoAdelanto, auxSaldo);
                        auxSaldo = decimal.Zero;
                    }

                    if (comprasAdelantos.Codmoneda == param.Cordobas)
                    {
                        saldoCordobas = decimal.Subtract(saldoCordobas, comprasAdelantos.Monto * tipoCambioDia);
                    }
                    else if (comprasAdelantos.Codmoneda == param.Dolares)
                    {
                        saldoDolares = decimal.Subtract(saldoDolares, comprasAdelantos.Monto);
                    }

                    comprasAdelantos.Sfinal = decimal.Subtract(adelanto.Saldo, comprasAdelantos.Monto);
                    var findAdelanto = await adelantoRepository.FindByCodigoAdelanto(adelanto.Idsalida);
                    if (findAdelanto is not null)
                    {
                        findAdelanto.Saldo = auxSaldo;
                        findAdelanto.Numcompra = findAdelanto.Numcompra.Length <= 0 ? $"{_agencia}-{compra.Numcompra}" : $"{findAdelanto.Numcompra}; {_agencia}-{compra.Numcompra}";
                    }

                    await context.ComprasAdelantos.AddAsync(comprasAdelantos);
                }
            }
        }

        var montoCordobas = decimal.Zero;
        if (compra.Codmoneda == param.Dolares)
        {
            montoCordobas = decimal.Multiply(compra.Efectivo, tipoCambioDia);
        }

        if (compra.Codmoneda == param.Cordobas)
        {
            montoCordobas = compra.Efectivo;
        }

        compra.SaldoAdelanto = saldoCordobas;
        compra.SaldoAdelantoDolares = saldoDolares;
        await context.Compras.AddAsync(compra);
        await context.DetCompras.AddRangeAsync(detaCompra);
        var salida = decimal.Add(modCaja.Salida!.Value, montoCordobas);
        try
        {
            await maestroCajaRepository.ActualizarDatosMaestroCaja(_caja, _agencia, decimal.Zero, salida);
            var result = await context.SaveChangesAsync();
            //dejar de ultimo este metodo
            await context.Precios.Where(precio => precio.Codcliente == compra.Codcliente).ExecuteDeleteAsync();
            await context.Agencias.Where(agencia => agencia.Codagencia == _agencia)
                .ExecuteUpdateAsync(calls => calls.SetProperty(agencia => agencia.Numcompra, agencia => agencia.Numcompra + 1));
            return result > 0;
        }
        catch (Exception e)
        {
            ErrorSms = $"No se pudo guardar la compra debido a error: {e.Message}";
            return false; 
        }
        
    }

    public async Task<bool> UpdateDescargue(Compra compra)
    {
        var listDeta = await FindDetaCompra(compra.Numcompra);
        if (listDeta.Count <= 0)
        {
            ErrorSms = $"No hay datos para la compra con numero {compra.Numcompra}";
            return false;
        }

        foreach (var detCompra in listDeta)
        {
            detCompra.Numdescargue = compra.Dgnumdes;
        }

        context.Update(compra);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = $"No se actualizaron los datos para la compra con numero {compra.Numcompra}";
        return false;
    }

    public async Task<string?> CodigoCompra()
    {
        var numCompra = await context.Agencias.Where(agencia => agencia.Codagencia == _agencia).Select(agencia => agencia.Numcompra).SingleAsync();
        return numCompra is null ? "" : numCompra.Value.ToString();
    }

    public string? ErrorSms { get; private set; } = string.Empty;

    public async Task<List<DetCompra>> FindDetaCompra(string numcompra)
    {
        return await context.DetCompras.Where(compra => compra.Numcompra == numcompra && compra.Codagencia == _agencia).OrderBy(compra => compra.Linea).ToListAsync();
    }

    public async Task<Compra?> FindById(string numerocompra)
    {
        var find = await context.Compras.Where(compra => compra.Numcompra == numerocompra && compra.Codagencia == _agencia).SingleOrDefaultAsync();
        if (find is not null) return find;
        ErrorSms = $"No existe la compra con codigo {numerocompra} en la sucursal {_agencia}";
        return null;
    }

    public async Task<bool> UpdateByDetaCompra(Compra compra, List<DetCompra> detaCompra)
    {
        context.Update(compra);
        foreach (var detCompra in detaCompra)
        {
            context.Update(detCompra);
        }

        var detalleCaja = await context.Detacajas.SingleOrDefaultAsync(detacaja => detacaja.Concepto == $"***COMPRA: {compra.Numcompra}***");
        if (detalleCaja is not null)
        {
            detalleCaja.Cheque = compra.Cheque;
            detalleCaja.Transferencia = compra.Transferencia;
        }

        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = $"No se pudo actualizar los detalles de la compra con el numero de compra {compra.Numcompra}";
        return false;
    }

    public async Task<bool> AnularCompra(string numeroCompra)
    {
        var usuario = VariablesGlobales.Instance.Usuario;
        if (usuario is null)
        {
            throw new EntityValidationException("No existe el usuario en el sistema o no se ha iniciado adecuadamente la sesion");
        }


        var findCompra = await FindById(numeroCompra);
        if (findCompra is null)
        {
            ErrorSms = $"No existe la compra con el codigo {numeroCompra} a anular, intente nuevamente";
            return false;
        }

        var mcaja = await maestroCajaRepository.FindByCajaAndAgencia(_caja, _agencia);
        var param = await parametersRepository.RecuperarParametros();
        if (mcaja is null)
        {
            ErrorSms = "No existe el maestro de caja o este no esta aperturado";
            return false;
        }

        var detalleCaja = await context.Detacajas.SingleOrDefaultAsync(detacaja => detacaja.Concepto == $"***COMPRA: {numeroCompra}***");
        if (detalleCaja is null)
        {
            ErrorSms = $"No existe un detalle de caja con el número de compra {numeroCompra}";
            return false;
        }

        var efectivo = decimal.Zero;
        var tipoCambio = detalleCaja.Tipocambio ?? decimal.One;
        if (param.Cordobas == findCompra.Codmoneda)
        {
            efectivo = findCompra.Efectivo;
        }
        else if (param.Dolares == findCompra.Codmoneda)
        {
            efectivo = findCompra.Efectivo * tipoCambio;
        }

        var nuevoDetaCaja = new Detacaja
        {
            Cheque = findCompra.Cheque,
            Efectivo = efectivo,
            Transferencia = findCompra.Transferencia,
            Codcaja = detalleCaja.Codcaja,
            Concepto = $@"***REVERTIR COMPRA: {numeroCompra}***",
            Fecha = DateTime.Now,
            Hora = DateTime.Now.ToLongTimeString(),
            Idmov = param.AnularAdelanto!.Value,
            Referencia = $@"Movimiento realizado por anulación de compra número: {numeroCompra}",
            Idcaja = mcaja.Idcaja,
            Tipocambio = tipoCambio
        };
        mcaja.Entrada = decimal.Add(efectivo, mcaja.Entrada!.Value);
        mcaja.Sfinal = decimal.Add(efectivo, mcaja.Sfinal!.Value);
        var detalleCierres = await context.Detacierres
            .Where(detacierre => detacierre.Numcompra == numeroCompra && detacierre.Codagencia == findCompra.Codagencia)
            .ToListAsync();
        if (detalleCierres.Count > 0)
        {
            foreach (var detacierre in detalleCierres)
            {
                var cierre = await cierrePrecioRepository.GetByIdAsync(detacierre.Codcierre);
                if (cierre is null) continue;
                cierre.SaldoOnzas = decimal.Add(cierre.SaldoOnzas, detacierre.Cantidad);
                if (decimal.Compare(cierre.SaldoOnzas, decimal.Zero) > 0)
                {
                    cierre.Status = true;
                }
            }

            context.Detacierres.RemoveRange(detalleCierres);
        }

        if (decimal.Compare(findCompra.Adelantos, decimal.Zero) > 0)
        {
            var comprasAdelantos = await adelantoRepository.FindByNumcompraComprasAdelantos(numeroCompra);
            var sumCordobas = decimal.Zero;
            var sumDolares = decimal.Zero;
            foreach (var comprasAdelanto in comprasAdelantos)
            {
                if (comprasAdelanto.Codmoneda == param.Cordobas)
                {
                    sumCordobas = decimal.Add(sumCordobas, comprasAdelanto.Monto);
                }

                if (comprasAdelanto.Codmoneda == param.Dolares)
                {
                    sumDolares = decimal.Add(sumDolares, comprasAdelanto.Monto);
                }
            }

            var recpuerarCodigoAdelanto = await adelantoRepository.RecpuerarCodigoAdelanto();
            var adelanto = new Adelanto
            {
                Codmoneda = param.Cordobas,
                Codcaja = mcaja.Codcaja,
                Hora = DateTime.Now.ToShortTimeString(),
                Fecha = DateTime.Now,
                Numcompra = "",
                Transferencia = 0M,
                Cheque = 0M,
                Usuario = usuario.Username,
                Codcliente = findCompra.Codcliente,
                Idsalida = recpuerarCodigoAdelanto ?? ""
            };

            if (decimal.Compare(sumCordobas, decimal.Zero) > 0)
            {
                adelanto.Monto = sumCordobas;
                adelanto.Saldo = sumCordobas;
                adelanto.MontoLetras = Utilities.NumeroALetras(sumCordobas);
            }

            if (decimal.Compare(sumDolares, decimal.Zero) > 0)
            {
                adelanto.Monto = sumDolares;
                adelanto.Saldo = sumDolares;
                adelanto.MontoLetras = Utilities.NumeroALetras(sumDolares);
            }

            context.ComprasAdelantos.RemoveRange(comprasAdelantos);
            await adelantoRepository.Add(adelanto);
        }

        await context.AnularCompraAsync(numeroCompra, _agencia);
        var movcaja = await context.Movcajas.FindAsync(nuevoDetaCaja.Idmov);
        await maestroCajaRepository.GuardarDatosDetaCaja(nuevoDetaCaja, movcaja!, mcaja);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = $"No se pudo anular la compra con el codigo {numeroCompra}, intente nuevamente";
        return false;
    }

    public async Task<List<Compra>> FindByCodigoCliente(string codCliente)
    {
        return await context.Compras.Where(compra => compra.Codcliente == codCliente).ToListAsync();
    }

    public async Task<bool> UpdateValues(Compra compra)
    {
        var find = await FindById(compra.Numcompra);
        if (find is null)
        {
            return false;
        }

        context.Entry(find).CurrentValues.SetValues(compra);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = $"No se actualizaron los datos de la compra con codigo {compra.Numcompra} en la sucursal {_agencia}";
        return false;
    }

    public async Task<IList<DtoComprasClientes>> FindComprasClientes()
    {
        var result = context.Compras.AsNoTracking()
            .Where(compra => compra.Codestado == EstadoCompra.Vigente || compra.Codestado == EstadoCompra.Cerrada)
            .Join(context.Clientes.AsNoTracking(), compra => compra.Codcliente, cliente => cliente.Codcliente, (compra, cliente) => new { compra, cliente })
            .Select(arg => new DtoComprasClientes()
            {
                Numcompra = arg.compra.Numcompra,
                Codcliente = arg.cliente.Codcliente,
                Nombre = arg.cliente.Nombres,
                Apellido = arg.cliente.Apellidos,
                Total = arg.compra.Total,
                Fecha = arg.compra.Fecha,
                Nocontrato = arg.compra.Nocontrato
            });
        return await result.ToListAsync();
    }
}