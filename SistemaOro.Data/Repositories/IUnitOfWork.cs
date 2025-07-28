namespace SistemaOro.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IAgenciaRepository AgenciaRepository { get; }
    public ICajaRepository CajaRepository { get; }
    public IMovimientosRepository MovimientosRepository { get; }
    public IDescarguesRepository DescarguesRepository { get; }
    public IMonedaRepository MonedaRepository { get; }
    public IClienteRepository ClienteRepository { get; }
    public IRubroRepository RubroRepository { get; }
    public ITipoDocumentoRepository TipoDocumentoRepository { get; }
    public ITipoPrecioRepository TipoPrecioRepository { get; }
    public IPreciosKilatesRepository PreciosKilatesRepository { get; }
    public ICompraRepository CompraRepository { get; }
    public IAdelantosRepository AdelantoRepository { get; }
    public IParametersRepository ParametersRepository { get; }
    public IMaestroCajaRepository MaestroCajaRepository { get; }
    public ICierrePrecioRepository CierrePrecioRepository { get; }
    public ITipoCambioRepository TipoCambioRepository { get; }
    public IUsuarioRepository UsuarioRepository { get; }
    Task BeginTransactionAsync();
    Task CommitAsync();

    Task RollbackAsync();
}