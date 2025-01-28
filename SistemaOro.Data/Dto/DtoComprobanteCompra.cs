using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Dto;

public record DtoComprobanteCompra(
    string NombreCliente,
    string Identificacion,
    string Direccion,
    string CodigoCliente,
    string NumeroCompra,
    string Agencia,
    string Caja,
    string Usuario,
    DateTime Fecha,
    string Hora,
    string NombreAgencia,
    string Logo,
    int NumeroContrato,
    List<DetCompra> DetalleCompra,
    decimal Peso,
    decimal Total
);