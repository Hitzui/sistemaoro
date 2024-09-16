using System.IO;
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
}