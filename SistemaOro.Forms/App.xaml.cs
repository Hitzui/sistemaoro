using DevExpress.Xpf.Core;
using OfficeOpenXml;
using System.Windows;
using Unity;

namespace SistemaOro.Forms
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UnityContainer UnityContainer => new UnityContainer();
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
