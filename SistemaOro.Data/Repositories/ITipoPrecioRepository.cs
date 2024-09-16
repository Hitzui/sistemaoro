using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ITipoPrecioRepository : ICrudRepository<TipoPrecio>
{
    
}

public class TipoPrecioRepository(DataContext context) : FacadeEntity<TipoPrecio>(context),ITipoPrecioRepository
{
}