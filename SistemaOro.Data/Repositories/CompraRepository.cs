using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NLog;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;
using SistemaOro.Data.Libraries;
using System;
using System.Diagnostics;

namespace SistemaOro.Data.Repositories;

public class CompraRepository(
    IAdelantosRepository adelantoRepository,
    IParametersRepository parametersRepository,
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

    public async Task<bool> Create(Compra compra, List<DetCompra> detaCompra, FormaPago? formaPago,
        List<Adelanto>? listaAdelantos = null, List<CierrePrecio>? listaPreciosaCerrar = null)
    {
        //await using var transaction = await context.Database.BeginTransactionAsync();
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

            var monedaLocal = await context.Monedas.FindAsync(param.Cordobas);
            if (monedaLocal is null)
            {
                ErrorSms = VariablesGlobales.Instance.ConfigurationSection["ERROR_MONEDA"];
                return false;
            }

            var monedaExt = await context.Monedas.FindAsync(param.Dolares);
            if (monedaExt is null)
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
            var modCaja = await context.Mcajas.SingleOrDefaultAsync(mcaja =>
                mcaja.Codagencia == _agencia && mcaja.Codcaja == _caja && mcaja.Estado == 1);
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

            var validarEfectivoLocal = decimal.Zero;
            var validarEfectivoExt = decimal.Zero;
            if (formaPago is not null)
            {
                validarEfectivoExt = formaPago.Monto2 ?? decimal.Zero;
                validarEfectivoLocal = formaPago.Monto1 ?? decimal.Zero;
            }
            else
            {
                if (compra.Codmoneda == param.Cordobas)
                {
                    validarEfectivoLocal = compra.Efectivo;
                }
                else
                {
                    validarEfectivoExt = compra.Efectivo;
                }
            }

            logger.Info($"Monto total a validar, Local: {validarEfectivoLocal:F2}; Ext: {validarEfectivoExt:F2}");

            if (validarEfectivoLocal > modCaja.Sfinal!.Value || validarEfectivoExt > modCaja.SfinalExt!.Value)
            {
                ErrorSms = "No hay saldo disponible en caja para realizar la compra";
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
            compra.Nocontrato = param.Nocontrato;
            param.Nocontrato += 1;
            var tbTipoCambio = tipoCambioRepository.FindByDateNow();
            var tipoCambioDia = decimal.One;


            if (tbTipoCambio is not null)
            {
                tipoCambioDia = tbTipoCambio.Tipocambio;
            }

            var adelantosPorClientes = await adelantoRepository.ListarAdelantosPorClientes(compra.Codcliente);
            if (adelantosPorClientes.Count > 0)
            {
                saldoCordobas = adelantosPorClientes
                    .Where(adelanto => adelanto.Saldo > 0 && adelanto.Codmoneda == param.Cordobas)
                    .Select(adelanto => adelanto.Saldo).Sum();
                saldoDolares = adelantosPorClientes
                    .Where(adelanto => adelanto.Saldo > 0 && adelanto.Codmoneda == param.Dolares)
                    .Select(adelanto => adelanto.Saldo).Sum();
            }


            var listTmpPrecios = await context.Tmpprecios.Where(tmpprecio => tmpprecio.Codcliente == compra.Codcliente)
                .ToListAsync();
            if (listTmpPrecios.Count > 0)
            {
                var listDetacierre = new List<Detacierre>();
                foreach (var tmpprecio in listTmpPrecios)
                {
                    var findByIdCierrePrecio =
                        await context.CierrePrecios.SingleOrDefaultAsync(c => c.Codcierre == tmpprecio.Codcierre); // cierrePrecioRepository.GetByIdAsync(tmpprecio.Codcierre);
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
                            findAdelanto.Numcompra = findAdelanto.Numcompra.Length <= 0
                                ? $"{_agencia}-{compra.Numcompra}"
                                : $"{findAdelanto.Numcompra}; {_agencia}-{compra.Numcompra}";
                        }

                        await context.ComprasAdelantos.AddAsync(comprasAdelantos);
                    }
                }
            }

            compra.SaldoAdelanto = saldoCordobas;
            compra.SaldoAdelantoDolares = saldoDolares;
            await context.Compras.AddAsync(compra);

            if (formaPago is not null)
            {
                // Asignar valores a formaPago
                formaPago.Tipocambio = tipoCambioDia;
                formaPago.Numcompra = compra.Numcompra;
                formaPago.Codagencia = compra.Codagencia;
                formaPago.Codcaja = compra.Codcaja;
                formaPago.Total = compra.Total;
                formaPago.Codusuario = _usuario.Codoperador;
                await context.FormaPagos.AddAsync(formaPago);

                var detaCaja1 = new Detacaja
                {
                    Tipocambio = tipoCambioDia,
                    Concepto = $"INGRESO FORMA PAGO LOCAL {numeroCompra}",
                    Referencia = numeroCompra,
                    Codcaja = _caja,
                    Fecha = compra.Fecha,
                    Hora = compra.Fecha.ToShortTimeString(),
                    Idcaja = modCaja.Idcaja,
                    Idmov = param.Idcompras!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = param.Cordobas!.Value,
                    Efectivo = formaPago.Monto1,
                    Transferencia = compra.Codmoneda == param.Cordobas ? compra.Transferencia : 0m,
                    Cheque = compra.Codmoneda == param.Cordobas ? compra.Cheque : 0m,
                    EfectivoExt = 0m,
                    TransferenciaExt = 0m,
                    ChequeExt = 0m
                };
                var detaCaja2 = new Detacaja
                {
                    Tipocambio = tipoCambioDia,
                    Concepto = $"INGRESO FORMA PAGO EXT {numeroCompra}",
                    Referencia = numeroCompra,
                    Codcaja = _caja,
                    Fecha = compra.Fecha,
                    Hora = compra.Fecha.ToShortTimeString(),
                    Idcaja = modCaja.Idcaja,
                    Idmov = param.Idcompras!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = param.Dolares!.Value,
                    Efectivo = 0m,
                    Transferencia = 0m,
                    Cheque = 0m,
                    EfectivoExt = formaPago.Monto2,
                    TransferenciaExt = compra.Codmoneda == param.Dolares ? compra.Transferencia : 0m,
                    ChequeExt = compra.Codmoneda == param.Dolares ? compra.Cheque : 0m
                };

                // Crear detalles de caja
                var detallesCaja = new List<Detacaja> { detaCaja1, detaCaja2 };

                await context.Detacajas.AddRangeAsync(detallesCaja);

                // Actualizar caja
                ActualizarModCaja(modCaja, formaPago.Monto1 ?? decimal.Zero, formaPago.Monto2 ?? decimal.Zero);
            }
            else
            {
                var detaCaja = new Detacaja
                {
                    Tipocambio = tipoCambioDia,
                    Concepto = $"INGRESO COMPRA {numeroCompra}",
                    Referencia = numeroCompra,
                    Codcaja = _caja,
                    Fecha = compra.Fecha,
                    Hora = compra.Fecha.ToShortTimeString(),
                    Idcaja = modCaja.Idcaja,
                    Idmov = param.Idcompras!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = compra.Codmoneda,
                    //moneda local
                    Efectivo = compra.Codmoneda == param.Cordobas ? compra.Efectivo : 0m,
                    Transferencia = compra.Codmoneda == param.Cordobas ? compra.Transferencia : 0m,
                    Cheque = compra.Codmoneda == param.Cordobas ? compra.Cheque : 0m,
                    //moneda extranjera
                    EfectivoExt = compra.Codmoneda == param.Dolares ? compra.Efectivo : 0m,
                    TransferenciaExt = compra.Codmoneda == param.Dolares ? compra.Transferencia : 0m,
                    ChequeExt = compra.Codmoneda == param.Dolares ? compra.Cheque : 0m
                };

                await context.Detacajas.AddAsync(detaCaja);

                // Actualizar caja con los valores de compra
                if (param.Dolares == compra.Codmoneda)
                    ActualizarModCaja(modCaja, 0m, compra.Efectivo);
                else
                    ActualizarModCaja(modCaja, compra.Efectivo, 0m);
            }

            foreach (var detCompra in detaCompra)
            {
                compra.DetCompras.Add(detCompra);
            }


            existeAgencia.Numcompra += 1;
            var preciosDelete = context.Precios.Where(precio => precio.Codcliente == compra.Codcliente);
            if (preciosDelete.Any())
            { 
                context.Precios.RemoveRange(preciosDelete);
            }
            //var result = await context.SaveChangesAsync();
            //if (result > 0)
            //{
            //    await context.Precios.Where(precio => precio.Codcliente == compra.Codcliente).ExecuteDeleteAsync();
            //}

            //await transaction.CommitAsync();
            //return result > 0;
            return true;
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
            //await transaction.RollbackAsync();
            return false;
        }
    }

    // Método auxiliar para crear un Detacaja
    private Detacaja CrearDetacaja(Compra compra, Mcaja modCaja, Id param, string numeroCompra, decimal tipoCambio,
        decimal efectivo, decimal efectivoExt, int codmoneda = 0)
    {
        return new Detacaja
        {
            Tipocambio = tipoCambio,
            Concepto = $"***Compra: {numeroCompra}***",
            Referencia = numeroCompra,
            Codcaja = _caja,
            Fecha = compra.Fecha,
            Hora = compra.Fecha.ToShortTimeString(),
            Idcaja = modCaja.Idcaja,
            Idmov = param.Idcompras!.Value,
            Codoperador = compra.Usuario,
            Idmoeda = codmoneda,
            Efectivo = efectivo,
            EfectivoExt = efectivoExt,
            Transferencia = 0m,
            Cheque = 0m,
            TransferenciaExt = 0m,
            ChequeExt = 0m
        };
    }

