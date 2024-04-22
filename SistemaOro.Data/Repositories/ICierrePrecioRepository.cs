using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ICierrePrecioRepository
{
    string ErrorSms { get; }
    Task<CierrePrecio?> FindById(int idcierre);
}