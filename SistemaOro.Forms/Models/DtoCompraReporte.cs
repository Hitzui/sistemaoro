using System;

namespace SistemaOro.Forms.Models;

public record DtoCompraReporte(
    string Numcompra,
    string Codcliente,
    string Nombres,
    string Apellidos,
    string Identificacion,
    string TipoIdentificacion,
    string Email,
    string Direccion,
    string Celular,
    DateTime FechaCompra,
    decimal TotalCompra,
    string MonedaDisplay);