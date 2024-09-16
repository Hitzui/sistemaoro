namespace SistemaOro.Data.Dto;

public class DtoComprasClientes
{
    public string? Numcompra { get; set; }
    public string? Codcliente { get; set; }
    public DateTime? Fecha { get; set; }
    public decimal? Total { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public int? Nocontrato { get; set; }
}