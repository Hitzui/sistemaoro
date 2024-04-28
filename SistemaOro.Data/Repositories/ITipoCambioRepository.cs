using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ITipoCambioRepository
{

    Task<TipoCambio?> GetByIdAsync(int id);
    Task<bool> AddAsync(TipoCambio? entity);
    Task<bool> UpdateAsync(TipoCambio? entity);
    Task<bool> DeleteAsync(int id);

    Task<List<TipoCambio>> FindAll();
    string ErrorSms { get; }
    Task<TipoCambio?> FindByDateNow();
}