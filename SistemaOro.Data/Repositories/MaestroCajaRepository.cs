using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class MaestroCajaRepository(IParametersRepository parametersRepository, DataContext context) : IMaestroCajaRepository
{
    public async Task<bool> ValidarCajaAbierta(string caja, string codagencia)
    {
        try
        {
            var mcaja = await context.Mcajas
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
        return context.Mcajas
            .OrderByDescending(mcaja => mcaja.Idcaja)
            .Where(mcaja => mcaja.Codcaja == caja
                            && mcaja.Codagencia == agencia);
    }

    public Task<Mcaja?> FindByCajaAndAgencia(string? caja, string? agencia)
    {
        return context.Mcajas.AsNoTracking()
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
                Sfinal = 0,
                Sinicial = 0,
                Estado = 0,
                Idcaja = 0,
                Fecha = DateTime.Now,
                Codagencia = agencia,
                Codcaja = caja!
            };


            if (xestado.Estado == 1)
            {
                ErrorSms = "Debe cerrar caja para poder continuar";
                return false;
            }

            var parametros = await parametersRepository.RecuperarParametros();
            if (parametros is null)
            {
                ErrorSms = "No hay parametros configurados en el sistema";
                return false;
            }

            var crearM = new Mcaja
            {
                Codcaja = xestado.Codcaja,
                Codagencia = xestado.Codagencia,
                Entrada = 0,
                Salida = 0,
                Fecha = DateTime.Now,
                Estado = 1,
                Sinicial = xestado.Sfinal,
                Sfinal = xestado.Sfinal
            };
            crearM = context.Mcajas.Add(crearM).Entity;
            var dcaja = new Detacaja
            {
                Idcaja = crearM.Idcaja,
                Fecha = DateTime.Now,
                Concepto = "***APERTURA DE CAJA***",
                Transferencia = 0,
                Cheque = 0,
                Hora = DateTime.Now.ToLongTimeString(),
                Idmov = parametros.SaldoAnterior!.Value,
                Referencia = string.Empty,
                Codcaja = caja,
                Tipocambio = decimal.Zero,
                Efectivo = xestado.Fecha!.Value.Day == DateTime.Now.Day ? 0 : crearM.Sfinal,
                Codoperador = VariablesGlobales.Instance.Usuario.Codoperador
            };
            //context.Detacajas.Add(dcaja);
            crearM.Detacajas.Add(dcaja);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            var innerMessage = "";
            if (ex.InnerException is not null)
            {
                innerMessage = ex.InnerException.Message;
            }

            ErrorSms = $"No se realizó la apertura de caja de forma exitosa, intente nuevamente {ex.Message} {innerMessage}";
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
            if (context.ChangeTracker.HasChanges())
            {
                context.ChangeTracker.Clear();
            }

            var prestamos = await ValidarPrestamosPuentes();
            if (prestamos != decimal.Zero)
            {
                ErrorSms =
                    $"La diferencia entre el prestamo puente egreso e ingreso es: {prestamos:#,###,##0.00}, revise las cajas y los debidos prestamos.";
                return false;
            }

            var parametros = await parametersRepository.RecuperarParametros();
            var query = Find(caja, agencia);
            var crearM = await query.FirstOrDefaultAsync(mcaja => mcaja.Estado == 1);
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
            context.Add(detaCaja);
            crearM.Estado = 0;
            await context.SaveChangesAsync();
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

    public async Task<bool> ActualizarDatosMaestroCaja(string codcaja, string codagencia, decimal entrada, decimal salida)
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
            if (context.ChangeTracker.HasChanges())
            {
                context.ChangeTracker.Clear();
            }

            var entrada = decimal.Zero;
            var salida = decimal.Zero;
            var rubro = await context.Rubros.AsNoTracking().SingleOrDefaultAsync(rubro1 => rubro1.Codrubro == movcaja.Codrubro);
            if (rubro is null)
            {
                return 0;
            }

            if (rubro.Naturaleza == 0)
            {
                salida = dcaja.Efectivo!.Value;
            }
            else
            {
                entrada = dcaja.Efectivo!.Value;
            }

            //var actualizarMcaja = await ActualizarDatosMaestroCaja(mocaja.Codcaja, mocaja.Codagencia!, entrada, salida);
            var mcaja = await context.Mcajas.SingleOrDefaultAsync(mcaja1 => mcaja1.Estado == 1);
            if (mcaja is null)
            {
                ErrorSms = "No hay caja abierta para realizar el movimiento";
                return 0;
            }

            mcaja.Entrada += entrada;
            mcaja.Salida += salida;
            mcaja.Sfinal += entrada - salida;
            //if (!actualizarMcaja) return 0;
            var tipoCambio = await context.TipoCambios.AsNoTracking().SingleOrDefaultAsync(cambio => cambio.Fecha == DateTime.Now) ?? new TipoCambio
            {
                Fecha = DateTime.Now,
                PrecioOro = decimal.Zero,
                Tipocambio = decimal.Zero
            };
            dcaja.Tipocambio = tipoCambio.Tipocambio;
            var result = await context.AddAsync(dcaja);
            await context.SaveChangesAsync();
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
        var nat = await context.Movcajas
            .Join(context.Rubros, movcaja => movcaja.Codrubro, rubro => rubro.Codrubro,
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
        return await context.Detacajas
            .Where(detacaja => detacaja.Codcaja == caja)
            .OrderByDescending(detacaja => detacaja.Idcaja)
            .ToListAsync();
    }

    public async Task<decimal> ValidarPrestamosPuentes()
    {
        var param = await parametersRepository.RecuperarParametros();
        try
        {
            var prestamoEgresoQueryable = from dc in context.Detacajas
                where dc.Idmov == param.PrestamoEgreso && dc.Fecha.Year == DateTime.Now.Year &&
                      dc.Fecha.DayOfYear == DateTime.Now.DayOfYear
                group dc by dc.Idmov
                into g
                select g.Sum(detacaja => detacaja.Efectivo);
            var prestamoEgreso = await prestamoEgresoQueryable.SingleAsync() ?? decimal.Zero;

            var prestamoIngresoQueryable = from dc in context.Detacajas
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
            var verDetaCaja = from dc in context.Detacajas
                join mov in context.Movcajas on dc.Idmov equals mov.Idmov
                join ru in context.Rubros on mov.Codrubro equals ru.Codrubro
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
        var query = from detacaja in context.Detacajas
            join movcaja in context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            where detacaja.Idcaja == id
            select new DtoMovimientosCaja(
                movcaja.Descripcion, 
                detacaja.Hora, 
                detacaja.Fecha, 
                detacaja.Concepto, 
                detacaja.Referencia, 
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                detacaja.IdDetaCaja);
        return query.AsNoTracking().ToListAsync();
    }

    public Task<List<DtoMovimientosCaja>> FindAllByFechaDesde(DateTime fechaDesde)
    {
        var query = from detacaja in context.Detacajas
            join movcaja in context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            where detacaja.Fecha.Date == fechaDesde.Date
            select new DtoMovimientosCaja(
                movcaja.Descripcion,
                detacaja.Hora,
                detacaja.Fecha,
                detacaja.Concepto,
                detacaja.Referencia,
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                detacaja.IdDetaCaja
            );
        return query.AsNoTracking().ToListAsync();
    }

    public Task<List<DtoMovimientosCaja>> FindAllByFechaDesdeActiva(DateTime fechaDesde)
    {
        var query = from detacaja in context.Detacajas
            join mcaja in context.Mcajas on detacaja.Idcaja equals mcaja.Idcaja
            join movcaja in context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            where detacaja.Fecha.Date == fechaDesde.Date && mcaja.Estado > 0
            select new DtoMovimientosCaja(
                movcaja.Descripcion,
                detacaja.Hora,
                detacaja.Fecha,
                detacaja.Concepto,
                detacaja.Referencia,
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                detacaja.IdDetaCaja
            );
        return query.AsNoTracking().ToListAsync();
    }

    public Task<List<DtoMovimientosCaja>> FindAllByFechaDesdeAndFechaHasta(DateTime fechaDesde, DateTime fechaHasta)
    {
        var query = from detacaja in context.Detacajas
            join movcaja in context.Movcajas on detacaja.Idmov equals movcaja.Idmov
            join rubro in context.Rubros on movcaja.Codrubro equals rubro.Codrubro
            where detacaja.Fecha.Date >= fechaDesde.Date && detacaja.Fecha.Date <= fechaHasta.Date
            select new DtoMovimientosCaja(
                movcaja.Descripcion,
                detacaja.Hora,
                detacaja.Fecha,
                detacaja.Concepto,
                detacaja.Referencia,
                rubro.Naturaleza == 1 ? detacaja.Efectivo.Value : detacaja.Efectivo.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Cheque.Value : detacaja.Cheque.Value * -1,
                rubro.Naturaleza == 1 ? detacaja.Transferencia.Value : detacaja.Transferencia.Value * -1,
                detacaja.IdDetaCaja
            );
        return query.AsNoTracking().ToListAsync();
    }

    public string ErrorSms { get; private set; } = "";
}