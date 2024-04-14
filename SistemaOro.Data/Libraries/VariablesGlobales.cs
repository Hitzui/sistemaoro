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
        UnityContainer = new UnityContainer();
        UnityContainer.RegisterSingleton<IRepositoryParameters, RepositoryParameters>();
        UnityContainer.RegisterSingleton<IRepositoryAdelantos, RepositoryAdelantos>();
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

    public ConfiguracionGeneral ConfiguracionGeneral()
    {
        return new ConfiguracionGeneral();
    }
}