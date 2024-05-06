using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IAgenciaRepository : ICrudRepository<Agencia>
{
    Task<string> CodigoAgencia();
    Task<List<Agencia>> FindByName(string nomagencia);
 
}