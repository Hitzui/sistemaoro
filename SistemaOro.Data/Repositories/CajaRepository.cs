using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class CajaRepository : ICajaRepository
{
    public string? ErrorSms { get; private set; }


    public async Task<bool> Create(Caja caja)
    {
        await using var context = new DataContext();
        context.Add(caja);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }

        ErrorSms = "No se pudo ingresar la caja al sistema, intente nuevamente";
        return false;
    }

    public async Task<bool> Update(Caja caja)
    {
        await using var context = new DataContext();
        var find = await FindByCod(caja.Codcaja);
        if (find is null)
        {
            ErrorSms = "No existe la caja en el sistema, intente nuevamente";
            return false;
        }
        context.Entry(find).CurrentValues.SetValues(caja);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }
        ErrorSms = "No se guardaron los cambios en el sistema, intente nuevamente";
        return false;
    }

    public async Task<bool> Delete(Caja caja)
    {
        await using var context = new DataContext();
        var find = await FindByCod(caja.Codcaja);
        if (find is null)
        {
            ErrorSms = "No existe la caja en el sistema, intente nuevamente";
            return false;
        }

        context.Remove(caja);
        var result = await context.SaveChangesAsync() > 0;
        if (result)
        {
            return true;
        }
        ErrorSms = "No se guardaron los cambios en el sistema, intente nuevamente";
        return false;
    }

    public async Task<List<Caja>> FindAll()
    {
        await using var context = new DataContext();
        return await context.Cajas.ToListAsync();
    }

    public async Task<Caja?> FindByCod(string codigo)
    {
        await using var context = new DataContext();
        return await context.Cajas.SingleOrDefaultAsync(caja => caja.Codcaja == codigo);
    }

    public string CodigoCaja()
    {
        throw new NotImplementedException();
    }
}