using System;

namespace SistemaOro.Forms.Dto;

public record DtoMovimientosCaja(
    string Descripcion,
    string Hora,
    DateTime Fecha,
    string Concepto,
    string Referencia,
    decimal Efectivo,
    decimal Cheque,
    decimal Transferencia,
    decimal EfectivoExt=0m,
    decimal ChequeExt=0m,
    decimal TransferenciaExt=0m);