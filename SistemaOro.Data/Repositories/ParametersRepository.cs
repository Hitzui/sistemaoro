    using Microsoft.EntityFrameworkCore;
    using SistemaOro.Data.Entities;
    using SistemaOro.Data.Exceptions;
    using SistemaOro.Data.Libraries;

    namespace SistemaOro.Data.Repositories;

public class ParametersRepository(DataContext context) : IParametersRepository
{
    public async Task<Id?> RecuperarParametros()
    {
        return await context.Id.FirstOrDefaultAsync();
    }

    public async Task<int> ActualizarParametros(Id param)
    {
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
        context.Add(parametros);
        return await context.SaveChangesAsync();
    }

    public async Task<bool> HabilitarVariasCompras(bool opcion)
    {
        try
        {
            var param =await RecuperarParametros();
            if (param is null)
            {
                throw new Exception(VariablesGlobales.Instance.ConfigurationSection["ERROR_PARAM"]);
            }
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