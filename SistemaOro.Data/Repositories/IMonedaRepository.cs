using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IMonedaRepository
{
    string ErrorSms { get; }
    Task<Moneda?> GetByIdAsync(int id);
    Task<bool> AddAsync(Moneda entity);
    Task<bool> UpdateAsync(Moneda entity);
    Task<bool> DeleteAsync(int id);
    Task<List<Moneda>> FindAll();
}