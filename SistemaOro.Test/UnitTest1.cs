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
    public async Task TestRubro()
    {
        var usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IUsuarioRepository>();
        var listUser = await usuarioRepository.FindAll();
        foreach (var usuario in listUser)
        {
            Console.WriteLine($@"Usuario: {usuario.Usuario1} creado en {usuario.Fcreau.ToLongDateString()}");
        }
    }
}