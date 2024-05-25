using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class PreciosKilatesRepository(DataContext context) : FacadeEntity<PrecioKilate>(context), IPreciosKilatesRepository
{
    public async Task<bool> UpdateList(List<PrecioKilate> precioKilates)
    {
        try
        {
            await context.PrecioKilates.ExecuteDeleteAsync();
            await context.PrecioKilates.BulkInsertAsync(precioKilates);
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = e.Message;
            return false;
        }
    }

    public async Task<bool> ValoresPorDefault()
    {
        try
        {
            await context.PrecioKilates.ExecuteUpdateAsync(
                calls => calls.SetProperty(kilate => kilate.PrecioKilate1, 1)
            );
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = e.Message;
            return false;
        }
    }

    public async Task<PrecioKilate?> FindByDescripcion(string descripcion)
    {
        return await context.PrecioKilates.FirstOrDefaultAsync(kilate => kilate.DescKilate == descripcion);
    }

    public async Task<List<Precio>?> FindByClientes(string codcliente)
    {
        return await context.Precios.Where(precio => precio.Codcliente == codcliente).ToListAsync();
    }

    public async Task<CierrePrecio?> FindByCierrePrecio(int codCierre)
    {
        return await context.CierrePrecios.SingleOrDefaultAsync(precio => precio.Codcierre == codCierre);
    }

    public async Task<List<CierrePrecio>> FindByClienteList(string codCliente)
    {
        return await context.CierrePrecios
            .Where(precio => precio.Codcliente == codCliente
                             && precio.SaldoOnzas > decimal.Zero && precio.Status)
            .ToListAsync();
    }
}