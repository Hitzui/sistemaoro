using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IDescarguesRepository : ICrudRepository<Descargue>
{
    Task<bool> GuardarDescargueByCompra(List<DtoComprasClientes> compras, DateTime fecha);

    Task<bool> GuardarDescargueDetalleCompra(string numcompra);

    Task<int> NumeroDescargue();

    Task<List<Compra>> GenerarLote(DateTime fecha);

    Task<bool> GuardarLotoeDetaCompra(List<Dictionary<string, int>> detalle, Descargue descargue);

    Task<List<Descargue>> FindByDesdeAndHasta(DateTime desde, DateTime hasta);

    Task<List<Compra>> GetComprasByNumdes(int numdes);

    Task<List<Compra>> GetComprasByFecha(DateTime desde, DateTime hasta);

    Task<List<GeneralDescargueCompra>> FindGeneralDescargueCompra(int dgnumdes);
    
    Task<List<DetalleDescarguePorCompra>> FindDetalleDescargueCompra(int dgnumdes);

    Task<List<SaldoCompra>> SaldoCompraDescargueAnio(DateTime fecha);

}