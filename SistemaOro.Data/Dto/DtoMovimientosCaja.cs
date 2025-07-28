namespace SistemaOro.Data.Dto;

public record DtoMovimientosCaja(
    string Caja,
    string Descripcion,
    string Hora,
    DateTime Fecha,
    string Concepto,
    string Referencia,
    decimal Efectivo,
    decimal Cheque,
    decimal Transferencia,
    decimal EfectivoExt,
    decimal ChequeExt ,
    decimal TransferenciaExt ,
    int IdDetacaja,
    string Moneda);