using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class MaestroCajaRepository : IMaestroCajaRepository
{

    private readonly string _caja;
    private readonly string _agencia;
    private readonly IParametersRepository _parametersRepository;
    private readonly DataContext _context;

    public MaestroCajaRepository(IParametersRepository parametersRepository, DataContext context)
    {
        _parametersRepository = parametersRepository;
        _context = context;
        _caja = Configuration.ConfiguracionGeneral.Caja ?? string.Empty;
        _agencia= Configuration.ConfiguracionGeneral.Agencia ?? string.Empty;
    }

    public async Task<bool> ValidarCajaAbierta(string caja, string codagencia)
    {
        try
        {
            var mcaja = await _context.Mcajas
                .SingleOrDefaultAsync(mcaja1 => mcaja1.Codcaja == caja
                                                && mcaja1.Codagencia == codagencia
                                                && mcaja1.Estado == 1);
            if (mcaja is not null) return mcaja.Fecha!.Value.Date == DateTime.Now.Date;
            ErrorSms = $"No existe el maestro de caja con el codigo de caja {caja}";
            return false;
        }
        catch (Exception e)
        {
            ErrorSms = e.Message;
            return false;
        }
    }

    private IQueryable<Mcaja> Find(string? caja, string? agencia)
    {
        return _context.Mcajas
            .OrderByDescending(mcaja => mcaja.Idcaja)
            .Where(mcaja => mcaja.Codcaja == caja
                            && mcaja.Codagencia == agencia);
    }

    public Task<Mcaja?> FindByCajaAndAgencia(string? caja, string? agencia)
    {
        return _context.Mcajas.AsNoTracking()
            .SingleOrDefaultAsync(mcaja => mcaja.Estado == 1
                                           && mcaja.Codagencia == agencia
                                           && mcaja.Codcaja == caja);
    }

    public async Task<bool> EstadoCaja(string? caja, string? agencia)
    {
        try
        {
            var findQuery = Find(caja, agencia);
            var mcaja = await findQuery.FirstOrDefaultAsync();
            if (mcaja is not null) return mcaja.Estado == 1;
            ErrorSms = $"No existe el maestro de caja con el codigo de caja {caja}";
            return false;
        }
        catch (Exception e)
        {
            ErrorSms = e.Message;
            return false;
        }
    }

    public async Task<bool> AbrirCaja(string? caja, string? agencia)
    {
        try
        {
            var query = Find(caja, agencia);
            var xestado = query.OrderByDescending(mcaja => mcaja.Idcaja).FirstOrDefault() ?? new Mcaja
            {
                Sfinal = 0m,
                Sinicial = 0m,
                Estado = 0,
                Idcaja = 0,
                SfinalExt = 0m,
                SinicialExt = 0m,
                Fecha = DateTime.Now,
                Codagencia = agencia,
                Codcaja = caja!
            };


            if (xestado.Estado == 1)
            {
                ErrorSms = "Debe cerrar caja para poder continuar";
                return false;
            }

            var parametros = await _parametersRepository.RecuperarParametros();
            if (parametros is null)
            {
                ErrorSms = "No hay parametros configurados en el sistema";
                return false;
            }

            var crearM = new Mcaja
            {
                Codcaja = caja,
                Codagencia = agencia,
                Entrada = 0m,
                EntradaExt = 0m,
                Salida = 0m,
                SalidaExt = 0m,
                Fecha = DateTime.Now,
                Estado = 1,
                Sinicial = xestado.Sfinal,
                Sfinal = xestado.Sfinal,
                SinicialExt = xestado.SfinalExt ?? 0m,
                SfinalExt = xestado.SfinalExt ?? 0m
            };
            crearM = _context.Mcajas.Add(crearM).Entity;
            var dcajaLocal = new Detacaja
            {
                Idcaja = crearM.Idcaja,
                Fecha = DateTime.Now,
                Concepto = "***APERTURA DE CAJA***",
                Transferencia = 0,
                TransferenciaExt = 0,
                Cheque = 0,
                ChequeExt = 0,
                Hora = DateTime.Now.ToLongTimeString(),
                Idmov = parametros.SaldoAnterior!.Value,
                Referencia = "APERTURA EN MONEDA LOCAL",
                Codcaja = caja,
                Tipocambio = decimal.Zero,
                Efectivo = crearM.Sfinal,
                EfectivoExt = 0m,
                Idmoeda = parametros.Cordobas,
                Codoperador = VariablesGlobales.Instance.Usuario.Codoperador
            };
            var dcajaExt = new Detacaja
            {
                Idcaja = crearM.Idcaja,
                Fecha = DateTime.Now,
                Concepto = "***APERTURA DE CAJA***",
                Transferencia = 0,
                TransferenciaExt = 0,
                Cheque = 0,
                ChequeExt = 0,
                Hora = DateTime.Now.ToLongTimeString(),
                Idmov = parametros.SaldoAnterior!.Value,
                Referencia = "APERTURA EN MONEDA EXTRANJERA",
                Codcaja = caja,
                Tipocambio = decimal.Zero,
                Efectivo = 0m,
                EfectivoExt = crearM.SfinalExt,
                Idmoeda = parametros.Dolares,
                Codoperador = VariablesGlobales.Instance.Usuario.Codoperador
            };
            //context.Detacajas.Add(dcaja);
            crearM.Detacajas.Add(dcajaLocal);
            crearM.Detacajas.Add(dcajaExt);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            var innerMessage = "";
            if (ex.InnerException is not null)
            {
                innerMessage = ex.InnerException.Message;
            }

            ErrorSms =
                $"No se realizó la apertura de caja de forma exitosa, intente nuevamente {ex.Message} {innerMessage}";
            return false;
        }
    }

    public async Task<bool> CerrarCaja(string? caja, string? agencia)
    {
        try
        {
            var usuario = VariablesGlobales.Instance.Usuario;
            if (usuario is null)
            {
                throw new Exception("No hay usuario en la variable global, inicializar");
            }

            if (_context.ChangeTracker.HasChanges())
            {
                _context.ChangeTracker.Clear();
            }

            var prestamos = await ValidarPrestamosPuentes();
            if (prestamos != decimal.Zero)
            {
                ErrorSms =
                    $"La diferencia entre el prestamo puente egreso e ingreso es: {prestamos:#,###,##0.00}, revise las cajas y los debidos prestamos.";
                return false;
            }

            var parametros = await _parametersRepository.RecuperarParametros();
            var query = Find(caja, agencia);
            var crearM = await query.FirstOrDefaultAsync(mcaja => mcaja.Estado == 1 && mcaja.Codagencia== _agencia && mcaja.Codcaja == _caja);
            if (crearM is null || parametros is null)
            {
                ErrorSms = $"No hay caja aperturada para {caja} {agencia}, intente nuevamente o parametros nulos";
                return false;
            }

            var detaCaja = new Detacaja()
            {
                Idcaja = crearM.Idcaja,
                Fecha = DateTime.Now,
                Concepto = "***CIERRE DE CAJA***",
                Efectivo = 0,
                Transferencia = 0,
                Cheque = 0,
                Hora = DateTime.Now.ToLongTimeString(),
                Idmov = parametros.SaldoAnterior!.Value,
                Referencia = $"Cierre de caja: {caja} {DateTime.Now.ToShortDateString()}",
                Codcaja = caja,
                Codoperador = usuario.Codoperador
            };
            _context.Add(detaCaja);
            crearM.Estado = 0;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            var innerMessage = "";
            if (ex.InnerException is not null)
            {
                innerMessage = ex.InnerException.Message;
            }

            ErrorSms = $"Se produjo un error al intentar cerrar la caja: {ex.Message} {innerMessage}";
            return false;
        }
    }

    public async Task<bool> ActualizarDatosMaestroCaja(string codcaja, string codagencia, decimal entrada,
        decimal salida)
    {
        var query = Find(codcaja, codagencia);
        try
        {
            await query.Where(mcaja => mcaja.Estado == 1)
                .ExecuteUpdateAsync(calls =>
                    calls.SetProperty(mcaja => mcaja.Entrada, mcaja => mcaja.Entrada!.Value + entrada)
                        .SetProperty(mcaja => mcaja.Salida, mcaja => mcaja.Salida!.Value + salida)
                        .SetProperty(mcaja => mcaja.Sfinal, mcaja => mcaja.Sfinal!.Value + entrada - salida)
                );
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = $"No se pudo actualizar los datos de la caja, favor intente de nuevo: {e.Message}";
            return false;
        }
    }

    public async Task<int> GuardarDatosDetaCaja(Detacaja dcaja, Movcaja movcaja, Mcaja mocaja)
    {
        try
        {
            if (_context.ChangeTracker.HasChanges())
            {
                _context.ChangeTracker.Clear();
            }

            var entrada = decimal.Zero;
            var salida = decimal.Zero;
            var entradaExt = decimal.Zero;
            var salidaExt = decimal.Zero;
            var rubro = await _context.Rubros.AsNoTracking()
                .SingleOrDefaultAsync(rubro1 => rubro1.Codrubro == movcaja.Codrubro);
            if (rubro is null)
            {
                return 0;
            }

            if (rubro.Naturaleza == 0)
            {
                salida = dcaja.Efectivo ?? 0m;
                salidaExt = dcaja.EfectivoExt ?? 0m;
            }
            else
            {
                entrada = dcaja.Efectivo ?? 0m;
                entradaExt = dcaja.EfectivoExt ?? 0m;
            }

            var param = await _context.Id.AsNoTracking().SingleOrDefaultAsync();
            if (param is null)
            {
                return 0;
            }

            //var actualizarMcaja = await ActualizarDatosMaestroCaja(mocaja.Codcaja, mocaja.Codagencia!, entrada, salida);
            var mcaja = await _context.Mcajas.SingleOrDefaultAsync(mcaja1 =>
                mcaja1.Estado == 1 &&
                mcaja1.Codcaja == _caja &&
                mcaja1.Codagencia == _agencia
             );

            if (mcaja is null)
            {
                ErrorSms = "No hay caja abierta para realiza el movimiento";
                return 0;
            }

            if (param.Dolares == dcaja.Idmoeda)
            {
                mcaja.EntradaExt += entradaExt;
                mcaja.SalidaExt += salidaExt;
                mcaja.SfinalExt += entradaExt - salidaExt;
            }
            else
            {
                mcaja.Entrada += entrada;
                mcaja.Salida += salida;
                mcaja.Sfinal += entrada - salida;
            }

            //if (!actualizarMcaja) return 0;
            var tipoCambio =
                await _context.TipoCambios.AsNoTracking().SingleOrDefaultAsync(cambio => cambio.Fecha == DateTime.Now) ??
                new TipoCambio
                {
                    Fecha = DateTime.Now,
                    PrecioOro = decimal.Zero,
                    Tipocambio = decimal.Zero
                };
            dcaja.Tipocambio = tipoCambio.Tipocambio;
            var result = await _context.AddAsync(dcaja);
            await _context.SaveChangesAsync();
            return result.Entity.IdDetaCaja;
        }
        catch (Exception e)
        {
            var innerMessage = "";
            if (e.InnerException is not null)
            {
                innerMessage = e.InnerException.Message;
            }

            ErrorSms = $"Error al realizar el movimiento en caja: {e.Message} {innerMessage}";
            return 0;
        }
    }

    public async Task<bool> ValidarMovimiento(int idmov)
    {
        var nat = await _context.Movcajas
            .Join(_context.Rubros, movcaja => movcaja.Codrubro, rubro => rubro.Codrubro,
                (movcaja, rubro) => new { movcaja, rubro })
            .Where(arg => arg.movcaja.Idmov == idmov)
            .Select(arg => arg.rubro)
            .FirstOrDefaultAsync();
        if (nat is null)
        {
            ErrorSms = $"No exite el movimiento de caja con el ID {idmov}";
            return false;
        }

        return nat.Naturaleza == 1;
    }

    public async Task<List<Detacaja>> ListarDetaCaja(string caja)
    {
        return await _context.Detacajas
            .Where(detacaja => detacaja.Codcaja == caja)
            .OrderByDescending(detacaja => detacaja.Idcaja)
            .ToListAsync();
    }

    public async Task<decimal> ValidarPrestamosPuentes()
    {
        var param = await _parametersRepository.RecuperarParametros();
        try
        {
            var prestamoEgresoQueryable = from dc in _context.Detacajas
                where dc.Idmov == param.PrestamoEgreso && dc.Fecha.Year == DateTime.Now.Year &&
                      dc.Fecha.DayOfYear == DateTime.Now.DayOfYear
                group dc by dc.Idmov
                into g
                select g.Sum(detacaja => detacaja.Efectivo);
            var prestamoEgreso = await prestamoEgresoQueryable.SingleAsync() ?? decimal.Zero;

            var prestamoIngresoQueryable = from dc in _context.Detacajas
                where dc.Idmov == param.PrestamoIngreso && dc.Fecha.Year == DateTime.Now.Year
                                                        && dc.Fecha.DayOfYear == DateTime.Now.DayOfYear
                group dc by dc.Idmov
                into g
                select g.Sum(detacaja => detacaja.Efectivo);
            var prestamoIngreso = await prestamoIngresoQueryable.SingleAsync() ?? decimal.Zero;
            return decimal.Subtract(prestamoEgreso, prestamoIngreso);
        }
        catch (Exception e)
        {
            ErrorSms = $"No se pudo validar los prestamos puentes realizados: {e.Message}";
            return decimal.Zero;
        }
    }

    public async Task<List<Detacaja>?> RecuperarDetaCajaValores(Mcaja mcaja)
    {
        try
        {
            var verDetaCaja = from dc in _context.Detacajas
                join mov in _context.Movcajas on dc.Idmov equals mov.Idmov
                join ru in _context.Rubros on mov.Codrubro equals ru.Codrubro
                where dc.Idcaja == mcaja.Idcaja
                select new Detacaja
                {
                    Tipocambio = dc.Tipocambio,
                    Codcaja = dc.Codcaja,
                    Fecha = dc.Fecha,
                    Idcaja = dc.Idmov,
                    Cheque = ru.Naturaleza == 1 ? dc.Cheque : dc.Cheque * -1,
                    Efectivo = ru.Naturaleza == 1 ? dc.Efectivo : dc.Efectivo * -1,
                    Transferencia = ru.Naturaleza == 1 ? dc.Transferencia : dc.Transferencia * -1,
                    Codoperador = dc.Codoperador
                };
            return await verDetaCaja.ToListAsync();
        }
        catch (Exception ex)
        {
            ErrorSms = $"No se pudo recuperar los valores de del detalle de caja {mcaja.Idcaja}. Error: {ex.Message}";
            return null;
        }
    }

    public Task<List<DtoMovimientosCaja>> FindAllByIdMaestroCaja(int id)
    {
        var query = from detacaja in _context.Detacajas
            join movcaja in _context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in _context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            join mon in _context.Monedas on detacaja.Idmoeda equals mon.Codmoneda
            where detacaja.Idcaja == id && detacaja.Codcaja==_caja
            select new DtoMovimientosCaja(
                movcaja.Descripcion,
                detacaja.Hora,
                detacaja.Fecha,
                detacaja.Concepto,
                detacaja.Referencia,
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.EfectivoExt.Value : detacaja.EfectivoExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.ChequeExt.Value : detacaja.ChequeExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.TransferenciaExt.Value : detacaja.TransferenciaExt.Value * -1,
                detacaja.IdDetaCaja,
                mon.Simbolo);
        return query.AsNoTracking().ToListAsync();
    }

    public Task<List<DtoMovimientosCaja>> FindAllByFechaDesde(DateTime fechaDesde)
    {
        var query = from detacaja in _context.Detacajas
            join movcaja in _context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in _context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            join mon in _context.Monedas on detacaja.Idmoeda equals mon.Codmoneda
            where detacaja.Fecha.Date == fechaDesde.Date && detacaja.Codcaja == _caja
            select new DtoMovimientosCaja(
                movcaja.Descripcion,
                detacaja.Hora,
                detacaja.Fecha,
                detacaja.Concepto,
                detacaja.Referencia,
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.EfectivoExt.Value : detacaja.EfectivoExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.ChequeExt.Value : detacaja.ChequeExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.TransferenciaExt.Value : detacaja.TransferenciaExt.Value * -1,
                detacaja.IdDetaCaja,
                mon.Simbolo
            );
        return query.AsNoTracking().ToListAsync();
    }

    public Task<List<DtoMovimientosCaja>> FindAllByFechaDesdeActiva(DateTime fechaDesde)
    {
        var query = from detacaja in _context.Detacajas
            join mcaja in _context.Mcajas on detacaja.Idcaja equals mcaja.Idcaja
            join movcaja in _context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in _context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            join mon in _context.Monedas on detacaja.Idmoeda equals mon.Codmoneda
            where detacaja.Fecha.Date == fechaDesde.Date && mcaja.Estado > 0 && mcaja.Codagencia== _agencia && mcaja.Codcaja == _caja
                    select new DtoMovimientosCaja(
                movcaja.Descripcion,
                detacaja.Hora,
                detacaja.Fecha,
                detacaja.Concepto,
                detacaja.Referencia,
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.EfectivoExt.Value : detacaja.EfectivoExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.ChequeExt.Value : detacaja.ChequeExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.TransferenciaExt.Value : detacaja.TransferenciaExt.Value * -1,
                detacaja.IdDetaCaja,
                mon.Simbolo
            );
        return query.AsNoTracking().ToListAsync();
    }

    public Task<List<DtoMovimientosCaja>> FindAllByFechaDesdeAndFechaHasta(DateTime fechaDesde, DateTime fechaHasta)
    {
        var query = from detacaja in _context.Detacajas
            join movcaja in _context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in _context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            join mon in _context.Monedas on detacaja.Idmoeda equals mon.Codmoneda
            where detacaja.Fecha.Date >= fechaDesde.Date && detacaja.Fecha.Date <= fechaHasta.Date && detacaja.Codcaja == _caja
            select new DtoMovimientosCaja(
                movcaja.Descripcion,
                detacaja.Hora,
                detacaja.Fecha,
                detacaja.Concepto,
                detacaja.Referencia,
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.EfectivoExt.Value : detacaja.EfectivoExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.ChequeExt.Value : detacaja.ChequeExt.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.TransferenciaExt.Value : detacaja.TransferenciaExt.Value * -1,
                detacaja.IdDetaCaja,
                mon.Simbolo
            );
        return query.AsNoTracking().ToListAsync();
    }

    public string ErrorSms { get; private set; } = "";
}