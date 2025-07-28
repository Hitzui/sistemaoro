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
    private static VariablesGlobales? _instance;
    private static readonly object Bloqueo = new();
    private static string? _appSettingsPath;

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

    public static string? ConnectionString => ConfigurationBuilder.GetConnectionString("ConnectionString");
    public static string? ConnectionStringReport => ConfigurationBuilder.GetConnectionString("ConnectionStringReport");

    public IConfigurationSection ConfigurationSection => ConfigurationBuilder.GetSection("globals");

    private static IConfigurationRoot ConfigurationBuilder
    {
        get
        {
            //var configurationBuilder = new ConfigurationBuilder();
            _appSettingsPath = "appsettings.json";
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_appSettingsPath, optional: false, reloadOnChange: true);

            return configurationBuilder.Build();
        }
    }

    public Usuario? Usuario { get; set; }

    public UnityContainer UnityContainer { get; }

    public ConfiguracionGeneral ConfiguracionGeneral { get; private set; }
    public DataContext DataContext { get; set; }

}