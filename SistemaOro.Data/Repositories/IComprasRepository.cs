﻿using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface ICompraRepository
{
    void ImprimirCompra(string numeroCompra);
    Task<bool> Create(Compra compra, List<DetCompra> detaCompra, Mcaja? modCaja = null, Detacaja? dcaja = null, List<Adelanto>? listaAdelantos = null, List<CierrePrecio>? listaPreciosaCerrar = null);
    Task<bool> UpdateDescargue(Compra compra);
    Task<string?> CodigoCompra();
    string? ErrorSms { get; }
    Task<List<DetCompra>> FindDetaCompra(string numcompra);
    Task<Compra?> FindById(string numerocompra);
    Task<bool> UpdateByDetaCompra(Compra compra, List<DetCompra> detaCompra);
    Task<bool> AnularCompra(string numeroCompra);
    Task<List<Compra>> FindByCodigoCliente(string codCliente);
    Task<bool> UpdateValues(Compra compra);
}