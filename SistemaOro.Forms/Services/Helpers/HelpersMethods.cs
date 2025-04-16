using System;
using System.Drawing;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using DevExpress.Drawing;
using DevExpress.XtraEditors;
using Topaz;

namespace SistemaOro.Forms.Services.Helpers;

public class HelpersMethods
{
    public static BitmapImage LoadImage(byte[] b)
    {
        var ms = new MemoryStream(b);
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = ms;
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        return bitmap;
    }

    public static DXImage LoadDxImage(byte[]? b)
    {
        var ms = new MemoryStream(b);
        var bitmap = DXImage.FromStream(ms);
        return bitmap;
    }

    public static void LoadReport(XtraReport report, string title = "Previsualizar Reporte")
    {
        var reportPrintTool = new ReportPrintTool(report);
        reportPrintTool.PreviewForm.Text = title;
        reportPrintTool.PreviewRibbonForm.WindowState = FormWindowState.Maximized;
        reportPrintTool.AutoShowParametersPanel = false;
        report.RequestParameters = false;
        report.CreateDocument(false);
        reportPrintTool.ShowRibbonPreview();
    }

    public static string ConvertirNumeroADecimalATexto(decimal numero)
    {
        // Separar la parte entera y la parte decimal
        var parteEntera = (int)numero;
        var parteDecimal = (int)((numero - parteEntera) * 100); // Multiplicamos por 100 para obtener los centavos

        var textoEntero = ConvertirNumeroATexto(parteEntera);
        var textoDecimal = ConvertirNumeroATexto(parteDecimal);

        // Construir el resultado final
        return parteDecimal > 0 ? $"{textoEntero} con {textoDecimal} centavos" : textoEntero;
    }

    private static string ConvertirNumeroATexto(int numero)

    {
        switch (numero)
        {
            case 0:
                return "cero";
            case < 0:
                return "menos " + ConvertirNumeroATexto(-numero);
        }


        string[] unidades = { "", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
        string[] decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        string[] decenasCompuestas = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
        string[] centenas = { "", "Cien", "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos", "Ochocientos", "Novecientos" };

        string resultado = "";
        if (numero >= 1000)
        {
            // Manejar el caso de mil
            if (numero / 1000 == 1)
            {
                resultado += "Mil ";
            }
            else
            {
                resultado += ConvertirNumeroATexto(numero / 1000) + " Mil ";
            }

            numero %= 1000;
        }

        if (numero >= 100)
        {
            resultado += centenas[numero / 100] + " ";
            numero %= 100;
        }

        if (numero >= 20)
        {
            resultado += decenas[numero / 10] + " ";
            numero %= 10;
        }
        else if (numero >= 10)
        {
            resultado += decenasCompuestas[numero - 10] + " ";
            numero = 0;
        }

        if (numero > 0)
        {
            resultado += unidades[numero] + " ";
        }

        return resultado.Trim();
    }

    public static decimal RedondeoHaciaArriba(decimal valor, int decimales = 2)
    {
        var format = $"F{decimales}";
        var auxiliarDecimal = valor.ToString(format);
       return decimal.Parse(auxiliarDecimal);
    }

    public static Image LoadSigImage(string sigString, string nameFirma="")
    {
        var sigPlusNet = new SigPlusNET();
        sigPlusNet.AutoKeyStart();
        sigPlusNet.SetAutoKeyANSIData("123");
        sigPlusNet.AutoKeyFinish();
        sigPlusNet.SetEncryptionMode(2);
                
        sigPlusNet.SetSigCompressionMode(1);
        sigPlusNet.SetSigString(sigString);
        
        if (string.IsNullOrWhiteSpace(nameFirma))
        {
            var now = DateTime.Now;
            nameFirma = $"{now.Date}{now.Month}{now.Year}{now.Hour}{now.Minute}{now.Second}";
        }
        var ruta = @$"C:\config\firmas\{nameFirma}.bmp";
        // Crear carpeta si no existe
        var carpeta = Path.GetDirectoryName(ruta);
        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }

        // Obtener imagen y guardar
        sigPlusNet.SetImageXSize(300);
        sigPlusNet.SetImageYSize(150);
        sigPlusNet.SetImagePenWidth(7);
        sigPlusNet.SetImageFileFormat(0);
        var firma = sigPlusNet.GetSigImage();
        using var ms = new MemoryStream();
        firma.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        var image = Image.FromStream(ms);
        image.Save(ruta);
        return image;
    }


}