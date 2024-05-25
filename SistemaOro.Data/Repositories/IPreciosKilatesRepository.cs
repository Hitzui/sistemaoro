﻿using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IPreciosKilatesRepository : ICrudRepository<PrecioKilate>
{
    Task<bool> UpdateList(List<PrecioKilate> precioKilates);

    Task<bool> ValoresPorDefault();

    Task<PrecioKilate?> FindByDescripcion(string descripcion);

    Task<List<Precio>?> FindByClientes(string codcliente);

    Task<CierrePrecio?> FindByCierrePrecio(int codCierre);

    Task<List<CierrePrecio>> FindByClienteList(string codCliente);
}