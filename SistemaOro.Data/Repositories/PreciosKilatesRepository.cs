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
            await context.PrecioKilates.AddRangeAsync(precioKilates);
            await context.SaveChangesAsync();
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
                calls => calls.SetProperty(kilate => kilate.Precio, 1)
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
        return await context.PrecioKilates.FirstOrDefaultAsync(kilate => kilate.Descripcion == descripcion);
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

    public async Task<bool> ActualizarPreciosGuardados(TipoCambio tipoCambio)
    {
        await using var dataContext = new DataContext();
        var kilata24 = new decimal(24);
        var listaPrecios = await dataContext.PrecioKilates.ToListAsync();
        if (listaPrecios.Count < 0)
        {
            return false;
        }

        var precioOro = tipoCambio.PrecioOro ?? decimal.Zero;
        var merma = precioOro * new decimal(0.98);
        var precio24Kilates = merma / new decimal(31.1035);
        var merma24Kilates = precio24Kilates * new decimal(0.982);
        foreach (var precioKilate in listaPrecios)
        {
            var porcentajeKilataje = precioKilate.Peso / kilata24;
            if (precioKilate.Peso.CompareTo(kilata24) == 0)
            {
                porcentajeKilataje = (decimal)0.982;
            }

            var precioKilataje = merma24Kilates * porcentajeKilataje;
            precioKilate.Precio = precioKilataje;
        }

        var result = await dataContext.SaveChangesAsync();
        return result > 0;
    }
}