using System;
using System.Windows;
using SistemaOro.Forms.ViewModels.TipoCambio;

namespace SistemaOro.Forms.Views.TipoCambio
{
    /// <summary>
    /// Interaction logic for ListadoTipoCambio.xaml
    /// </summary>
    public partial class ListadoTipoCambio : Window
    {
        public ListadoTipoCambio()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((TipoCambioViewModel)DataContext).OnLoad();
            
        }
    }
}