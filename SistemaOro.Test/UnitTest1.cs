using System.Diagnostics;
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
        var usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IDescarguesRepository>();
        var listUser = await usuarioRepository.FindAll();
        Debug.WriteLine(listUser);
    }
}