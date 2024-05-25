namespace SistemaOro.Data.Dto;

public record DetalleDescarguePorCompra(
    int Dgnumdes,
    string Numcliente,
    DateTime Dgfecdes,
    string Numcompra,
    DateTime Fecha,
    string Codcliente,
    string Nombres,
    string Apellidos,
    decimal Peso,
    decimal Total,
    int Cantcompra,
    string Kilshowdoc,
    decimal Detapesoa,
    decimal Importe);