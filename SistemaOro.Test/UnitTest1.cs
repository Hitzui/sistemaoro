using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;

namespace SistemaOro.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestUsuario()
    {
        var usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IUsuarioRepository>();
        var listUser = await usuarioRepository.FindAll();
        foreach (var usuario in listUser)
        {
            Console.WriteLine($@"Usuario: {usuario.Usuario1} creado en {usuario.Fcreau.ToLongDateString()}");
        }
    }

    [Test]
    public void TestConfiguracionGeneral()
    {
        VariablesGlobales.Instance.ConfiguracionGeneral.Caja = "C001";
        VariablesGlobales.Instance.ConfiguracionGeneral.Agencia = "A001";
        Console.WriteLine($@"Configuración: {VariablesGlobales.Instance.ConfiguracionGeneral.Caja} y {VariablesGlobales.Instance.ConfiguracionGeneral.Agencia}");
    }
}