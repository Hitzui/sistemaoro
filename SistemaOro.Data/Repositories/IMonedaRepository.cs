using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IMonedaRepository : ICrudRepository<Moneda>
{
    Task<int> Save(Moneda moneda);

    Task<Moneda?> FindDefaultMoneda();
}