using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class TipoDocumentoRepository(DataContext context) : FacadeEntity<TipoDocumento>(context),ITipoDocumentoRepository
{
}