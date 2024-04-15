using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class RepositoryAdelantos(
    IRepositoryParameters repositoryParameters,
    IRepositoryMaestroCaja repositoryMaestroCaja) : IRepositoryAdelantos
{
    private readonly IRepositoryParameters _repositoryParameters = repositoryParameters;
    private readonly IRepositoryMaestroCaja _repositoryMaestroCaja = repositoryMaestroCaja;

    public string? ErrorSms { get; private set; }

    public async Task<int> Add(Adelanto adelanto)
    {
        await using var context = new DataContext();
        try
        {
            adelanto = context.Add(adelanto).Entity;
            var comprasAdelantos = new ComprasAdelanto()
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
                Codagencia = VariablesGlobales.Instance.ConfiguracionGeneral.Agencia,
                Codmoneda = adelanto.Codmoneda,
            };
            context.Add(comprasAdelantos);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ErrorSms = $"No se pudo ingresar el adelanto: {e.Message}";
            return 0;
        }
    }

    public async Task<int> Update(decimal adelanto, string idSalida, string numCompra)
    {
        var config = VariablesGlobales.Instance.ConfiguracionGeneral;
        await using var context = new DataContext();
        var find = await context.Adelantos.FindAsync(idSalida);
        if (find is null)
        {
            ErrorSms = "No existe la entidad con esos datos";
            return 0;
        }

        find.Saldo = adelanto;
        find.Numcompra = string.IsNullOrEmpty(find.Numcompra)
            ? $@"{config.Agencia}.{numCompra}"
            : $@"{find.Numcompra};{config.Agencia}.{numCompra}";

        return await context.SaveChangesAsync();
    }

    public async Task<int> ActualizarCodigoAdelanto()
    {
        await using var context = new DataContext();
        var findIds = await context.Id.FirstOrDefaultAsync();
        if (findIds is not null)
        {
            findIds.Idadelanto += 1;
        }

        return await context.SaveChangesAsync();
    }

    public async Task<Adelanto?> FindByCodigoCliente(string codigoCliente)
    {
        await using var context = new DataContext();
        var find = await context.Adelantos.FirstOrDefaultAsync(adelanto => adelanto.Codcliente == codigoCliente);
        if (find is not null) return find;
        ErrorSms = $"No existe el adelanto con el codigo del cliente especificado {codigoCliente}";
        return null;
    }

    public async Task<Adelanto?> FindByCodigoAdelanto(string codigoAdelanto)
    {
        await using var context = new DataContext();
        return await context.Adelantos.FindAsync(codigoAdelanto);
    }

    public async Task<List<Adelanto>> FindAll()
    {
        await using var context = new DataContext();
        return await context.Adelantos.ToListAsync();
    }

    public async Task<string?> RecpuerarCodigoAdelanto()
    {
        await using var context = new DataContext();
        var ids = await context.Id.FirstOrDefaultAsync();
        return ids is null ? null : ids.Idadelanto.ToString()?.PadLeft(10, '0');
    }

    public async Task<List<Adelanto>> ListarAdelantosPorClientes(string codigo)
    {
        await using var context = new DataContext();
        return await context.Adelantos.Where(adelanto => adelanto.Codcliente == codigo).ToListAsync();
    }

    public async Task<bool> AnularAdelanto(string codigo)
    {
        await using var context = new DataContext();
        var param = await _repositoryParameters.RecuperarParametros();
        var find = await context.Adelantos.FirstOrDefaultAsync(adelanto =>
            adelanto.Idsalida == codigo && adelanto.Monto == adelanto.Saldo);
        if (find is null)
        {
            ErrorSms =
                $"No se pudo encontrar el adelanto con el codigo especificado {codigo} o ya se ha aplicado en distintas compras";
            return false;
        }

        find.Saldo = decimal.Zero;
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AplicarAdelantoEfectivo(List<Adelanto> listaAdelantos, decimal monto,
        string codCliente = "")
    {
        await using var context = new DataContext();
        var dSaldo = decimal.Zero;
        var param = await _repositoryParameters.RecuperarParametros();
        var config = VariablesGlobales.Instance.ConfiguracionGeneral;
        var mcaja = await _repositoryMaestroCaja.RecuperarSaldoCaja(config.Caja, config.Agencia);
        if (mcaja is null)
        {
            ErrorSms = _repositoryMaestroCaja.ErrorSms;
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
            Codcaja = config.Caja,
            Concepto = "***EFECTIVO A ADELANTO(S): ",
            Transferencia = decimal.Zero,
            Efectivo = monto,
            Cheque = decimal.Zero,
            Fecha = DateTime.Now,
            Hora = DateTime.Now.ToLongTimeString(),
            Referencia = "Pago en efectivo a adelanto",
            Tipocambio = tipoCambio.Tipocambio
        };
        foreach (var adelanto in listaAdelantos)
        {
            if (decimal.Compare(monto, decimal.Zero) <= 0) continue;
            detaCaja.Concepto += $"{adelanto.Idsalida}; ";
            var saldo = adelanto.Saldo;
            var comprasAdelantos = new ComprasAdelanto
            {
                Codcaja = detaCaja.Codcaja,
                Codcliente = adelanto.Codcliente,
                Fecha = DateTime.Now,
                Hora = DateTime.Now.TimeOfDay,
                Idadelanto = adelanto.Idsalida,
                Numcompra = adelanto.Idsalida,
                Usuario = VariablesGlobales.Instance.Usuario!.Usuario1,
                Codmoneda = adelanto.Codmoneda,
                Codagencia = config.Agencia,
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
        await using var context = new DataContext();
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
            return [];
        }
    }

    public async Task<List<ComprasAdelanto>> ListarAdelantosCompras(string idAdelanto)
    {
        await using var context = new DataContext();
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
        await using var context = new DataContext();
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
}