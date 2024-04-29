using SistemaOro.Data.Configuration;
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
        ConfiguracionGeneral.Caja = "C001";
        ConfiguracionGeneral.Agencia = "A001";
        Console.WriteLine($@"Configuración: {ConfiguracionGeneral.Caja} y {ConfiguracionGeneral.Agencia}");
    }

    [Test]
    public async Task TestAdelantos()
    {
        var adelantosRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IAdelantosRepository>();
        var findAll = await adelantosRepository.FindAll();
        Console.WriteLine(findAll.Count);
    }
}