using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IRepositoryMaestroCaja
{
    Task<bool> ValidarCajaAbierta(string caja);

    Task<Mcaja> RecuperarSaldoCaja(string caja, string agencia);

    Task<bool> EstadoCaja(string caja, string agencia);

    Task<bool> AbrirCaja(string caja, string agencia);

    Task<bool> CerrarCaja(string caja, string agencia);

    Task<bool> ActualizarDatosMaestroCaja(Mcaja mocaja);

    Task<bool> GuardarDatosDetaCaja(Detacaja dcaja, Mcaja mocaja);

    Task<bool> ValidarMovimiento(int idmov);

    Task<List<Detacaja>> ListarDetaCaja(string caja);

    Task<decimal> ValidarPrestamosPuentes();

    Task<List<Detacaja>?> RecuperarDetaCajaValores(Mcaja mcaja);

    string ErrorSms { get; }
}