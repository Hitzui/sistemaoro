using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Repositories;

public class PreciosKilatesRepository(DataContext context) : FacadeEntity<PrecioKilate>(context), IPreciosKilatesRepository
{
    private readonly DataContext _context = context;

    public async Task<bool> UpdateList(List<PrecioKilate> precioKilates)
    {
        try
        {
            await _context.PrecioKilates.ExecuteDeleteAsync();
            await _context.PrecioKilates.AddRangeAsync(precioKilates);
            await _context.SaveChangesAsync();
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
            await _context.PrecioKilates.ExecuteUpdateAsync(
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

    public  Task<PrecioKilate?> FindByDescripcion(string descripcion)
    {
        return  _context.PrecioKilates.AsNoTracking().SingleOrDefaultAsync(kilate => kilate.Descripcion == descripcion);
    }

    public Task<PrecioKilate?> FindByPeso(decimal peso)
    {
        return _context.PrecioKilates.AsNoTracking().SingleOrDefaultAsync(kilate => kilate.Peso == peso);
    }

    public async Task<List<Precio>?> FindByClientes(string codcliente)
    {
        return await _context.Precios.Where(precio => precio.Codcliente == codcliente).ToListAsync();
    }

    public async Task<CierrePrecio?> FindByCierrePrecio(int codCierre)
    {
        return await _context.CierrePrecios.SingleOrDefaultAsync(precio => precio.Codcierre == codCierre);
    }

    public async Task<List<CierrePrecio>> FindByClienteList(string codCliente)
    {
        return await _context.CierrePrecios
            .Where(precio => precio.Codcliente == codCliente
                             && precio.SaldoOnzas > decimal.Zero && precio.Status)
            .ToListAsync();
    }

    public async Task<bool> ActualizarPreciosGuardados(TipoCambio tipoCambio)
    {
        await using var dataContext = new DataContext();
        var kilata24 = 24m;
        var listaPrecios = await dataContext.PrecioKilates.ToListAsync();
        if (listaPrecios.Count < 0)
        {
            return false;
        }

        var precioOro = tipoCambio.PrecioOro ?? decimal.Zero;
        var merma = precioOro * 0.98m;
        var precio24Kilates = merma / 31.1035m;
        var merma24Kilates = precio24Kilates * 0.982m;
        foreach (var precioKilate in listaPrecios)
        {
            var porcentajeKilataje = precioKilate.Peso / kilata24;
            var precioKilataje = merma24Kilates * porcentajeKilataje;
            precioKilate.Precio = Math.Round(precioKilataje, 4);
        }

        var result = await dataContext.SaveChangesAsync();
        return result > 0;
    }
}