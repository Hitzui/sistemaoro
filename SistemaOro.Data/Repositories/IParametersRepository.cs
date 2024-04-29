using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IParametersRepository
{
    Task<Id?> RecuperarParametros();
    Task<int> ActualizarParametros(Id param);
    Task<int> CrearParametros(Id parametros);
    /// <summary>
    /// De esta manera los precios de oro no volveran a los defualts
    /// al menos que el usuario indique lo contrario.
    /// Así se podrán realizar compras sin que se espere a que otro
    /// usuario termine la compra.
    /// </summary>
    /// <returns>True, habilitar varias compras</returns>
    /// <remarks></remarks>
    Task<bool> HabilitarVariasCompras(bool opcion);
}