using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class MonedaRepository : IMonedaRepository
{
    public string ErrorSms { get; private set; } = "";

    public async Task<Moneda?> FindById(int codmoneda)
    {
        await using var context = new DataContext();
        return await context.Monedas.FindAsync(codmoneda);
    }
}