using System.Windows;
using DevExpress.Xpf.Core;
using SistemaOro.Forms.ViewModels.Cajas;

namespace SistemaOro.Forms.Views.Cajas
{
    /// <summary>
    /// Interaction logic for AgregarMovimientoCaja.xaml
    /// </summary>
    public partial class AgregarMovimientoCaja : ThemedWindow
    {
        public AgregarMovimientoCaja()
        {
            InitializeComponent();
            ((AgregarMovimientoCajaViewModel)DataContext).CloseAction = Close;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((AgregarMovimientoCajaViewModel)DataContext).Load();
        }
    }
}
