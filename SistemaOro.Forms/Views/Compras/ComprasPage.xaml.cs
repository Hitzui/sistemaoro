using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
using SistemaOro.Forms.ViewModels;
using SistemaOro.Forms.ViewModels.Compras;

namespace SistemaOro.Forms.Views.Compras
{
    /// <summary>
    /// Interaction logic for ComprasPage.xaml
    /// </summary>
    public partial class ComprasPage : Page
    {
        private ComprasViewModel viewModel;
        public ComprasPage()
        {
            InitializeComponent();
            viewModel = ((ComprasViewModel)DataContext);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.Load();
            viewModel.Source = Source;
        }

        private void BarItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            viewModel.ImprimirCompra();
        }

        private void EditarCompraItem_OnItemClick(object sender, ItemClickEventArgs e)
        {
            viewModel.EditarCompraCommand(NavigationService);
        }

        private void ImprimirTicket_ItemClick(object sender, ItemClickEventArgs e)
        {
            viewModel.ImprimirTicketCompra();
        }

        private void OnSearchStringToFilterCriteria(object sender, SearchStringToFilterCriteriaEventArgs e)
        {
            if (string.IsNullOrEmpty(e.SearchString)) return;
            var search = e.SearchString.Trim();
            // Definir condiciones de búsqueda en varias columnas
            var criteria = new GroupOperator(GroupOperatorType.Or,
                new BinaryOperator("Numcompra", search, BinaryOperatorType.Equal),
                new BinaryOperator("Codcliente", search, BinaryOperatorType.Equal),
                new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("NombreCliente"), search)
            );

            e.Filter = criteria;
            e.ApplyToColumnsFilter = true;
        }
    }
}
