using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class CierrePrecioRepository(DataContext context) : FacadeEntity<CierrePrecio>(context),ICierrePrecioRepository
{
}