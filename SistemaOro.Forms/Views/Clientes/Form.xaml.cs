using System.Windows;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.ViewModels.Clientes;

namespace SistemaOro.Forms.Views.Clientes;

public partial class Form : Window
{
    private readonly ClienteFormViewModel _viewModel;
    public Form()
    {
        InitializeComponent();
        _viewModel = (ClienteFormViewModel)DataContext;
    }

    public static Cliente? SelectedCliente { get; set; }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _viewModel.Load(SelectedCliente);
    }
}