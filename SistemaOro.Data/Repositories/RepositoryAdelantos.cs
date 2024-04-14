using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Exceptions;
using SistemaOro.Data.Libraries;
using Unity;

namespace SistemaOro.Data.Repositories;

public class RepositoryAdelantos : IRepositoryAdelantos
{
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
                Codagencia = VariablesGlobales.Instance.ConfiguracionGeneral().Agencia,
                Codmoneda = adelanto.Codmoneda,
            };
            context.Add(comprasAdelantos);
            return await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new EntityValidationException(e.Message);
        }
    }

    public async Task<int> Update(decimal adelanto, string idSalida, string numCompra)
    {
        var config = VariablesGlobales.Instance.ConfiguracionGeneral();
        await using var context = new DataContext();
        var find = await context.Adelantos.FindAsync(idSalida);
        if (find is null)
        {
            throw new EntityValidationException("No existe la entidad con esos datos");
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

    public async Task<Adelanto> FindByCodigoCliente(string codigoCliente)
    {
        await using var context = new DataContext();
        var find = await context.Adelantos.SingleOrDefaultAsync(adelanto => adelanto.Codcliente == codigoCliente);
        if (find is null)
        {
            throw new EntityValidationException("No existe el adelanto con el codigo del cliente especificado");
        }

        return find;
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

    public async Task<bool> anularAdelanto(string codigo)
    {
        await using var context = new DataContext();
        var repositoryParameters = VariablesGlobales.Instance.UnityContainer.Resolve<IRepositoryParameters>();
        throw new Exception();
    }

    public bool AplicarAdelantoEfectivo(List<Adelanto> listaAdelantos, decimal monto, string codCliente = "")
    {
        throw new NotImplementedException();
    }

    public void Imprimir(string codigo, string nombre)
    {
        throw new NotImplementedException();
    }

    public List<Adelanto> ListarAdelantosPorFecha(DateTime desde, DateTime hasta, string codCliente)
    {
        throw new NotImplementedException();
    }

    public List<ComprasAdelanto> ListarAdelantosComrpas(string idAdelanto)
    {
        throw new NotImplementedException();
    }

    public List<ComprasAdelanto> ListarAdelantosComrpasCliente(string codCliente)
    {
        throw new NotImplementedException();
    }
}