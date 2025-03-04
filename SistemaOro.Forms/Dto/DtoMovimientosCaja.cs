using System;

namespace SistemaOro.Forms.Dto;

public record DtoMovimientosCaja(string Descripcion, string Hora, DateTime Fecha, string Concepto, string Referencia, decimal Efectivo, decimal Cheque, decimal Transferencia);