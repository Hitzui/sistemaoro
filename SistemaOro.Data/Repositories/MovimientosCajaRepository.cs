using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class MovimientosCajaRepository(DataContext dataContext) : FacadeEntity<Movcaja>(dataContext), IMovimientosRepository
{
    
}