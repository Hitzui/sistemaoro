using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ITipoCambioRepository : ICrudRepository<TipoCambio>
{
    Task<TipoCambio?> FindByDateNow();
}