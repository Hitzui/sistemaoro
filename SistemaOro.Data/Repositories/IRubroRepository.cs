using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IRubroRepository : ICrudRepository<Rubro>
{
    
}

public class RubroRepository(DataContext context) : FacadeEntity<Rubro>(context),IRubroRepository
{
}