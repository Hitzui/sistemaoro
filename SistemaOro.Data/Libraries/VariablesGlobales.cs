using System.Reflection;
using Microsoft.Extensions.Configuration;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Repositories;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace SistemaOro.Data.Libraries;

public class VariablesGlobales
{
    private static IConfigurationBuilder? _configurationBuilder;
    private static VariablesGlobales? _instance;
    private static readonly object Bloqueo = new();

    private VariablesGlobales()
    {
        DataContext = new DataContext();
        ConfiguracionGeneral = new ConfiguracionGeneral();
        UnityContainer = new UnityContainer();
        UnityContainer.RegisterType<IAdelantosRepository, AdelantosRepository>();
        UnityContainer.RegisterType<IAgenciaRepository, AgenciaRepository>(new InjectionConstructor(DataContext));
        UnityContainer.RegisterType<ICajaRepository, CajaRepository>(new InjectionConstructor(DataContext));
        UnityContainer.RegisterType<ICierrePrecioRepository, CierrePrecioRepository>();
        UnityContainer.RegisterType<ICompraRepository, CompraRepository>();
        UnityContainer.RegisterType<IClienteRepository, ClienteRepository>();
        UnityContainer.RegisterType<IDescarguesRepository, DescarguesRepository>();
        UnityContainer.RegisterType<IMaestroCajaRepository, MaestroCajaRepository>();
        UnityContainer.RegisterType<IMovimientosRepository, MovimientosCajaRepository>();
        UnityContainer.RegisterType<IMonedaRepository, MonedaRepository>();
        UnityContainer.RegisterType<IParametersRepository, ParametersRepository>();
        UnityContainer.RegisterType<IPicaRepository, PicaRepository>();
        UnityContainer.RegisterType<IPreciosKilatesRepository, PreciosKilatesRepository>();
        UnityContainer.RegisterType<IRubroRepository, RubroRepository>();
        UnityContainer.RegisterType<ITipoCambioRepository, TipoCambioRepository>();
        UnityContainer.RegisterType<ITipoDocumentoRepository, TipoDocumentoRepository>();
        UnityContainer.RegisterType<ITipoPrecioRepository, TipoPrecioRepository>();
        UnityContainer.RegisterType<IUsuarioRepository, UsuarioRepository>();
        UnityContainer.RegisterType<DataContext>(new PerResolveLifetimeManager());
        UnityContainer.RegisterType<IUnitOfWork, UnitOfWork>(new InjectionConstructor(DataContext));

        var exePath = AppContext.BaseDirectory; // Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var appSettingsPath = Path.Combine(exePath, "appsettings.json");
        _configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(exePath) // base = carpeta del exe
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true);
    }

    public static VariablesGlobales Instance
    {
        get
        {
            if (_instance != null) return _instance;
            lock (Bloqueo)
            {
                _instance ??= new VariablesGlobales();
            }

            return _instance;
        }
    }

    public static string? ConnectionString => _configurationBuilder?.Build()?.GetConnectionString("ConnectionString");
    public static string? ConnectionStringReport => _configurationBuilder?.Build()?.GetConnectionString("ConnectionStringReport");

    public IConfigurationSection? ConfigurationSection => _configurationBuilder?.Build().GetSection("globals");

    public Usuario? Usuario { get; set; }

    public UnityContainer UnityContainer { get; }

    public ConfiguracionGeneral ConfiguracionGeneral { get; private set; }

    public DataContext DataContext { get; set; }

}