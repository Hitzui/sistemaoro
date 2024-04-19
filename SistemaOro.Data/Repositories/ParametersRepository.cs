    using Microsoft.EntityFrameworkCore;
    using SistemaOro.Data.Entities;
    using SistemaOro.Data.Exceptions;

    namespace SistemaOro.Data.Repositories;

public class ParametersRepository : IParametersRepository
{
    public async Task<Id> RecuperarParametros()
    {
        await using var context = new DataContext();
        return await context.Id.FirstAsync();
    }

    public async Task<int> ActualizarParametros(Id param)
    {
        await using var context = new DataContext();
        var find = await context.Id.FirstOrDefaultAsync();
        if (find is null)
        {
            return 0;
        }

        context.Entry(find).CurrentValues.SetValues(param);
        return await context.SaveChangesAsync();
    }

    public async Task<int> CrearParametros(Id parametros)
    {
        await using var context = new DataContext();
        context.Add(parametros);
        return await context.SaveChangesAsync();
    }

    public async Task<bool> HabilitarVariasCompras(bool opcion)
    {
        await using var context = new DataContext();
        try
        {
            var param =await RecuperarParametros();
            param.VariasCompras = opcion;
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new EntityValidationException("No se pudo habilitar la opción para varias compras. "+e.Message);
        }
    }
}