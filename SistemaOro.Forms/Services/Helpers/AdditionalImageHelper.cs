using System.Windows;
using System.Windows.Media;

namespace SistemaOro.Forms.Services.Helpers;

public class AdditionalImageHelper
{
    public static ImageSource GetAdditionalIcon(DependencyObject obj)
    {
        return (ImageSource)obj.GetValue(AdditionalIconProperty);
    }

    public static void SetAdditionalIcon(DependencyObject obj, ImageSource value)
    {
        obj.SetValue(AdditionalIconProperty, value);
    }

    public static readonly DependencyProperty AdditionalIconProperty =
        DependencyProperty.RegisterAttached("AdditionalIcon", typeof(ImageSource), typeof(AdditionalImageHelper), new PropertyMetadata(null));
}