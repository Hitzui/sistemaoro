using System;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using DevExpress.Drawing;
using NLog;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.Views.Reportes.Compras;
using Topaz;
using System.Linq;
using System.Windows.Media;
using DevExpress.Xpf.Editors;
using ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource;

namespace SistemaOro.Forms.Services.Helpers;

public static class HelpersMethods
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

    public static DXImage LoadDxImage(byte[] b)
    {
        var ms = new MemoryStream(b);
        var bitmap = DXImage.FromStream(ms);
        return bitmap;
    }

    public static void LoadReport(XtraReport report, string title = "Previsualizar Reporte")
    {
        var reportPrintTool = new ReportPrintTool(report);
        reportPrintTool.PreviewRibbonForm.Text = title;
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
        string[] decenas =
            { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        string[] decenasCompuestas =
        {
            "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve"
        };
        string[] centenas =
        {
            "", "Cien", "Doscientos", "Trescientos", "Cuatrocientos", "Quinientos", "Seiscientos", "Setecientos",
            "Ochocientos", "Novecientos"
        };

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

    public static decimal RedondeoHaciaArriba(decimal valor, int decimales = 4)
    {
        if (valor == 0m)
        {
            return valor;
        }
        var format = $"F{decimales}";
        var auxiliarDecimal = valor.ToString(format);
        return decimal.Parse(auxiliarDecimal);
    }

    public static Image LoadSigImage(string sigString, string nameFirma = "", bool save = true)
    {
        var sigPlusNet = new SigPlusNET();
        sigPlusNet.AutoKeyStart();
        sigPlusNet.SetAutoKeyANSIData("123");
        sigPlusNet.AutoKeyFinish();
        sigPlusNet.SetEncryptionMode(2);

        sigPlusNet.SetSigCompressionMode(1);
        sigPlusNet.SetSigString(sigString);
        // Obtener imagen y guardar
        sigPlusNet.SetImageXSize(300);
        sigPlusNet.SetImageYSize(150);
        sigPlusNet.SetImagePenWidth(7);
        sigPlusNet.SetImageFileFormat(0);
        var firma = sigPlusNet.GetSigImage();
        using var ms = new MemoryStream();
        firma.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        var image = Image.FromStream(ms);

        if (!save) return image;
        // Guardar imagen
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

        image.Save(ruta);

        return image;
    }

    public static void ImprimirReportesCompra(Compra compra, List<DetalleCompra> findCompra)
    {
        Logger.Info($"Numero de compra {compra.Numcompra} - Cantidad de datos: {findCompra.Count}");
        try
        {
            var agencia = VariablesGlobalesForm.Instance.Agencia;
            if (agencia is null)
            {
                Logger.Error($"No fue posible encontrar la agencia {compra.Codagencia}");
                return;
            }

            if (findCompra.Count > 0)
            {
                foreach (var detalleCompra in findCompra)
                {
                    Logger.Info($"Datos: {detalleCompra.Numcompra} - {detalleCompra.Nocontrato}");
                }
            }

            // Reporte Anexo
            var reporteAnexo = new ReporteAnexo();
            reporteAnexo.DataSource = findCompra;

            if (!string.IsNullOrWhiteSpace(compra.Firma))
            {
                var image = LoadSigImage(compra.Firma, compra.Numcompra ?? "");
                reporteAnexo.imgFirma.ImageSource = new ImageSource(image);
            }

            if (compra.Huella is not null && compra.Huella.Length > 0)
            {
                reporteAnexo.imgHuella.ImageSource = new ImageSource(LoadDxImage(compra.Huella));
            }

            if (!string.IsNullOrWhiteSpace(agencia.Firma))
            {
                var image = LoadSigImage(agencia.Firma, compra.Numcompra ?? "", false);
                reporteAnexo.imgFirmaPrincipal.ImageSource = new ImageSource(image);
            }

            LoadReport(reporteAnexo);

            // Reporte Contrato Contra Venta
            var reporteContrantoContraVenta = new ReporteContratoContraVenta();
            reporteContrantoContraVenta.DataSource = findCompra;
            if (compra.Huella is not null && compra.Huella.Length > 0)
            {
                reporteContrantoContraVenta.imgHuella.ImageSource = new ImageSource(LoadDxImage(compra.Huella));
            }

            if (!string.IsNullOrWhiteSpace(compra.Firma))
            {
                var image = LoadSigImage(compra.Firma, compra.Numcompra ?? "");
                reporteContrantoContraVenta.imgFirma.ImageSource = new ImageSource(image);
            }

            if (!string.IsNullOrWhiteSpace(agencia.Firma))
            {
                var image = LoadSigImage(agencia.Firma, compra.Numcompra ?? "", false);
                reporteContrantoContraVenta.imgFirmaPrincipal.ImageSource = new ImageSource(image);
            }

            LoadReport(reporteContrantoContraVenta);

            // Reporte Contrato Prestamo
            var reporteContrantoPrestamo = new ReporteContrantoPrestamo();
            reporteContrantoPrestamo.DataSource = findCompra;
            if (!string.IsNullOrWhiteSpace(compra.Firma))
            {
                var image = LoadSigImage(compra.Firma, compra.Numcompra ?? "");
                reporteContrantoPrestamo.imgFirma.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(image);
            }

            if (!string.IsNullOrWhiteSpace(agencia.Firma))
            {
                var image = LoadSigImage(agencia.Firma, compra.Numcompra ?? "", false);
                reporteContrantoPrestamo.imgFirmaPrincipal.ImageSource =
                    new DevExpress.XtraPrinting.Drawing.ImageSource(image);
            }

            LoadReport(reporteContrantoPrestamo);

            // Reporte Comprobante de Compra
            var reporteComprobanteCompra = new ReporteComprobanteCompra();
            reporteComprobanteCompra.DataSource = findCompra;
            LoadReport(reporteComprobanteCompra);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error al imprimir reportes de compra");
            HelpersMessage.MensajeErroResult("Error", e.Message);
        }
    }

    public static string ConvertImageToBase64(Image image)
    {
        using MemoryStream ms = new MemoryStream();
        // Guardar la imagen en el MemoryStream en formato JPEG (puedes cambiarlo a PNG, etc.)
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp); // Cambia a ImageFormat.Png si es PNG

        // Convertir los bytes a Base64
        byte[] imageBytes = ms.ToArray();
        string base64String = Convert.ToBase64String(imageBytes);
        return base64String;
    }

    public static Image ConvertBase64ToImage(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        using (MemoryStream ms = new MemoryStream(imageBytes))
        {
            return Image.FromStream(ms);
        }
    }

    public static BitmapImage ConvertBitmapToImageSource(byte[] imageBytes, int mImageWidth, int mImageHeight)
    {
        int colorval;
        var bmp = new Bitmap(mImageWidth, mImageHeight);
        for (int i = 0; i < bmp.Width; i++)
        {
            for (int j = 0; j < bmp.Height; j++)
            {
                colorval = (int)imageBytes[(j * mImageWidth) + i];
                bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(colorval, colorval, colorval));
            }
        }

        using var memory = new MemoryStream();
        bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
        memory.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze(); // importante si lo usas en diferentes hilos

        return bitmapImage;
    }

    public static BitmapImage ByteArrayToBitmapImage(byte[] bytes)
    {
        using (var stream = new MemoryStream(bytes))
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze(); // Para uso seguro en hilos
            return image;
        }
    }


    public static BitmapSource? LoadImageFromBytes(byte[] imageBytes, int mImageWidth, int mImageHeight)
    {
        if (imageBytes.Length == 0)
        {
            Logger.Info("Datos de imagen en 0");
            return null;
        }

        try
        {
            var stride = mImageWidth * 1;
            var bitmap = BitmapSource.Create(
                mImageWidth,
                mImageHeight,
                96, 96, // DPI
                PixelFormats.Gray32Float,
                null,
                imageBytes,
                stride
            );

            bitmap.Freeze();
            return bitmap;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return null;
        }
    }
}