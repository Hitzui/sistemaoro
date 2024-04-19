using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class AgenciaRepository : IAgenciaRepository
{
    public async Task<string> CodigoAgencia()
    {
        await using var context = new DataContext();
        var find = await context.Id.FirstOrDefaultAsync();
        if (find is not null)
        {
            return $"A{find.Codagencia.ToString()!.PadLeft(3, '0')}";
        }

        ErrorSms = "No hay parametros para el codigo de agencia";
        throw new Exception("No existe el parametro de agencia");
    }

    public async Task<bool> Create(Agencia agencia)
    {
        await using var context = new DataContext();
        try
        {
            context.Add(agencia);
            var find = await context.Id.FirstOrDefaultAsync();
            if (find is not null)
            {
                find.Codagencia += 1;
            }

            var result= await context.SaveChangesAsync() > 0;
            if (result)
            {
                return true;
            }

            ErrorSms = "No se pudo ingresar la agencia en la base de datos.";
            return false;
        }
        catch (Exception e)
        {
            ErrorSms = $"No se pudo ingresar la agencia en la base de datos. Error: {e.Message}";
            return false;
        }
    }

    public async Task<bool> Update(Agencia agencia)
    {
        await using var context = new DataContext();
        var find =await FindById(agencia.Codagencia);
        if (find is null)
        {
            ErrorSms = "No existe la agencia a actualizar. Intente nuevamente";
            return false;
        }

        context.Entry(find).CurrentValues.SetValues(agencia);
        var result = await context.SaveChangesAsync()>0;
        if (result)
        {
            return true;
        }
        ErrorSms = "No se actualizaron los datos de la agencia.";
        return false;
    }

    public async Task<bool> Delete(Agencia agencia)
    {
        await using var context = new DataContext();
        var find = await FindById(agencia.Codagencia);
        if (find is null)
        {
            ErrorSms = "No existe la agencia a actualizar. Intente nuevamente";
            return false;
        }
        context.Agencias.Remove(agencia);
        var result = await context.SaveChangesAsync();
        if (result>0)
        {
            return true;
        }
        ErrorSms = "No se pudo eliminar la agencia seleccionada. Intente nuevamente";
        return false;
    }

    public async Task<List<Agencia>> FindAll()
    {
        await using var context = new DataContext();
        return await context.Agencias.ToListAsync();
    }

    public async Task<Agencia?> FindById(string codagencia)
    {
        await using var context = new DataContext();
        return await context.Agencias.SingleOrDefaultAsync(agencia => agencia.Codagencia == codagencia);
    }

    public async Task<List<Agencia>> FindByName(string nomagencia)
    {
        await using var context = new DataContext();
        return await context.Agencias.Where(agencia => agencia.Nomagencia.Contains(nomagencia)).ToListAsync();
    }

    public string? ErrorSms { get; private set; }
}