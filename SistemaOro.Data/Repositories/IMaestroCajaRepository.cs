using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IMaestroCajaRepository
{
    Task<bool> ValidarCajaAbierta(string caja, string codagencia);

    Task<Mcaja?> FindByCajaAndAgencia(string? caja, string? agencia);

    Task<bool> EstadoCaja(string? caja, string? agencia);

    Task<bool> AbrirCaja(string? caja, string? agencia);

    Task<bool> CerrarCaja(string? caja, string? agencia);

    Task<bool> ActualizarDatosMaestroCaja(string codcaja, string codagencia, decimal entrada, decimal salida);

    Task<int> GuardarDatosDetaCaja(Detacaja dcaja, Movcaja movcaja, Mcaja mocaja);

    Task<bool> ValidarMovimiento(int idmov);

    Task<List<Detacaja>> ListarDetaCaja(string caja);

    Task<decimal> ValidarPrestamosPuentes();

    Task<List<Detacaja>?> RecuperarDetaCajaValores(Mcaja mcaja);

    Task<List<MovimientosCajaSelect>> FindAllByIdMaestroCaja(int id);

    Task<List<MovimientosCajaSelect>> FindAllByFechaDesde(DateTime fechaDesde);

    Task<List<MovimientosCajaSelect>> FindAllByFechaDesdeActiva(DateTime fechaDesde);

    Task<List<MovimientosCajaSelect>> FindAllByFechaDesdeAndFechaHasta(DateTime fechaDesde, DateTime fechaHasta);

    string ErrorSms { get; }
}