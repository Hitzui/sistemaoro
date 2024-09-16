using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SistemaOro.Forms.ViewModels.Precios;


namespace SistemaOro.Forms.Views.Precios
{
    /// <summary>
    /// Interaction logic for TiposPreciosWindow.xaml
    /// </summary>
    public partial class TiposPreciosWindow : ThemedWindow
    {
        public TiposPreciosWindow()
        {
            InitializeComponent();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((TiposPreciosViewModel)DataContext).Load();
        }
    }
}
