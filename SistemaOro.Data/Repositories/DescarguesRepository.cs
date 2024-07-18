using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public class DescarguesRepository(DataContext context, ICompraRepository compraRepository) : FacadeEntity<Descargue>(context), IDescarguesRepository
{
    public async Task<bool> GuardarDescargueDetalleCompra(string numcompra)
    {
        var findCompra = await compraRepository.FindById(numcompra);
        if (findCompra is null)
        {
            ErrorSms = $"No existe la compra con el código {numcompra}";
            return false;
        }

        var findDetaCompra = await compraRepository.FindDetaCompra(numcompra);
        foreach (var detCompra in findDetaCompra)
        {
            detCompra.Numdescargue = await NumeroDescargue();
        }

        context.UpdateRange(findDetaCompra);
        try
        {
            await context.BulkSaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = $"No se pudo guardar el descargue del detalle de compra con el código {numcompra}. Error: {e.Message}";
            return false;
        }
    }

    public async Task<int> NumeroDescargue()
    {
        var findNumero = await context.Descargues.OrderByDescending(descargue => descargue.Dgnumdes).FirstOrDefaultAsync();
        return findNumero is null ? 1 : findNumero.Dgnumdes + 1;
    }

    public async Task<List<Compra>> GenerarLote(DateTime fecha)
    {
        return await context.Compras.OrderBy(compra => compra.Fecha)
            .Where(compra => compra.Fecha.Date <= fecha.Date && compra.Codestado == EstadoCompra.Cerrada)
            .ToListAsync();
    }

    public async Task<bool> GuardarLotoeDetaCompra(List<Dictionary<string, int>> detalle, Descargue descargue)
    {
        var numeroDescargue = await NumeroDescargue();
        var listCompra = new List<Compra>();
        var listDetaCompra = new List<DetCompra>();
        foreach (var values in detalle)
        {
            var numcompra = values.Keys.First();
            var findCompra = await compraRepository.FindById(numcompra);
            if (findCompra is null)
            {
                ErrorSms = compraRepository.ErrorSms!;
                return false;
            }

            if (findCompra.Codestado == EstadoCompra.Anulada)
            {
                ErrorSms = $"La compra actual se encuntra anulada, {numcompra}";
                return false;
            }

            var findDetaCompra = await compraRepository.FindDetaCompra(numcompra);
            foreach (var detCompra in findDetaCompra.Where(compra => values.ContainsValue(compra.Linea)))
            {
                detCompra.Numdescargue = numeroDescargue;
                listDetaCompra.Add(detCompra);
            }

            var lista = findDetaCompra.Count;
            var count = findDetaCompra.Count(compra => compra.Numdescargue > 0);
            var estado = EstadoCompra.Vigente;
            if (lista > count)
            {
                estado = EstadoCompra.Cerrada;
            }
            else if (lista == count)
            {
                estado = EstadoCompra.Descargada;
            }

            findCompra.Codestado = estado;
            listCompra.Add(findCompra);
        }

        try
        {
            await context.SingleInsertAsync(descargue);
            await context.BulkUpdateAsync(listCompra);
            await context.BulkUpdateAsync(listDetaCompra);
            return true;
        }
        catch (Exception e)
        {
            ErrorSms = $"No se guardaron los datos por el siguiente error: {e.Message}";
            return false;
        }
    }

    public async Task<List<Descargue>> FindByDesdeAndHasta(DateTime desde, DateTime hasta)
    {
        return await context.Descargues.Where(descargue => descargue.Dgfecdes >= desde && descargue.Dgfecdes <= hasta).ToListAsync();
    }

    public async Task<List<Compra>> GetComprasByNumdes(int numdes)
    {
        return await context.Compras.Where(compra => compra.Dgnumdes == numdes).ToListAsync();
    }

    public async Task<List<Compra>> GetComprasByFecha(DateTime desde, DateTime hasta)
    {
        return await context.Compras
            .Join(context.Descargues,
                compra => compra.Dgnumdes,
                descargue => descargue.Dgnumdes,
                (compra, descargue) => new { compra, descargue })
            .Where(arg => arg.compra.Dgnumdes > 0 && arg.descargue.Dgfecdes >= desde && arg.descargue.Dgfecdes <= hasta)
            .Select(arg => arg.compra)
            .ToListAsync();
    }

    public async Task<List<GeneralDescargueCompra>> FindGeneralDescargueCompra(int dgnumdes)
    {
        var result = from descargue in context.Descargues
            join compra in context.Compras on descargue.Dgnumdes equals compra.Dgnumdes
            join cliente in context.Clientes on compra.Codcliente equals cliente.Codcliente
            where descargue.Dgnumdes == dgnumdes
            select new GeneralDescargueCompra
            (
                descargue.Dgnumdes,
                cliente.Codcliente,
                descargue.Dgfecdes,
                compra.Numcompra,
                compra.Fecha,
                cliente.Codcliente,
                cliente.Nombres,
                cliente.Apellidos,
                compra.Peso,
                compra.Total,
                descargue.Dgcancom
            );
        return await result.ToListAsync();
    }

    public async Task<List<DetalleDescarguePorCompra>> FindDetalleDescargueCompra(int dgnumdes)
    {
        var result = from descargue in context.Descargues
            join compra in context.Compras on descargue.Dgnumdes equals compra.Dgnumdes
            join detcompra in context.DetCompras on compra.Numcompra equals detcompra.Numcompra
            join cliente in context.Clientes on compra.Codcliente equals cliente.Codcliente
            where descargue.Dgnumdes == dgnumdes
            orderby compra.Numcompra
            select new DetalleDescarguePorCompra(descargue.Dgnumdes, compra.Codcliente, 
                descargue.Dgfecdes, compra.Numcompra, compra.Fecha, cliente.Codcliente, cliente.Nombres,
                cliente.Apellidos, compra.Peso, compra.Total, descargue.Dgcancom,
                detcompra.Kilshowdoc, detcompra.Peso, detcompra.Importe.Value);
        return await result.ToListAsync();
    }

    public async Task<List<SaldoCompra>> SaldoCompraDescargueAnio(DateTime fecha)
    {
        var result = from c in context.Compras
            join d in context.Descargues on c.Dgnumdes equals d.Dgnumdes into cd
            from d in cd.DefaultIfEmpty()
            where d.Dgfecdes.Year == fecha.Year
            group new { c, d } by d.Dgfecgen.Month
            into g
            select new SaldoCompra
            (
                 g.Key,
                g.Sum(x => x.c.Peso),
                 g.Sum(x => x.c.Total),
                 g.Sum(x => x.d.Dgpesntt),
                 g.Sum(x => x.d.Dgimptcom),
                 g.Sum(x => x.d.Dgpesntt - x.c.Peso),
                 g.Sum(x => x.d.Dgimptcom - x.c.Total)
            );
        return await result.ToListAsync();
    }
}