using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IAgenciaRepository
{
    Task<string> CodigoAgencia();
    Task<bool> Create(Agencia agencia);
    Task<bool> Update(Agencia agencia);
    Task<bool> Delete(Agencia agencia);
    Task<List<Agencia>> FindAll();
    Task<Agencia?> FindById(string codagencia);
    Task<List<Agencia>> FindByName(string nomagencia);
    string? ErrorSms { get; }
}