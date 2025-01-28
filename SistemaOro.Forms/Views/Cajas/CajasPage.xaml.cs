using System.Windows.Controls;
using SistemaOro.Forms.ViewModels.Cajas;

namespace SistemaOro.Forms.Views.Cajas
{
    /// <summary>
    /// Interaction logic for CajasPage.xaml
    /// </summary>
    public partial class CajasPage : Page
    {
        public CajasPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ((CajasViewModel)DataContext).OnLoad();
        }
    }
}
