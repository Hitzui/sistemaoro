using System.Xml.Linq;

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
        catch (Exception ex)
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
                new XElement("Caja", "C01"),
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

    public string Agencia
    {
        get
        {
            var val = _xdoc.Descendants("Agencia").Select(x => x).Single().Value;
            return val;
        }
        set
        {
            var xVElemental = _xdoc.Elements("Configuracion").SingleOrDefault(element => element.Name == "Agencia");
            if (xVElemental is null)
            {
                return;
            }

            xVElemental.Value = value;
            xVElemental.Save(Ruta);
        }
    }

    public string Caja
    {
        get
        {
            var element = _xdoc.Element("Configuracion")!.Element("Caja");
            return element is null ? "" : element.Value;
        }
        set
        {
            var element = _xdoc.Element("Configuracion")!.Element("Caja");
            if (element is null)
            {
                return;
            }

            element.Value = value;
            element.Save(Ruta);
        }
    }

    public string Instance
    {
        get => (from x in _xdoc.Descendants("Instancia") select x).Single().Value;
        set
        {
            var element = _xdoc.Element("Configuracion")!.Element("Instancia");
            if (element is null)
            {
                return;
            }

            element.Value = value;
            element.Save(Ruta);
        }
    }

    public string Catalogo
    {
        get => _xdoc.Element("Configuracion")!.Element("Catalogo")!.Value;
        set
        {
            var element = _xdoc.Element("Configuracion")!.Element("Catalogo");
            if (element is null)
            {
                return;
            }

            element.Value = value;
            element.Save(Ruta);
        }
    }

    public string Usuario
    {
        get => _xdoc.Element("Configuracion")!.Element("Usuario")!.Value;
        set
        {
            var element = _xdoc.Element("Configuracion")!.Element("Usuario");
            if (element is null)
            {
                return;
            }

            element.Value = value;
            element.Save(Ruta);
        }
    }

    public string Password
    {
        get => _xdoc.Element("Configuracion")!.Element("Password")!.Value;
        set
        {
            var element = _xdoc.Element("Configuracion")!.Element("Password");
            if (element is null)
            {
                return;
            }

            element.Value = value;
            element.Save(Ruta);
        }
    }

    public string Security
    {
        get => _xdoc.Element("Configuracion")!.Element("Security")!.Value;
        set
        {
            var element = _xdoc.Element("Configuracion")!.Element("Security");
            if (element is null)
            {
                return;
            }

            element.Value = value;
            element.Save(Ruta);
        }
    }

    #endregion
}