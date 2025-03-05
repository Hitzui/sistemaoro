using System.Windows.Controls;
using SistemaOro.Forms.ViewModels.Descargue;

namespace SistemaOro.Forms.Views.Descargue;

public partial class ListaDescarguesPage : Page
{

    private readonly ListaDescarguesViewModels _viewModel;
    public ListaDescarguesPage()
    {
        InitializeComponent();
        _viewModel = new ListaDescarguesViewModels();
        DataContext = _viewModel;
    }
}