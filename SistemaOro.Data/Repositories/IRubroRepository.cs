using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IRubroRepository : ICrudRepository<Rubro>
{
    Task<Rubro?> FindById(int codrubro);
}

public class RubroRepository(DataContext context) : FacadeEntity<Rubro>(context),IRubroRepository
{
    private readonly DataContext _context = context;

    public Task<Rubro?> FindById(int codrubro)
    {
        return _context.Rubros.FirstOrDefaultAsync(rubro => rubro.Codrubro == codrubro);
    }
}