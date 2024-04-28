using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ICajaRepository
{
    Task<Caja?> GetByIdAsync(int id);
    Task<bool> AddAsync(Caja? entity);
    Task<bool> UpdateAsync(Caja? entity);
    Task<bool> DeleteAsync(int id);

    Task<List<Caja>> FindAll();
    Task<Caja?> FindByCod(string codigo);
    string CodigoCaja();
}