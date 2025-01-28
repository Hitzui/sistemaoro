using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using SistemaOro.Forms.ViewModels;

namespace SistemaOro.Forms.Services.Helpers;

public class HelpersWindows
{
    private static readonly MainWindow MainWindow= new();

    public HelpersWindows()
    {
    }
    public static ThemedWindow DefaultWindow(Page content)
    {
        var themedWindow = new ThemedWindow
        {
            Content = content,
            Title =((BaseViewModel)content.DataContext).Title,
            SizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode = ResizeMode.NoResize,
            Icon = MainWindow.Icon
        };
        return themedWindow;
    }
    public static ThemedWindow NoneWindow(Page content)
    {
        var themedWindow = new ThemedWindow
        {
            Content = content,
            Title =((BaseViewModel)content.DataContext).Title,
            RenderSize = content.RenderSize,
            WindowStyle = WindowStyle.None,
            Icon = MainWindow.Icon
        };
        return themedWindow;
    }
}