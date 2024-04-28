using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using SistemaOro.Data.Configuration;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Repositories;
using Unity;
using Unity.Injection;

namespace SistemaOro.Data.Libraries;

public class VariablesGlobales
{
    private static VariablesGlobales? _instance;
    private static readonly object Bloqueo = new();
    private static string? _appSettingsPath;

    private VariablesGlobales()
    {
        var dataContext = new DataContext();
        ConfiguracionGeneral = new ConfiguracionGeneral();
        UnityContainer = new UnityContainer();
        UnityContainer.RegisterSingleton<IMaestroCajaRepository, MaestroCajaRepository>();
        UnityContainer.RegisterSingleton<IParametersRepository, ParametersRepository>();
        UnityContainer.RegisterSingleton<IAdelantosRepository, AdelantosRepository>();
        UnityContainer.RegisterSingleton<ICierrePrecioRepository, CierrePrecioRepository>();
        UnityContainer.RegisterSingleton<IMonedaRepository, MonedaRepository>();
        UnityContainer.RegisterSingleton<IUsuarioRepository, UsuarioRepository>(new InjectionConstructor(dataContext));
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
}