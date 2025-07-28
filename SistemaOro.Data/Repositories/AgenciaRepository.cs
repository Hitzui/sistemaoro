using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class AgenciaRepository(DataContext context) : FacadeEntity<Agencia>(context),IAgenciaRepository
{
    private readonly DataContext _context = context;

    public async Task<string> CodigoAgencia()
    {
        var find = await _context.Id.SingleOrDefaultAsync();
        if (find is not null)
        {
            return $"A{find.Codagencia.ToString()!.PadLeft(3, '0')}";
        }

        ErrorSms = "No hay parametros para el codigo de agencia";
        throw new Exception("No existe el parametro de agencia");
    }

    public async Task<List<Agencia>> FindByName(string nomagencia)
    {
        return await _context.Agencias.Where(agencia => agencia.Nomagencia.Contains(nomagencia)).ToListAsync();
    }
}