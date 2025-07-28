using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ICompraRepository
{
    void ImprimirCompra(string numeroCompra);

    Task<List<DetalleCompra>> DetalleCompraImprimir(string numcompra);

    Task<bool> Create(Compra compra, List<DetCompra> detaCompra, FormaPago? formaPago, List<Adelanto>? listaAdelantos = null, List<CierrePrecio>? listaPreciosaCerrar = null);
    
    Task<bool> UpdateDescargue(Compra compra);

    Task<string?> CodigoCompra();

    string? ErrorSms { get; }

    Task<List<DetCompra>> FindDetaCompra(string numcompra);

    Task<Compra?> FindById(string numerocompra);

    Task<bool> UpdateByDetaCompra(Compra compra, List<DetCompra> detaCompra, FormaPago? formaPago);

    Task<bool> AnularCompra(string numeroCompra);

    Task<List<Compra>> FindByCodigoCliente(string codCliente);

    Task<bool> UpdateValues(Compra compra);

    /// <summary>
    /// Busca las compras vigentes y retorna una lista con los nombres y codigos de clientes
    /// </summary>
    /// <returns>Lista de compras por clientes</returns>
    Task<IList<DtoComprasClientes>> FindComprasClientes();

    Task<int> CountCompras();

    IQueryable<DtoComprasClientes> FindComprasClientesPaged();

    Task<List<DtoComprasClientes>> FindComprasClientesFechaAndCerrada(DateTime fecha);

    FormaPago? FindFormaPago(string numcompra);

    Task<List<ViewComprasOnza>> ListadoComprasOnzas(DateTime fechaInicial, DateTime fechaFinal);

    Task<List<DetalleCompra>> DetalleCompraImprimirPorCliente(string codcliente);

    Task<List<DetalleCompra>> DetalleCompraImprimirPorClientePorFecha(string codcliente, DateTime fechaInicial, DateTime fechaFinal);
}