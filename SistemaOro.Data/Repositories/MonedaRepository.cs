using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class MonedaRepository(DataContext context) : FacadeEntity<Moneda>(context), IMonedaRepository
{
    private readonly DataContext _context = context;

    public async Task<int> Save(Moneda moneda)
    {
        await using var ctx = new DataContext();

        if (moneda.Codmoneda <= 0)
        {
            await ctx.AddAsync(moneda);
        }
        else
        {
            ctx.Monedas.Update(moneda);
        }


        var save= await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();
        if (moneda.Default!.Value)
        {
            await ctx.Monedas.Where(m => m.Codmoneda != moneda.Codmoneda).ExecuteUpdateAsync(calls => calls.SetProperty(u => u.Default, false));
        }
        return save;
    }

    public Task<Moneda?> FindDefaultMoneda()
    {
        return _context.Monedas.AsNoTracking().SingleOrDefaultAsync(moneda => moneda.Default!.Value);
    }
}