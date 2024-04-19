using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ICajaRepository
{
    string? ErrorSms { get; }
    Task<bool> Create(Caja caja);
    Task<bool> Update(Caja caja);
    Task<bool> Delete(Caja caja);
    Task<List<Caja>> FindAll();
    Task<Caja?> FindByCod(string codigo);
    string CodigoCaja();
}