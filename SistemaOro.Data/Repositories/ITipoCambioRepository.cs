using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ITipoCambioRepository : ICrudRepository<TipoCambio>
{
    TipoCambio? FindByDateNow();

    Task<List<TipoCambio>> FindAllByMonth(DateTime date);

    Task Delete(DateTime date);
}