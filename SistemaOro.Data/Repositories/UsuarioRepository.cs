using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using Unity;

namespace SistemaOro.Data.Repositories;

[method: InjectionConstructor]
public class UsuarioRepository(DataContext context) : FacadeEntity<Usuario>(context), IUsuarioRepository
{
    private readonly DataContext _context = context;

    public async Task<Usuario?> FindByCodigo(string codigo)
    {
        var find = await _context.Usuarios.FindAsync(codigo);
        if (find is not null) return find;
        ErrorSms = $"No existe el usuario con el codigo: {codigo}";
        return null;

    }

    public async Task<Usuario?> FindByUsuario(string nomusuario)
    {
        var find = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Usuario1.Contains(nomusuario));
        if (find is not null) return find;
        ErrorSms = $"No existe el usuario con el codigo: {nomusuario}";
        return null;
    }
}