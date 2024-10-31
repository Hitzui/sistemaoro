using DevExpress.Xpf.Core;
using OfficeOpenXml;
using System.Windows;

namespace SistemaOro.Forms
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //CompatibilitySettings.UseLightweightThemes = true;
            ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Office2019Colorful.Name;
            //LightweightThemeManager.AllowStandardControlsTheming = false;
            ApplicationThemeHelper.Preload(PreloadCategories.Core);
        }
    }
}
