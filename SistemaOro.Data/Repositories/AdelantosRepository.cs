using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class AdelantosRepository(IParametersRepository parametersRepository, IMaestroCajaRepository maestroCajaRepository, DataContext context) : IAdelantosRepository
{
    public string? ErrorSms { get; private set; }

    public async Task<bool> Add(Adelanto adelanto)
    {
        try
        {
            adelanto = context.Add(adelanto).Entity;
            var comprasAdelantos = new ComprasAdelanto
            {
                Numcompra = string.Empty,
                Codcliente = adelanto.Codcliente,
                Codcaja = adelanto.Codcaja!,
                Idadelanto = adelanto.Idsalida,
                Fecha = DateTime.Now,
                Hora = TimeOnly.FromDateTime(DateTime.Now).ToTimeSpan(),
                Monto = adelanto.Monto!.Value,
                Sinicial = decimal.Zero,
                Sfinal = adelanto.Monto!.Value,
                Usuario = VariablesGlobales.Instance.Usuario!.Codoperador,
                Codagencia = ConfiguracionGeneral.Agencia,
                Codmoneda = adelanto.Codmoneda,
            };
            context.Add(comprasAdelantos);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                ErrorSms = $"No se pudo ingresar el adelanto con el codigo {adelanto.Idsalida}, revisar los parametros. Error: {e.Message} {e.Source}";
                return false;
            }
        }
        catch (Exception e)
        {
            ErrorSms = $"No se pudo ingresar el adelanto: {e.Message}";
            return false;
        }
    }

    public async Task<bool> Update(decimal adelanto, string idSalida, string numCompra)
    {
        var caja = VariablesGlobales.Instance.ConfigurationSection["CAJA"];
        var agencia = VariablesGlobales.Instance.ConfigurationSection["AGENCIA"];
        var find = await context.Adelantos.FindAsync(idSalida);
        if (find is null)
        {
            throw new EntityValidationException("No existe la entidad con esos datos");
        }

        find.Saldo = adelanto;
        find.Numcompra = string.IsNullOrEmpty(find.Numcompra) ? $@"{agencia}.{numCompra}" : $@"{find.Numcompra};{agencia}.{numCompra}";

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = $"No existe en el cotexto actual la entidad. Error: {e.Message} {e.Source}";
            return false;
        }
    }

    public async Task<bool> ActualizarCodigoAdelanto()
    {
        var findIds = await context.Id.FirstOrDefaultAsync();
        if (findIds is not null)
        {
            findIds.Idadelanto += 1;
        }

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = $"No fue posible actualizar el codigo del adelanto. Error: {e.Message} {e.Source}";
            return false;
        }
    }

    public async Task<Adelanto?> FindByCodigoCliente(string codigoCliente)
    {
        var find = await context.Adelantos.FirstOrDefaultAsync(adelanto => adelanto.Codcliente == codigoCliente);
        if (find is not null) return find;
        ErrorSms = $"No existe el adelanto con el codigo del cliente especificado {codigoCliente}";
        return null;
    }

    public async Task<Adelanto?> FindByCodigoAdelanto(string codigoAdelanto)
    {
        return await context.Adelantos.SingleOrDefaultAsync(adelanto => adelanto.Idsalida == codigoAdelanto && adelanto.Estado);
    }

    public async Task<List<Adelanto>> FindAll()
    {
        return await context.Adelantos.ToListAsync();
    }

    public async Task<string?> RecpuerarCodigoAdelanto()
    {
        var ids = await parametersRepository.RecuperarParametros();
        return ids is null ? null : ids.Idadelanto.ToString()?.PadLeft(10, '0');
    }

    public async Task<List<Adelanto>> ListarAdelantosPorClientes(string codigo)
    {
        return await context.Adelantos.Where(adelanto => adelanto.Codcliente == codigo).ToListAsync();
    }

    public async Task<bool> AnularAdelanto(string codigo, bool debitar)
    {
        var param = await parametersRepository.RecuperarParametros();
        var caja = VariablesGlobales.Instance.ConfigurationSection["CAJA"];
        var agencia = VariablesGlobales.Instance.ConfigurationSection["AGENCIA"];
        if (param is null)
        {
            throw new Exception(VariablesGlobales.Instance.ConfigurationSection["ERROR_PARAM"]);
        }

        var find = await FindByCodigoAdelanto(codigo);
        if (find is null)
        {
            ErrorSms =
                $"No se pudo encontrar el adelanto con el codigo especificado {codigo}";
            return false;
        }

        if (find.Saldo.CompareTo(find.Monto!.Value) == 0)
        {
            ErrorSms =
                $"No se puede anular el adelanto con el codigo especificado {codigo} ya que se ha aplicado en compras";
            return false;
        }

        find.Estado = false;
        if (!debitar) return await context.SaveChangesAsync() > 0;
        var findMcaja = await maestroCajaRepository.FindByCajaAndAgencia(caja, agencia);
        if (findMcaja is null) return await context.SaveChangesAsync() > 0;
        findMcaja.Entrada = find.Monto.Value;
        findMcaja.Salida = decimal.Zero;
        var nuevoDetaCaja = new Detacaja
        {
            Codcaja = find.Codcaja,
            Idcaja = findMcaja.Idcaja,
            Efectivo = find.Efectivo,
            Cheque = find.Cheque,
            Transferencia = find.Transferencia,
            Idmov = param.AnularAdelanto!.Value,
            Referencia = $"Revertir adelanto con codigo N°: {codigo}",
            Hora = DateTime.Now.ToLongTimeString(),
            Concepto = $"***REVERTIR ADELANTO: {codigo}***",
            Fecha = DateTime.Now
        };
        var movcaja = await context.Movcajas.FindAsync(nuevoDetaCaja.Idmov);
        var insertValueDetaCaja = await maestroCajaRepository.GuardarDatosDetaCaja(nuevoDetaCaja, movcaja!, findMcaja);
        if (insertValueDetaCaja>0) return await context.SaveChangesAsync() > 0;

        ErrorSms = maestroCajaRepository.ErrorSms;
        return false;
    }

    public async Task<bool> AplicarAdelantoEfectivo(List<Adelanto> listaAdelantos, decimal monto, string codCliente = "")
    {
        //var dSaldo = decimal.Zero;
        var param = await parametersRepository.RecuperarParametros();
        if (param is null)
        {
            throw new Exception(VariablesGlobales.Instance.ConfigurationSection["ERROR_PARAM"]);
        }

        var mcaja = await maestroCajaRepository.FindByCajaAndAgencia(ConfiguracionGeneral.Caja, ConfiguracionGeneral.Agencia);
        if (mcaja is null)
        {
            ErrorSms = maestroCajaRepository.ErrorSms;
            return false;
        }

        var tipoCambio = await context.TipoCambios.SingleOrDefaultAsync(cambio => cambio.Fecha == DateTime.Now) ??
                         new TipoCambio
                         {
                             Tipocambio = decimal.Zero
                         };
        mcaja.Entrada = decimal.Add(mcaja.Entrada!.Value, monto);
        mcaja.Sfinal = decimal.Add(mcaja.Sfinal!.Value, monto);
        var detaCaja = new Detacaja
        {
            Idcaja = mcaja.Idcaja,
            Idmov = param.PagoAdelanto!.Value,
            Codcaja = ConfiguracionGeneral.Caja,
            Concepto = "***EFECTIVO A ADELANTO(S): ",
            Transferencia = decimal.Zero,
            Efectivo = monto,
            Cheque = decimal.Zero,
            Fecha = DateTime.Now,
            Hora = DateTime.Now.ToLongTimeString(),
            Referencia = "Pago en efectivo a adelanto",
            Tipocambio = tipoCambio.Tipocambio
        };
        foreach (var adelanto in listaAdelantos.Where(_ => decimal.Compare(monto, decimal.Zero) > 0))
        {
            detaCaja.Concepto += $"{adelanto.Idsalida}; ";
            var saldo = adelanto.Saldo;
            var comprasAdelantos = new ComprasAdelanto
            {
                Codcaja = detaCaja.Codcaja!,
                Codcliente = adelanto.Codcliente,
                Fecha = DateTime.Now,
                Hora = DateTime.Now.TimeOfDay,
                Idadelanto = adelanto.Idsalida,
                Numcompra = adelanto.Idsalida,
                Usuario = VariablesGlobales.Instance.Usuario!.Username,
                Codmoneda = adelanto.Codmoneda,
                Codagencia = ConfiguracionGeneral.Agencia,
                Sinicial = saldo
            };
            if (decimal.Compare(saldo, monto) >= 0)
            {
                comprasAdelantos.Monto = monto;
                saldo = decimal.Subtract(saldo, monto);
                monto = decimal.Zero;
            }
            else
            {
                comprasAdelantos.Monto = saldo;
                monto = decimal.Subtract(monto, saldo); //monto - saldo;
                saldo = decimal.Zero;
            }

            comprasAdelantos.Sfinal = saldo;
        }

        context.Add(detaCaja);
        return false;
    }

    public void Imprimir(string codigo, string nombre)
    {
    }

    public async Task<List<Adelanto>> ListarAdelantosPorFecha(DateTime desde, DateTime hasta, string codCliente)
    {
        try
        {
            return await context.Adelantos
                .Where(adelanto => adelanto.Fecha >= desde
                                   && adelanto.Fecha <= hasta
                                   && adelanto.Codcliente == codCliente)
                .ToListAsync();
        }
        catch (Exception e)
        {
            ErrorSms =
                $"No se recuperaron datos según las fechas indicadas {desde.ToShortDateString()} y {hasta.ToShortDateString()} con el codigo del cliente {codCliente}";
            Debug.WriteLine(e.Message);
            return new List<Adelanto>();
        }
    }

    public async Task<List<ComprasAdelanto>> ListarAdelantosCompras(string idAdelanto)
    {
        try
        {
            return await context.ComprasAdelantos
                .Where(adelanto => adelanto.Idadelanto == idAdelanto)
                .OrderByDescending(adelanto => adelanto.Idadelanto)
                .ThenBy(adelanto => adelanto.Fecha)
                .ToListAsync();
        }
        catch (Exception e)
        {
            ErrorSms =
                $"No se encontraron datos para el codigo de adelanto especificado {idAdelanto}, Error: {e.Message}";
            return [];
        }
    }

    public async Task<List<ComprasAdelanto>> ListarAdelantosComrpasCliente(string codCliente)
    {
        try
        {
            return await context.ComprasAdelantos
                .Where(adelanto => adelanto.Codcliente == codCliente)
                .OrderByDescending(adelanto => adelanto.Idadelanto)
                .ThenBy(adelanto => adelanto.Fecha)
                .ToListAsync();
        }
        catch (Exception e)
        {
            ErrorSms =
                $"No se encontraron datos para el codigo de cliente especificado {codCliente}, Error: {e.Message}";
            return [];
        }
    }

    public async Task<List<ComprasAdelanto>> FindByNumcompraComprasAdelantos(string numeroCompra)
    {
        return await context.ComprasAdelantos.Where(adelanto => adelanto.Numcompra == numeroCompra).ToListAsync();
    }
}