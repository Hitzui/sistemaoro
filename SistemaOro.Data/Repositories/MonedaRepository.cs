using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class MonedaRepository(DataContext context) : FacadeEntity<Moneda>(context), IMonedaRepository
{
}