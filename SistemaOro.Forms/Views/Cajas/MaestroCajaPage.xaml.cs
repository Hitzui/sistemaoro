using SistemaOro.Forms.ViewModels.Cajas;
using System.Windows;
using System.Windows.Controls;

namespace SistemaOro.Forms.Views.Cajas
{
    /// <summary>
    /// Lógica de interacción para MaestroCajaPage.xaml
    /// </summary>
    public partial class MaestroCajaPage : Page
    {
        private MaestroCajaViewModel _viewModel;
        public MaestroCajaPage()
        {
            InitializeComponent();
            _viewModel = new MaestroCajaViewModel();
            DataContext = _viewModel;
            _viewModel = (MaestroCajaViewModel)DataContext;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }

        private void Page_GotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }
    }
}
