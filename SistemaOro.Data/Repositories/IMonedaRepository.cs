using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IMonedaRepository
{
    
    string ErrorSms { get; }
    Task<Moneda?> FindById(int codmoneda);
}