namespace SistemaOro.Data.Dto;

public record DtoComprasOnas(
    string CodigoAgencia,
    string Numcompra,
    string Codcliente,
    string Nombres,
    string Apellidos,
    string Kilate,
    decimal Peso,
    decimal PrecioK,
    decimal? Importe,
    decimal Total,
    DateTime? Fecha,
    decimal TipoCambio,
    decimal? PrecioOro,
    decimal Margen,
    decimal PrecioKilate,
    decimal? Onzas);