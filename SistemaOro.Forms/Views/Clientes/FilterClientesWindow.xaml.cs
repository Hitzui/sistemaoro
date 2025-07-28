using System.Windows;
using DevExpress.Xpf.Core;
using SistemaOro.Forms.ViewModels.Clientes;


namespace SistemaOro.Forms.Views.Clientes
{
    /// <summary>
    /// Interaction logic for FilterClientesWindow.xaml
    /// </summary>
    public partial class FilterClientesWindow : ThemedWindow
    {
        public FilterClientesWindow()
        {
            InitializeComponent();
            if (DataContext is FilterClientesViewModel viewModel)
            {
                viewModel.CloseAction = () =>
                {
                    DialogResult = viewModel.SelectedCliente is not null;
                    Close();
                };
            }
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((FilterClientesViewModel)DataContext).Load();
        }
    }
}
