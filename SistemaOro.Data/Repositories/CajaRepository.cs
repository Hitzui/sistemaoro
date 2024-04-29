using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class CajaRepository(DataContext context) : FacadeEntity<Caja>(context),ICajaRepository
{
    public async Task<Caja?> FindByCod(string codigo)
    {
        return await context.Cajas.SingleOrDefaultAsync(caja => caja.Codcaja == codigo);
    }

    public string CodigoCaja()
    {
        throw new NotImplementedException();
    }
}