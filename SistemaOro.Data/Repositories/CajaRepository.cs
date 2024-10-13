using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class CajaRepository(DataContext context) : FacadeEntity<Caja>(context),ICajaRepository
{
    private readonly DataContext _context = context;

    public async Task<Caja?> FindByCod(string codigo)
    {
        return await _context.Cajas.SingleOrDefaultAsync(caja => caja.Codcaja == codigo);
    }

    public string CodigoCaja()
    {
        throw new NotImplementedException();
    }
}