// Método auxiliar para actualizar modCaja
    void ActualizarModCaja(Mcaja modCaja, decimal montoLocal, decimal montoExt)
    {
        modCaja.Sfinal -= montoLocal;
        modCaja.Salida += montoLocal;
        modCaja.SfinalExt -= montoExt;
        modCaja.SalidaExt += montoExt;
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
        var numCompra = await context.Agencias.Where(agencia => agencia.Codagencia == _agencia)
            .Select(agencia => agencia.Numcompra).SingleAsync();
        return numCompra is null ? "" : numCompra.Value.ToString();
    }

    public string? ErrorSms { get; private set; } = string.Empty;

    public async Task<List<DetCompra>> FindDetaCompra(string numcompra)
    {
        return await context.DetCompras.Where(compra => compra.Numcompra == numcompra && compra.Codagencia == _agencia)
            .OrderBy(compra => compra.Linea).ToListAsync();
    }

    public async Task<Compra?> FindById(string numerocompra)
    {
        var find = await context.Compras.Include(compra => compra.DetCompras)
            .Include(compra => compra.Cliente).ThenInclude(cliente => cliente.TipoDocumento)
            .Include(compra => compra.Moneda)
            .Where(compra => compra.Numcompra == numerocompra && compra.Codagencia == _agencia)
            .SingleOrDefaultAsync();
        return find;
    }

    public async Task<bool> UpdateByDetaCompra(Compra compra, List<DetCompra> detaCompra, FormaPago? formaPago1)
    {
        try
        {
            var param = await context.Id.FirstOrDefaultAsync();
            if (param is null)
            {
                ErrorSms = "NO hay parametros configurados en sistema";
                return false;
            }

            var mcaja = await context.Mcajas.SingleOrDefaultAsync(mc =>
                mc.Codagencia == _agencia && mc.Codcaja == _caja && mc.Estado == 1 &&
                mc.Fecha!.Value.Date == DateTime.Now.Date);
            if (mcaja is null)
            {
                ErrorSms = "NO se ha aperturado caja para el día de hoy";
                return false;
            }

            var esCordobas = compra.Codmoneda == param.Cordobas;
            var esDolares = compra.Codmoneda == param.Dolares;
            var detalleCajas = new List<Detacaja>();
            var formaPago = await context.FormaPagos.SingleOrDefaultAsync(pago => pago.Numcompra == compra.Numcompra);
            var compraAnterior = await context.Compras.AsNoTracking().SingleOrDefaultAsync(c => c.Numcompra == compra.Numcompra);
            if (compraAnterior is null)
            {
                ErrorSms = $"NO EXISTE LA COMPRA {compra.Numcompra} A ACTUALIZAR, FAVOR INTENTAR NUEVAMENTE";
                return false;
            }

            if (formaPago is not null)
            {
                var detaCaja1 = new Detacaja
                {
                    Tipocambio = mcaja.Tipocambio,
                    Concepto = $"REVERSION DE FORMA PAGO LOCAL {compra.Numcompra}",
                    Referencia = compra.Numcompra,
                    Codcaja = _caja,
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.ToShortTimeString(),
                    Idcaja = mcaja.Idcaja,
                    Idmov = param.AnularCompra!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = param.Cordobas,
                    Efectivo = formaPago.Monto1,
                    EfectivoExt = 0m,
                    Transferencia = esCordobas ? compraAnterior.Transferencia : 0m,
                    Cheque = esCordobas ? compraAnterior.Cheque : 0m,
                    TransferenciaExt = 0m,
                    ChequeExt = 0m
                };
                var detaCaja2 = new Detacaja
                {
                    Tipocambio = mcaja.Tipocambio,
                    Concepto = $"REVERSION DE FORMA PAGO EXT {compra.Numcompra}",
                    Referencia = compra.Numcompra,
                    Codcaja = _caja,
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.ToShortTimeString(),
                    Idcaja = mcaja.Idcaja,
                    Idmov = param.AnularCompra!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = param.Dolares,
                    Efectivo = 0m,
                    EfectivoExt = formaPago.Monto2,
                    Transferencia = 0m,
                    Cheque = 0m,
                    TransferenciaExt = esDolares ? compraAnterior.Transferencia : 0m,
                    ChequeExt = esDolares ? compraAnterior.Cheque : 0m
                };
                detalleCajas.Add(detaCaja1);
                detalleCajas.Add(detaCaja2);

                mcaja.Entrada += formaPago.Monto1;
                mcaja.Sfinal += formaPago.Monto1;

                mcaja.EntradaExt += formaPago.Monto2;
                mcaja.SfinalExt += formaPago.Monto2;

                if (formaPago1 is null)
                {
                    context.Remove(formaPago);
                }
            }
            else
            {
                if (compraAnterior.Codmoneda == param.Cordobas)
                {
                    mcaja.Entrada += compraAnterior.Efectivo;
                    mcaja.Sfinal += compraAnterior.Efectivo;
                }

                if (compraAnterior.Codmoneda == param.Dolares)
                {
                    mcaja.EntradaExt += compraAnterior.Efectivo;
                    mcaja.SfinalExt += compraAnterior.Efectivo;
                }

                var detaCajaPrincipal = new Detacaja
                {
                    Tipocambio = mcaja.Tipocambio,
                    Concepto = $"REVERSION DE COMPRA {compra.Numcompra} POR ACTUALIZACION",
                    Referencia = compra.Numcompra,
                    Codcaja = _caja,
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.ToShortTimeString(),
                    Idcaja = mcaja.Idcaja,
                    Idmov = param.AnularCompra!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = compraAnterior.Codmoneda,

                    // Cordobas
                    Efectivo = esCordobas ? compraAnterior.Efectivo : 0m,
                    Transferencia = esCordobas ? compraAnterior.Transferencia : 0m,
                    Cheque = esCordobas ? compraAnterior.Cheque : 0m,

                    // Dólares
                    EfectivoExt = esDolares ? compraAnterior.Efectivo : 0m,
                    TransferenciaExt = esDolares ? compraAnterior.Transferencia : 0m,
                    ChequeExt = esDolares ? compraAnterior.Cheque : 0m
                };

                detalleCajas.Add(detaCajaPrincipal);
            }

            if (formaPago1 is not null)
            {
                var detaCaja3 = new Detacaja
                {
                    Tipocambio = mcaja.Tipocambio,
                    Concepto = $"INGRESO FORMA PAGO LOCAL {compra.Numcompra}",
                    Referencia = compra.Numcompra,
                    Codcaja = _caja,
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.ToShortTimeString(),
                    Idcaja = mcaja.Idcaja,
                    Idmov = param.Idcompras!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = param.Cordobas,
                    // Cordobas
                    Efectivo = formaPago1.Monto1,
                    Transferencia = esCordobas ? compra.Transferencia : 0m,
                    Cheque = esCordobas ? compra.Cheque : 0m,

                    // Dólares
                    EfectivoExt = 0m,
                    TransferenciaExt = 0m,
                    ChequeExt = 0m
                };
                var detaCaja4 = new Detacaja
                {
                    Tipocambio = mcaja.Tipocambio,
                    Concepto = $"INGRESO FORMA PAGO EXT {compra.Numcompra}",
                    Referencia = compra.Numcompra,
                    Codcaja = _caja,
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.ToShortTimeString(),
                    Idcaja = mcaja.Idcaja,
                    Idmov = param.Idcompras!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = param.Dolares,
                    // Cordobas
                    Efectivo = 0m,
                    Transferencia = 0m,
                    Cheque = 0m,

                    // Dólares
                    EfectivoExt = formaPago1.Monto2,
                    TransferenciaExt = esDolares ? compra.Transferencia : 0m,
                    ChequeExt = esDolares ? compra.Cheque : 0m
                };
                mcaja.Salida += formaPago1.Monto1;
                mcaja.Sfinal -= formaPago1.Monto1;

                mcaja.SalidaExt += formaPago1.Monto2;
                mcaja.SfinalExt -= formaPago1.Monto2;

                detalleCajas.Add(detaCaja3);
                detalleCajas.Add(detaCaja4);
                await context.FormaPagos.AddAsync(formaPago1);
            }
            else
            {
                //ingresamos el nuevo detalle de caja
                if (esCordobas)
                {
                    mcaja.Salida += compra.Efectivo;
                    mcaja.Sfinal -= compra.Efectivo;
                }

                if (esDolares)
                {
                    mcaja.SalidaExt += compra.Efectivo;
                    mcaja.SfinalExt -= compra.Efectivo;
                }

                var detaCajaPrincipal = new Detacaja
                {
                    Tipocambio = mcaja.Tipocambio,
                    Concepto = $"INGRESO DE COMPRA {compra.Numcompra} POR ACTUALIZACION",
                    Referencia = compra.Numcompra,
                    Codcaja = _caja,
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.ToShortTimeString(),
                    Idcaja = mcaja.Idcaja,
                    Idmov = param.Idcompras!.Value,
                    Codoperador = compra.Usuario,
                    Idmoeda = compra.Codmoneda,

                    // Cordobas
                    Efectivo = esCordobas ? compra.Efectivo : 0m,
                    Transferencia = esCordobas ? compra.Transferencia : 0m,
                    Cheque = esCordobas ? compra.Cheque : 0m,

                    // Dólares
                    EfectivoExt = esDolares ? compra.Efectivo : 0m,
                    TransferenciaExt = esDolares ? compra.Transferencia : 0m,
                    ChequeExt = esDolares ? compra.Cheque : 0m
                };

                detalleCajas.Add(detaCajaPrincipal);
            }

            var detCompras = context.DetCompras.Where(c => c.Numcompra == compra.Numcompra);
            await context.Detacajas.AddRangeAsync(detalleCajas);
            context.RemoveRange(detCompras);
            context.Compras.Update(compra);
            foreach (var detCompra in detaCompra)
            {
                compra.DetCompras.Add(detCompra);
            }

            //var result = await context.SaveChangesAsync() > 0;
            //if (result)
            //{
            //    return true;
            //}

            //ErrorSms = $"No se pudo actualizar los detalles de la compra con el numero de compra {compra.Numcompra}";
            //return false;
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            ErrorSms =
                $"No se pudo actualizar los detalles de la compra con el numero de compra {compra.Numcompra}. {e.Message}";
            throw;
        }
    }

    public async Task<bool> AnularCompra(string numeroCompra)
    {
        try
        {
            var usuario = VariablesGlobales.Instance.Usuario;
            if (usuario is null)
            {
                throw new EntityValidationException(
                    "No existe el usuario en el sistema o no se ha iniciado adecuadamente la sesion");
            }


            var findCompra = await FindById(numeroCompra);
            if (findCompra is null)
            {
                ErrorSms = $"No existe la compra con el codigo {numeroCompra} a anular, intente nuevamente";
                return false;
            }

            findCompra.Codestado = EstadoCompra.Anulada;
            var mcaja = context.Mcajas.SingleOrDefault(mc => mc.Codcaja == _caja
                                                                        && mc.Codagencia == _agencia
                                                                        && mc.Fecha!.Value.Date == DateTime.Now.Date
                                                                        && mc.Estado == 1);
            if (mcaja is null)
            {
                ErrorSms = "No existe el maestro de caja o este no esta aperturado";
                return false;
            }

            var param = await parametersRepository.RecuperarParametros();
            if (param is null)
            {
                ErrorSms = "No hay parametros en la base de datos";
                return false;
            }

            var tipoCambioCompra =
                await context.TipoCambios.FirstOrDefaultAsync(tc => tc.Fecha.Date == findCompra.Fecha.Date);
            if (tipoCambioCompra is null)
            {
                ErrorSms = $"No existe un tipo de cambio para el número de compra {numeroCompra}";
                return false;
            }

            var formaPago =
                await context.FormaPagos.SingleOrDefaultAsync(formaPago => formaPago.Numcompra == numeroCompra);
            if (formaPago is not null)
            {
                if (formaPago.Monto1 is not null && formaPago.Monto1.Value > 0)
                {
                    var detaCajaLocal = new Detacaja
                    {
                        Cheque = decimal.Zero,
                        Efectivo = formaPago.Monto1,
                        Transferencia = decimal.Zero,
                        ChequeExt = decimal.Zero,
                        EfectivoExt = decimal.Zero,
                        TransferenciaExt = decimal.Zero,
                        Idmoeda = param.Cordobas ?? 0,
                        Codcaja = formaPago.Codcaja,
                        Concepto = $@"***REVERTIR FORMA PAGO COMPRA: {numeroCompra}***",
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.ToLongTimeString(),
                        Idmov = param.AnularAdelanto ?? 0,
                        Referencia = $@"Movimiento realizado por anulación de compra número: {numeroCompra}",
                        Idcaja = mcaja.Idcaja,
                        Tipocambio = formaPago.Tipocambio,
                        Codoperador = usuario.Codoperador
                    };
                    await context.Detacajas.AddAsync(detaCajaLocal);
                    mcaja.Sfinal += formaPago.Monto1;
                    mcaja.Entrada += formaPago.Monto1;
                }

                if (formaPago.Monto2 is not null && formaPago.Monto2.Value > 0)
                {
                    var detaCajaEXt = new Detacaja
                    {
                        Cheque = decimal.Zero,
                        Efectivo = decimal.Zero,
                        Transferencia = decimal.Zero,
                        ChequeExt = decimal.Zero,
                        EfectivoExt = formaPago.Monto2,
                        TransferenciaExt = decimal.Zero,
                        Idmoeda = param.Dolares ?? 0,
                        Codcaja = formaPago.Codcaja,
                        Concepto = $@"***REVERTIR FORMA PAGO COMPRA: {numeroCompra}***",
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.ToLongTimeString(),
                        Idmov = param.AnularAdelanto ?? 0,
                        Referencia = $@"Movimiento realizado por anulación de compra número: {numeroCompra}",
                        Idcaja = mcaja.Idcaja,
                        Tipocambio = formaPago.Tipocambio,
                        Codoperador = usuario.Codoperador
                    };
                    mcaja.SfinalExt += formaPago.Monto2;
                    mcaja.EntradaExt += formaPago.Monto2;
                    await context.Detacajas.AddAsync(detaCajaEXt);
                }
            }
            else
            {
                var tipoCambio = tipoCambioCompra.Tipocambio;
                var nuevoDetaCaja = new Detacaja
                {
                    Cheque = param.Cordobas == findCompra.Codmoneda ? findCompra.Cheque : 0m,
                    ChequeExt = param.Dolares == findCompra.Codmoneda ? findCompra.Cheque : 0m,
                    Efectivo = param.Cordobas == findCompra.Codmoneda ? findCompra.Efectivo : 0m,
                    Transferencia = param.Cordobas == findCompra.Codmoneda ? findCompra.Transferencia : 0m,
                    EfectivoExt = param.Dolares == findCompra.Codmoneda ? findCompra.Efectivo : 0m,
                    TransferenciaExt = param.Dolares == findCompra.Codmoneda ? findCompra.Transferencia : 0m,
                    Codcaja = _caja,
                    Concepto = $@"***REVERTIR COMPRA: {numeroCompra}***",
                    Fecha = DateTime.Now,
                    Hora = DateTime.Now.ToLongTimeString(),
                    Idmov = param.AnularAdelanto!.Value,
                    Referencia = $@"Movimiento realizado por anulación de compra número: {numeroCompra}",
                    Idcaja = mcaja.Idcaja,
                    Tipocambio = tipoCambio,
                    Codoperador = usuario.Codoperador,
                    Idmoeda = findCompra.Codmoneda
                };
                if (findCompra.Codmoneda == param.Cordobas)
                {
                    mcaja.Entrada = decimal.Add(findCompra.Efectivo, mcaja.Entrada!.Value);
                    mcaja.Sfinal = decimal.Add(findCompra.Efectivo, mcaja.Sfinal!.Value);
                }
                else
                {
                    mcaja.EntradaExt = decimal.Add(findCompra.Efectivo, mcaja.EntradaExt!.Value);
                    mcaja.SfinalExt = decimal.Add(findCompra.Efectivo, mcaja.SfinalExt!.Value);
                }

                await context.Detacajas.AddAsync(nuevoDetaCaja);
            }


            var detalleCierres = await context.Detacierres
                .Where(detacierre =>
                    detacierre.Numcompra == numeroCompra && detacierre.Codagencia == findCompra.Codagencia)
                .ToListAsync();
            if (detalleCierres.Count > 0)
            {
                foreach (var detacierre in detalleCierres)
                {
                    var cierre = await context.CierrePrecios.SingleOrDefaultAsync(c => c.Codcierre == detacierre.Codcierre);
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
            .Join(context.Clientes.AsNoTracking(), compra => compra.Codcliente, cliente => cliente.Codcliente,
                (compra, cliente) => new { compra, cliente })
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
                Nocontrato = arg.compra.Nocontrato,
                Firma = arg.compra.Firma
            });
        return await result.ToListAsync();
    }

    public Task<int> CountCompras()
    {
       return context.Compras.AsNoTracking()
            .Where(compra => compra.Codestado == EstadoCompra.Vigente || compra.Codestado == EstadoCompra.Cerrada)
            .CountAsync();
    }

    public IQueryable<DtoComprasClientes> FindComprasClientesPaged(){
        var result = context.Compras.AsNoTracking()
            .Where(compra => compra.Codestado == EstadoCompra.Vigente || compra.Codestado == EstadoCompra.Cerrada)
            .Join(context.Clientes.AsNoTracking(), compra => compra.Codcliente, cliente => cliente.Codcliente,
                (compra, cliente) => new { compra, cliente })
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
                Nocontrato = arg.compra.Nocontrato,
                Firma = arg.compra.Firma
            });
        return result;
    }

    public Task<List<DtoComprasClientes>> FindComprasClientesFechaAndCerrada(DateTime fecha)
    {
        var parameter = context.Id.AsNoTracking().SingleOrDefault();
        if (parameter is null)
        {
            return new Task<List<DtoComprasClientes>>(() => []);
        }

        var query = from c in context.Compras
            join cl in context.Clientes on c.Codcliente equals cl.Codcliente
            join tc in context.TipoCambios on c.Fecha.Date equals tc.Fecha.Date into tcJoin
            from tc in tcJoin.DefaultIfEmpty()
            where c.Fecha.Date <= fecha.Date &&
                  c.Codestado == EstadoCompra.Cerrada ||
                  c.Codestado == EstadoCompra.Vigente &&
                  c.Codagencia == _agencia
            select new DtoComprasClientes()
            {
                Numcompra = c.Numcompra,
                Codcliente = c.Codcliente,
                Nombre = cl.Nombres,
                Apellido = cl.Apellidos,
                Total = c.Total,
                Codmoneda = c.Codmoneda,
                Peso = c.Peso,
                Nocontrato = c.Nocontrato,
                TipoCambio = tc != null ? tc.Tipocambio : 0m,
                Fecha = c.Fecha
            };

        return query.ToListAsync();
    }

    public FormaPago? FindFormaPago(string numcompra)
    {
        return context.FormaPagos.AsNoTracking().SingleOrDefault(formaPago => formaPago.Numcompra == numcompra);
    }

    public Task<List<ViewComprasOnza>> ListadoComprasOnzas(DateTime fechaInicial, DateTime fechaFinal)
    {
        return context.ViewComprasOnzas.Where(c =>
                c.Fecha.Value.Date >= fechaInicial.Date &&
                c.Fecha.Value.Date <= fechaFinal.Date)
            .OrderByDescending(c => c.Fecha).ThenBy(c => c.Numcompra)
            .ToListAsync();
    }

    public Task<List<DetalleCompra>> DetalleCompraImprimirPorCliente(string codcliente)
    {
        return context.DetalleCompras.Where(dc => dc.Codcliente == codcliente)
            .OrderByDescending(dc => dc.FechaCompra)
            .ToListAsync();
    }

    public Task<List<DetalleCompra>> DetalleCompraImprimirPorClientePorFecha(string codcliente, DateTime fechaInicial,
        DateTime fechaFinal)
    {
        return context.DetalleCompras.Where(
                dc => dc.Codcliente == codcliente
                      && dc.FechaCompra.Date >= fechaInicial.Date
                      && dc.FechaCompra.Date <= fechaFinal.Date)
            .OrderByDescending(dc => dc.FechaCompra)
            .ToListAsync();
    }

    public Task<List<DetalleCompra>> DetalleCompraImprimir(string numcompra)
    {
        return context.DetalleCompras.Where(compra => compra.Numcompra == numcompra).ToListAsync();
    }
}