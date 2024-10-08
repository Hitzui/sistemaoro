using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class MonedaRepository(DataContext context) : FacadeEntity<Moneda>(context), IMonedaRepository
{
    private readonly DataContext _context = context;

    public Task<int> Save(Moneda moneda)
    {
        foreach (var contextMoneda in _context.Monedas)
        {
            if (contextMoneda.Codmoneda == moneda.Codmoneda) continue;
            if (moneda.Default!.Value)
            {
                contextMoneda.Default = false;
            }
        }

        if (moneda.Codmoneda <= 0)
        {
            _context.AddAsync(moneda);
        }
        else
        {
            _context.Monedas.Update(moneda);
        }


        return _context.SaveChangesAsync();
    }

    public Task<Moneda?> FindDefaultMoneda()
    {
        return context.Monedas.AsNoTracking().SingleOrDefaultAsync(moneda => moneda.Default!.Value);
    }
}