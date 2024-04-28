using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IUsuarioRepository
{
    string ErrorSms { get; }
    Task<bool> AddAsync(Usuario? entity);
    Task<bool> UpdateAsync(Usuario? entity);
    Task<bool> DeleteAsync(int id);
    Task<List<Usuario>> FindAll();
    Task<Usuario?> FindByCodigo(string codigo);
    Task<Usuario?> FindByUsuario(string nomusuario);
}