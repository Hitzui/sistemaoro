using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IRepositoryAdelantos
{
    string? ErrorSms { get; }
    Task<int> Add(Adelanto adelanto);

    Task<int> Update(decimal adelanto, string idSalida, string numCompra);

    Task<int> ActualizarCodigoAdelanto();
    Task<Adelanto?> FindByCodigoCliente(string codigoCliente);

    Task<Adelanto?> FindByCodigoAdelanto(string codigoAdelanto);

    Task<List<Adelanto>> FindAll();

    Task<string?> RecpuerarCodigoAdelanto();

    Task<List<Adelanto>> ListarAdelantosPorClientes(string codigo);
    Task<bool> AnularAdelanto(string codigo);

    /// <summary>
    /// Esta funcion permite aplicar efectivo a un adelanto sin necesidad de pasar por compra
    /// </summary>
    /// <param name="listaAdelantos">Lista de adelantos</param>
    /// <param name="monto">Monto a restar</param>
    /// <param name="codCliente"></param>
    /// <param name="codcliente">Codigo del cliente</param>
    /// <returns>Boolean</returns>
    /// <remarks>Aplica efectivo a un adelanto sin una compra en especifico</remarks>
    Task<bool> AplicarAdelantoEfectivo(List<Adelanto> listaAdelantos, decimal monto, string codCliente = "");
    void Imprimir(string codigo, string nombre);

    /// <summary>
    /// Listar adelantos por fecha y por codigo del cliente
    /// </summary>
    /// <param name="desde">Date</param>
    /// <param name="hasta">Date</param>
    /// <param name="codCliente">String</param>
    /// <returns>Lista de adelantos</returns>
    Task<List<Adelanto>> ListarAdelantosPorFecha(DateTime desde, DateTime hasta, string codCliente);

    /// <summary>
    /// Buscar lista de adelantos por compras, kardex de adelantos
    /// </summary>
    /// <param name="idAdelanto">Id Adelanto</param>
    /// <returns>Lista de comrpas adelantos</returns>
    Task<List<ComprasAdelanto>> ListarAdelantosCompras(string idAdelanto);

    /// <summary>
    /// Recupera la lista de adelantos aplicados por compras segun el cliente indicado
    /// </summary>
    /// <param name="codCliente">Codigo del cliente</param>
    /// <returns>Lista de compras-adelantos</returns>
    Task<List<ComprasAdelanto>> ListarAdelantosComrpasCliente(string codCliente);
}