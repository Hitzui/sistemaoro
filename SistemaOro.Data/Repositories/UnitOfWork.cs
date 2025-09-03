using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NLog;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using Unity;

namespace SistemaOro.Data.Repositories;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();
    private IDbContextTransaction? _transaction;
    private readonly DataContext _context;
    private bool _disposed;

    public UnitOfWork(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        var unity = VariablesGlobales.Instance.UnityContainer;
        // Inicialización de repositorios
        ParametersRepository = new ParametersRepository(_context);
        MaestroCajaRepository = new MaestroCajaRepository(ParametersRepository, _context);
        CierrePrecioRepository = new CierrePrecioRepository(_context);
        TipoCambioRepository = new TipoCambioRepository(_context);
        AdelantoRepository = new AdelantosRepository(ParametersRepository, MaestroCajaRepository, _context);
        CompraRepository =
            new CompraRepository(AdelantoRepository, ParametersRepository, TipoCambioRepository, _context);
        PreciosKilatesRepository = new PreciosKilatesRepository(_context);
        AgenciaRepository = new AgenciaRepository(_context);
        CajaRepository = new CajaRepository(_context);
        MovimientosRepository = new MovimientosCajaRepository(_context);
        DescarguesRepository = new DescarguesRepository(_context, CompraRepository);
        MonedaRepository = new MonedaRepository(_context);
        ClienteRepository = new ClienteRepository(_context);
        RubroRepository = new RubroRepository(_context);
        TipoDocumentoRepository = new TipoDocumentoRepository(_context);
        TipoPrecioRepository = new TipoPrecioRepository(_context);
        UsuarioRepository = new UsuarioRepository(_context);
    }

    // Propiedades de los repositorios (igual que antes)
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

    public async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }


    public async Task CommitAsync()
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No hay ninguna transacción para confirmar");
        }

        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch (DbUpdateException ex)
        {
            var entries = ex.Entries;
            foreach (var entry in entries)
            {
                logger.Info($"Entidad con error: {entry.Entity.GetType().Name}");
                logger.Error(ex, "Error al hacer commit async");
                foreach (var prop in entry.CurrentValues.Properties)
                {
                    var value = entry.CurrentValues[prop];
                    logger.Info($"{prop.Name}: {value}");
                }
            }

            throw;
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Error al hacer commit async");
            await RollbackAsync();
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction == null) return;

        try
        {
            await _transaction.RollbackAsync();
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async ValueTask DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        _disposed = true;
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync().ConfigureAwait(false);
        }

        await _context.DisposeAsync().ConfigureAwait(false);
    }
}