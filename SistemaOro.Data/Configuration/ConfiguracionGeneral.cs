using System.Xml.Linq;
using SistemaOro.Data.Libraries;

namespace SistemaOro.Data.Configuration;

public class ConfiguracionGeneral
{
    private const string Ruta = @"c:\config\opcion.xml";
    private XDocument? _xdoc;

    /// <summary>
    /// Lee el archivo de configuración y lo carga en memoria
    /// </summary>
    /// <returns>Archivo XML de config</returns>
    public XDocument? CargarDocumento()
    {
        try
        {
            _xdoc = XDocument.Load(Ruta);
        }
        catch (Exception)
        {
           CrearArchivo();
        }

        return _xdoc;
    }

    private bool ExisteArchivo()
    {
        return File.Exists(Ruta);
    }

    private static bool ExisteCarpet()
    {
        return Directory.Exists(@"c:\config");
    }

    /// <summary>
    /// Crea el archivo de configuración
    /// </summary>
    private void CrearArchivo()
    {
        if (!ExisteCarpet())
        {
            Directory.CreateDirectory(@"c:\config");
        }

        if (ExisteArchivo()) return;
        _xdoc = new XDocument(
            new XElement("Configuracion",
                new XElement("Caja", "C001"),
                new XElement("Agencia", "A001"),
                new XElement("Instancia", "server"),
                new XElement("Catalogo", "EfectiOro"),
                new XElement("Usuario", "sa"),
                new XElement("Password", "seguro"),
                new XElement("Security", "False")
            )
        );
        _xdoc.Save(Ruta);
    }

    #region Archivo XML de configuracion

    public static string? Caja
    {
        get=>VariablesGlobales.Instance.ConfigurationSection["CAJA"];
        set => Utilities.UpdateAppSetting("CAJA", value);
    }

    public static string? Agencia
    {
        get=> VariablesGlobales.Instance.ConfigurationSection["AGENCIA"];
        set => Utilities.UpdateAppSetting("AGENCIA", value);
    }
    
    #endregion
}