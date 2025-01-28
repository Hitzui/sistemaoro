using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IUsuarioRepository : ICrudRepository<Usuario>
{
    Task<Usuario?> FindByCodigo(string codigo);
    Task<Usuario?> FindByUsuario(string nomusuario);
}