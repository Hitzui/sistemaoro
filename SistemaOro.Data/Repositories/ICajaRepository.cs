using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ICajaRepository:ICrudRepository<Caja>
{
    Task<Caja?> FindByCod(string codigo);
    string CodigoCaja();
}