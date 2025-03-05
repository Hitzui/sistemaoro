using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NLog;
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
    DataContext context)
    : ICompraRepository
{
    private string? _caja = ConfiguracionGeneral.Caja;
    private string? _agencia = ConfiguracionGeneral.Agencia;
    private readonly Usuario? _usuario = VariablesGlobales.Instance.Usuario;
    private readonly Logger logger = LogManager.GetCurrentClassLogger();
    public void ImprimirCompra(string numeroCompra)
    {
        //imprimir la comprar con los datos implicados
    }

    public async Task<bool> Create(Compra compra, List<DetCompra> detaCompra, List<Adelanto>? listaAdelantos = null, List<CierrePrecio>? listaPreciosaCerrar = null)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            if (context.ChangeTracker.HasChanges())
            {
                context.ChangeTracker.Clear();
            }

            var param = await context.Id.FirstOrDefaultAsync();
            if (param is null)
            {
                ErrorSms = (VariablesGlobales.Instance.ConfigurationSection["ERROR_PARAM"]);
                return false;
            }

            var moneda = await context.Monedas.FindAsync(compra.Codmoneda);
            if (moneda is null)
            {
                ErrorSms = VariablesGlobales.Instance.ConfigurationSection["ERROR_MONEDA"];
                return false;
            }
            if (string.IsNullOrWhiteSpace(_caja) || string.IsNullOrWhiteSpace(_agencia))
            {
                ErrorSms = "No existen los parametros de caja o agencia, favor crearlos";
                return false;
            }

            var existCaja = await context.Cajas.SingleOrDefaultAsync(caja => caja.Codcaja == _caja);
            if (existCaja is null)
            {
                ErrorSms = "No existe la caja, favor crearla";
                return false;
            }

            var existeAgencia = await context.Agencias.SingleOrDefaultAsync(agencia => agencia.Codagencia == _agencia);
            if (existeAgencia is null)
            {
                ErrorSms = "No existe la agencia, favor crearla";
                return false;
            }

            compra.Codcaja = _caja;
            compra.Codagencia = _agencia;
            var modCaja = await context.Mcajas.SingleOrDefaultAsync(mcaja => mcaja.Codagencia == _agencia && mcaja.Codcaja == _caja && mcaja.Estado == 1);
            if (modCaja is null)
            {
                ErrorSms = "No hay caja activa para realizar la operación";
                return false;
            }

            if (modCaja.Fecha!.Value.Date.CompareTo(DateTime.Now.Date) < 0)
            {
                ErrorSms = "No ha aperturado caja para el día de hoy. Aperture caja para realizar movimientos.";
                return false;
            }

            if (param.Dolares == compra.Codmoneda)
            {
                if (decimal.Compare(modCaja.SfinalExt!.Value, decimal.Zero) <= 0)
                {
                    ErrorSms = "No hay saldo disponible en moneda extranjera en caja para realizar la compra";
                    return false;
                }
            }
            else
            {
                if (decimal.Compare(modCaja.Sfinal!.Value, decimal.Zero) <= 0)
                {
                    ErrorSms = "No hay saldo disponible en moneda local en caja para realizar la compra";
                    return false;
                }
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
            compra.Nocontrato = param.Nocontrato;
            param.Nocontrato += 1;
            var tbTipoCambio = await tipoCambioRepository.FindByDateNow();
            var tipoCambioDia = decimal.One;
            

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


            if (decimal.Compare(modCaja.Sfinal.Value, compra.Efectivo) < 0)
            {
                ErrorSms = $"No hay saldo disponible en caja para realizar la compra, Saldo: {modCaja.Sfinal}";
                return false;
            }

            //detalle de caja
            var detaCaja = new Detacaja
            {
                Tipocambio = tipoCambioDia,
                Concepto = $"***Compra: {numeroCompra}***",
                Referencia = numeroCompra,
                Codcaja = _caja,
                Fecha = compra.Fecha,
                Hora = compra.Fecha.ToShortTimeString(),
                Idcaja = modCaja.Idcaja,
                Idmov = param.Idcompras!.Value,
                Codoperador = compra.Usuario,
                Idmoeda = compra.Codmoneda
            };

            if (compra.Codmoneda == param.Dolares)
            {
                detaCaja.EfectivoExt = compra.Efectivo;
                detaCaja.TransferenciaExt = compra.Transferencia;
                detaCaja.ChequeExt = compra.Cheque;
                detaCaja.Efectivo = 0m;
                detaCaja.Transferencia = 0m;
                detaCaja.Cheque = 0m;
            }
            else
            {
                detaCaja.Efectivo = compra.Efectivo;
                detaCaja.Transferencia = compra.Transferencia;
                detaCaja.Cheque = compra.Cheque;
                detaCaja.EfectivoExt = 0m;
                detaCaja.TransferenciaExt = 0m;
                detaCaja.ChequeExt = 0m;
            }
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

            compra.SaldoAdelanto = saldoCordobas;
            compra.SaldoAdelantoDolares = saldoDolares;
            await context.Compras.AddAsync(compra);
            foreach (var detCompra in detaCompra)
            {
                compra.DetCompras.Add(detCompra);
            }

            if (param.Dolares==compra.Codmoneda)
            {
                modCaja.SfinalExt -= compra.Efectivo;
                modCaja.SalidaExt += compra.Efectivo;
            }
            else
            {
                modCaja.Sfinal -= compra.Efectivo;
                modCaja.Salida += compra.Efectivo;
            }
            
            existeAgencia.Numcompra += 1;
            var result = await context.SaveChangesAsync();
            if (result > 0)
            {
                await context.Precios.Where(precio => precio.Codcliente == compra.Codcliente).ExecuteDeleteAsync();
            }

            await transaction.CommitAsync();
            return result > 0;
        }
        catch (Exception e)
        {
            logger.Error(e, "Error al crear la compra");
            var innerMessage = "";
            if (e.InnerException is not null)
            {
                innerMessage = e.InnerException.Message;
            }

            ErrorSms = $"No se pudo guardar la compra debido a error: {e.Message} {innerMessage}";
            await transaction.RollbackAsync();
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
        var find = await context.Compras.Include(compra => compra.DetCompras)
            .Include(compra => compra.Cliente).ThenInclude(cliente => cliente.TipoDocumento)
            .Include(compra => compra.Moneda)
            .Where(compra => compra.Numcompra == numerocompra && compra.Codagencia == _agencia)
            .SingleOrDefaultAsync();
        if (find is not null) return find;
        ErrorSms = $"No existe la compra con codigo {numerocompra} en la sucursal {_agencia}";
        return null;
    }

    public async Task<bool> UpdateByDetaCompra(Compra compra, List<DetCompra> detaCompra)
    {
        try
        {
            var param = await context.Id.FirstOrDefaultAsync();
            if (param is null)
            {
                ErrorSms = "NO hay parametros configurados en sistema";
                return false;
            }

            var mcaja = await context.Mcajas.SingleOrDefaultAsync(mc => mc.Codagencia == _agencia && mc.Codcaja == _caja && mc.Estado > 0 && mc.Fecha!.Value.Date == DateTime.Now.Date);
            if (mcaja is null)
            {
                ErrorSms = "NO se ha aperturado caja para el día de hoy";
                return false;
            }

            var efectivoOriginal = await context.Compras.AsNoTracking().Where(c => c.Numcompra == compra.Numcompra).Select(c => c.Efectivo).SingleOrDefaultAsync();
            var efectivoEditado = compra.Efectivo;
            if (efectivoEditado < efectivoOriginal)
            {
                // Entrada de efectivo (menos efectivo fue usado en la compra)
                var entrada = efectivoOriginal - efectivoEditado;
                mcaja.Entrada = decimal.Add(entrada, mcaja.Entrada!.Value);
                mcaja.Sfinal = decimal.Add(mcaja.Sfinal!.Value, entrada);
            }
            else if (efectivoEditado > efectivoOriginal)
            {
                // Salida de efectivo (más efectivo fue usado en la compra)
                var salida = efectivoEditado - efectivoOriginal;
                mcaja.Salida = decimal.Add(mcaja.Salida!.Value, salida);
                mcaja.Sfinal = decimal.Subtract(mcaja.Sfinal!.Value, salida);
            }

            compra.DetCompras.Clear();
            context.Compras.Update(compra);
            foreach (var detCompra in detaCompra)
            {
                compra.DetCompras.Add(detCompra);
            }


            var detalleCaja = await context.Detacajas.SingleOrDefaultAsync(detacaja => detacaja.Referencia == compra.Numcompra && detacaja.Idcaja==mcaja.Idcaja);
            if (detalleCaja is not null)
            {
                detalleCaja.Efectivo = compra.Efectivo;
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
        catch (Exception e)
        {
            Debug.WriteLine(e);
            ErrorSms = $"No se pudo actualizar los detalles de la compra con el numero de compra {compra.Numcompra}. {e.Message}";
            return false;
        }
    }

    public async Task<bool> AnularCompra(string numeroCompra)
    {
        try
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

            findCompra.Codestado = EstadoCompra.Anulada;
            var mcaja = await maestroCajaRepository.FindByCajaAndAgencia(_caja, _agencia);
            var param = await parametersRepository.RecuperarParametros();
            if (mcaja is null)
            {
                ErrorSms = "No existe el maestro de caja o este no esta aperturado";
                return false;
            }

            var detalleCaja = await context.Detacajas.SingleOrDefaultAsync(detacaja => detacaja.Referencia == $"{numeroCompra}");
            if (detalleCaja is null)
            {
                ErrorSms = $"No existe un detalle de caja con el número de compra {numeroCompra}";
                return false;
            }

            var efectivo = decimal.Zero;
            var tipoCambio = detalleCaja.Tipocambio ?? decimal.One;
            efectivo = param.Cordobas == findCompra.Codmoneda ? findCompra.Efectivo : findCompra.Efectivo * tipoCambio;

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
                Tipocambio = tipoCambio,
                Codoperador = usuario.Codoperador
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
                    adelanto.MontoLetras = Libraries.Utilities.NumeroALetras(sumCordobas);
                }

                if (decimal.Compare(sumDolares, decimal.Zero) > 0)
                {
                    adelanto.Monto = sumDolares;
                    adelanto.Saldo = sumDolares;
                    adelanto.MontoLetras = Libraries.Utilities.NumeroALetras(sumDolares);
                }

                context.ComprasAdelantos.RemoveRange(comprasAdelantos);
                await adelantoRepository.Add(adelanto);
            }

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
        catch (Exception e)
        {
            logger.Error(e, "Error al anular la compra");
            throw;
        }
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
            .OrderByDescending(arg => arg.compra.Fecha)
            .Select(arg => new DtoComprasClientes()
            {
                Numcompra = arg.compra.Numcompra,
                Codcliente = arg.cliente.Codcliente,
                Nombre = arg.cliente.Nombres,
                Apellido = arg.cliente.Apellidos,
                Total = arg.compra.Total,
                Peso = arg.compra.Peso,
                Fecha = arg.compra.Fecha,
                Nocontrato = arg.compra.Nocontrato
            });
        return await result.ToListAsync();
    }

    public Task<List<DtoComprasClientes>> FindComprasClientesFechaAndCerrada(DateTime fecha)
    {
        var parameter = context.Id.AsNoTracking().SingleOrDefault();
        if (parameter is null)
        {
            return new Task<List<DtoComprasClientes>>(() => []);
        }
        var result = context.Compras.AsNoTracking()
            .Where(compra => compra.Codestado == EstadoCompra.Vigente 
                             || compra.Codestado == EstadoCompra.Cerrada 
                             && compra.Fecha.Date<= fecha.Date)
            .Join(context.Clientes.AsNoTracking(), compra => compra.Codcliente, 
                cliente => cliente.Codcliente, (compra, cliente) => new { compra, cliente })
            .Join(context.TipoCambios.AsNoTracking(), arg => arg.compra.Fecha.Date, cambio => cambio.Fecha.Date,
                (arg1, cambio) => new { arg1.compra, arg1.cliente, cambio})
            .OrderByDescending(arg => arg.compra.Fecha)
            .Select(arg => new DtoComprasClientes()
            {
                Numcompra = arg.compra.Numcompra,
                Codcliente = arg.cliente.Codcliente,
                Nombre = arg.cliente.Nombres,
                Apellido = arg.cliente.Apellidos,
                Total = arg.compra.Codmoneda==parameter.Dolares.Value ? decimal.Multiply(arg.compra.Total,arg.cambio.Tipocambio) : arg.compra.Total,
                Peso = arg.compra.Peso,
                Fecha = arg.compra.Fecha,
                Nocontrato = arg.compra.Nocontrato,
                IsChecked = false
            });
        return result.ToListAsync();
    }
    public Task<List<DetalleCompra>> DetalleCompraImprimir(string numcompra)
    {
        return context.DetalleCompras.Where(compra => compra.Numcompra == numcompra).ToListAsync();
    }
}