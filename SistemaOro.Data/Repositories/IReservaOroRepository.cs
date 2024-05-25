using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IReservaOroRepository : ICrudRepository<ReservaOro>
{
    Task<bool> Save(ReservaOro reserva, DetaReserva detaReserva);
    
    Task<bool>  Edit(ReservaOro reserva, DetaReserva detaReserva);
    
    Task<List<ReservaOro>> FindByCliente(string codcliente);
    
    Task<DetaReserva> FindSaldoReserva(int idreserva);
    
    Task<bool>  Cerrar(ReservaOro reserva);

    Task<List<DetaReserva>> FindDetaReserva(int idreserva);

    Task<List<ReservaOro>> BuscarReservasFecha(DateOnly desde, DateOnly hasta);

    Task<List<DetaReserva>> BuscarDetaReservasFecha(DateOnly desde, DateOnly hasta);
}