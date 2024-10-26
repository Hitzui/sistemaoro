using DevExpress.XtraReports.UI;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using DevExpress.Drawing;

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

    public static void LoadReport(XtraReport report, string title="Previsualizar Reporte")
    {
        var reportPrintTool = new ReportPrintTool(report);
        reportPrintTool.PreviewForm.Text = title;
        reportPrintTool.PreviewRibbonForm.WindowState = FormWindowState.Maximized;
        reportPrintTool.AutoShowParametersPanel = false;
        report.CreateDocument(false);
        reportPrintTool.ShowRibbonPreview();
    }
}