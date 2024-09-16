using DevExpress.Xpf.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using SistemaOro.Forms.Services;
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
            CompatibilitySettings.UseLightweightThemes = true;
            ApplicationThemeHelper.Preload(PreloadCategories.Core);
        }
    }
}
