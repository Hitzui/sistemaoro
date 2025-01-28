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
using DevExpress.Mvvm.Xpf;
using SistemaOro.Forms.ViewModels.Monedas;


namespace SistemaOro.Forms.Views.Monedas
{
    /// <summary>
    /// Interaction logic for MonedasWindow.xaml
    /// </summary>
    public partial class MonedasWindow : ThemedWindow
    {
        public MonedasWindow()
        {
            InitializeComponent();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((MonedasViewModel)DataContext).Load();
        }
    }
}
