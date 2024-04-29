using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class CierrePrecioRepository(DataContext context) : ICierrePrecioRepository
{
    public string ErrorSms { get; private set; } = "";

    public async Task<CierrePrecio?> FindById(int idcierre)
    {
        var find = await context.CierrePrecios.FindAsync(idcierre);
        if (find is not null)
        {
            return find;
        }

        ErrorSms = $"No existe el cierre de precio con el número {idcierre}";
        return null;
    }
}