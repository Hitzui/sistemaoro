using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ITipoCambioRepository
{

    string ErrorSms { get; }
    Task<TipoCambio?> FindByDateNow();
}