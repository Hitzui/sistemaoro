using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using System.Linq;

namespace SistemaOro.Data.Repositories;

public class MovimientosCajaRepository(DataContext dataContext) : FacadeEntity<Movcaja>(dataContext), IMovimientosRepository
{
    private readonly DataContext _dataContext = dataContext;

    public Task<List<MovCajasDto>> GetMovcajasAndRubro()
    {
        return _dataContext.Movcajas.Join(_dataContext.Rubros,
                movcaja => movcaja.Codrubro,
                rubro => rubro.Codrubro,
                (movcaja, rubro) => new { movcaja, rubro })
            .Select(arg => new MovCajasDto(arg.movcaja.Idmov, arg.movcaja.Descripcion, arg.rubro.Codrubro, arg.rubro.Descrubro, arg.rubro.Naturaleza == 0 ? "Egreso" : "Ingreso"))
            .ToListAsync();
    }
}