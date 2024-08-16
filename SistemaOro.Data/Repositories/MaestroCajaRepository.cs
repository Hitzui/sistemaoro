using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;

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
            if (mcaja is not null) return mcaja.Fecha!.Value.Day == DateTime.Now.Day;
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
            context.Add(crearM);
            await context.SaveChangesAsync();
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
                Efectivo = xestado.Fecha!.Value.Day == DateTime.Now.Day ? 0 : crearM.Sfinal
            };
            context.Add(dcaja);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            ErrorSms = $"No se realizó la apertura de caja de forma exitosa, intente nuevamente {ex.Message}";
            return false;
        }
    }

    public async Task<bool> CerrarCaja(string? caja, string? agencia)
    {
        try
        {
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
                Codcaja = caja
            };
            context.Add(detaCaja);
            crearM.Estado = 0;
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            ErrorSms = $"Se produjo un error al intentar cerrar la caja: {ex.Message}";
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

    public async Task<bool> GuardarDatosDetaCaja(Detacaja dcaja, Movcaja movcaja, Mcaja mocaja)
    {
        var entrada = decimal.Zero;
        var salida = decimal.Zero;
        var rubro = await context.Rubros.AsNoTracking().SingleOrDefaultAsync(rubro1 => rubro1.Codrubro == movcaja.Codrubro);
        if (rubro is null)
        {
            return false;
        }
        if (rubro.Naturaleza==0)
        {
            salida = dcaja.Efectivo!.Value;
        }
        else
        {
            entrada = dcaja.Efectivo!.Value;
        }
        var actualizarMcaja = await ActualizarDatosMaestroCaja(mocaja.Codcaja, mocaja.Codagencia!, entrada, salida);
        if (!actualizarMcaja) return false;
        var tipoCambio = await context.TipoCambios.AsNoTracking().SingleOrDefaultAsync(cambio => cambio.Fecha == DateTime.Now) ?? new TipoCambio
        {
            Fecha = DateTime.Now,
            PrecioOro = decimal.Zero,
            Tipocambio = decimal.One
        };
        dcaja.Tipocambio = tipoCambio.Tipocambio;
        context.Add(dcaja);
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = $"Error: {e.Message} {e.Source}";
            return false;
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
                    Transferencia = ru.Naturaleza == 1 ? dc.Transferencia : dc.Transferencia * -1
                };
            return await verDetaCaja.ToListAsync();
        }
        catch (Exception ex)
        {
            ErrorSms = $"No se pudo recuperar los valores de del detalle de caja {mcaja.Idcaja}. Error: {ex.Message}";
            return null;
        }
    }

    public string ErrorSms { get; private set; } = "";
}