using System.Windows.Controls;
using SistemaOro.Forms.ViewModels.Descargue;

namespace SistemaOro.Forms.Views.Descargue;

public partial class ListaDescarguesPage : Page
{
    public ListaDescarguesPage()
    {
        InitializeComponent();
        var viewModel = new ListaDescarguesViewModels();
        DataContext = viewModel;
    }
}