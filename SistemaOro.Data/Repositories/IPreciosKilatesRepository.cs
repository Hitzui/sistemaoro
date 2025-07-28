using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IPreciosKilatesRepository : ICrudRepository<PrecioKilate>
{
    Task<bool> UpdateList(List<PrecioKilate> precioKilates);

    Task<bool> ValoresPorDefault();

    Task<PrecioKilate?> FindByDescripcion(string descripcion);

    PrecioKilate? FindByPeso(decimal peso);

    Task<List<Precio>?> FindByClientes(string codcliente);

    Task<CierrePrecio?> FindByCierrePrecio(int codCierre);

    Task<List<CierrePrecio>> FindByClienteList(string codCliente);

    Task<bool> ActualizarPreciosGuardados(TipoCambio tipoCambio);
}