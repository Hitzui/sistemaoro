using SistemaOro.Data.Configuration;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Repositories;
using Unity;

namespace SistemaOro.Data.Libraries;

public class VariablesGlobales
{
    private static VariablesGlobales? _instance;
    private static readonly object Bloqueo = new();

    private VariablesGlobales()
    {
        ConfiguracionGeneral = new ConfiguracionGeneral();
        UnityContainer = new UnityContainer();
        UnityContainer.RegisterSingleton<IMaestroCajaRepository, MaestroCajaRepository>();
        UnityContainer.RegisterSingleton<IParametersRepository, ParametersRepository>();
        UnityContainer.RegisterSingleton<IAdelantosRepository, AdelantosRepository>();
        UnityContainer.RegisterSingleton<ICierrePrecioRepository, CierrePrecioRepository>();
        UnityContainer.RegisterSingleton<IMonedaRepository, MonedaRepository>();
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

    public Usuario? Usuario { get; set; }

    public UnityContainer UnityContainer { get; }

    public ConfiguracionGeneral ConfiguracionGeneral { get; private set; }
}