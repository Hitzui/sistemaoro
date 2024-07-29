using System.IO;
using System.Windows.Media.Imaging;

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
}