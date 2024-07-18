using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class TipoCambioRepository(DataContext context) : FacadeEntity<TipoCambio>(context),ITipoCambioRepository
{
    private readonly DataContext _context1 = context;

    public async Task<TipoCambio?> FindByDateNow()
    {
        var find =await GetByIdAsync(DateTime.Now);
        if (find is not null) return find;
        ErrorSms = $"No existe el tipo de cambio para el dia de hoy {DateTime.Now.ToShortDateString()}";
        return null;

    }

    public Task<List<TipoCambio>> FindAllByMonth(DateTime date)
    {
        return _context1.TipoCambios.Where(cambio => 
            cambio.Fecha.Month == date.Month && cambio.Fecha.Year==date.Year)
            .ToListAsync();
    }

    public async Task Delete(DateTime date)
    {
        var find = await GetByIdAsync(date);
        if (find is not null)
        {
            _context1.TipoCambios.Remove(find);
        }
    }
}