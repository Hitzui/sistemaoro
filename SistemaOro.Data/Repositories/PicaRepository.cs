using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class PicaRepository(DataContext dataContext) : FacadeEntity<Pica>(dataContext),IPicaRepository
{
}