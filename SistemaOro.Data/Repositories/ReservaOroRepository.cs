using System.IO.Enumeration;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class ReservaOroRepository(DataContext context) : FacadeEntity<ReservaOro>(context), IReservaOroRepository
{
    public Task<bool> Save(ReservaOro reserva, DetaReserva detaReserva)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Edit(ReservaOro reserva, DetaReserva detaReserva)
    {
        try
        {
            var findReserva = await GetByIdAsync(reserva.IdReserva);
            if (findReserva is null)
            {
                return false;
            }

            var findDetaReservas = await FindDetaReserva(reserva.IdReserva);
            var count = findDetaReservas.Count;
            detaReserva.linea = count + 1;
            detaReserva.IdReserva = reserva.IdReserva;
            context.DetaReservas.Add(detaReserva);
            var findSaldoReserva = await FindSaldoReserva(reserva.IdReserva);
            var onzasDif = decimal.Subtract(findSaldoReserva.Diferencias, detaReserva.Entregadas);
            context.Entry(findReserva).CurrentValues.SetValues(reserva);
            var save = await context.SaveChangesAsync();
            return save > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            ErrorSms = ex.Message;
            return false;
        }
    }

    public Task<List<ReservaOro>> FindByCliente(string codcliente)
    {
        throw new NotImplementedException();
    }

    public Task<DetaReserva> FindSaldoReserva(int idreserva)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Cerrar(ReservaOro reserva)
    {
        var findReserva = await context.ReservaOros.FindAsync(reserva.IdReserva);
        if (findReserva is null)
        {
            return false;
        }

        findReserva.Estado = false;
        var saveChangesAsync = await context.SaveChangesAsync();
        return saveChangesAsync > 0;
    }

    public async Task<List<DetaReserva>> FindDetaReserva(int idreserva)
    {
        return await context.DetaReservas.Where(reserva => reserva.IdReserva == idreserva).ToListAsync();
    }

    public async Task<List<ReservaOro>> BuscarReservasFecha(DateOnly desde, DateOnly hasta)
    {
        return await context.ReservaOros
            .Where(oro => oro.FechaReserva <= hasta && oro.FechaReserva >= desde)
            .ToListAsync();
    }

    public async Task<List<DetaReserva>> BuscarDetaReservasFecha(DateOnly desde, DateOnly hasta)
    {
        return await context.DetaReservas
            .Where(reserva => reserva.Fecha <= hasta && reserva.Fecha >= desde)
            .ToListAsync();
    }
}