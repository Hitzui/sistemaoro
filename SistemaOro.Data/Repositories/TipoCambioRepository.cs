using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class TipoCambioRepository(DataContext context) : FacadeEntity<TipoCambio>(context),ITipoCambioRepository
{

    public async Task<TipoCambio?> FindByDateNow()
    {
        var find =await GetByIdAsync(DateTime.Now);
        if (find is not null) return find;
        ErrorSms = $"No existe el tipo de cambio para el dia de hoy {DateTime.Now.ToShortDateString()}";
        return null;

    }
}