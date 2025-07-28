using DevExpress.Xpf.Core;
using OfficeOpenXml;
using System.Windows;
using SistemaOro.Forms.ViewModels;
using Unity;

namespace SistemaOro.Forms
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UnityContainer UnityContainer;
        static App()
        {
            ExcelPackage.License.SetNonCommercialPersonal("SunMetals");
            //CompatibilitySettings.UseLightweightThemes = true;
            ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Office2019Colorful.Name;
            //LightweightThemeManager.AllowStandardControlsTheming = false;
            ApplicationThemeHelper.Preload(PreloadCategories.Core);
            UnityContainer = new UnityContainer();
            UnityContainer.RegisterSingleton<MainViewModel>();
        }
    }
}
