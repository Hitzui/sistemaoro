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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.ViewModels.Compras;

namespace SistemaOro.Forms.Views.Compras
{
    /// <summary>
    /// Interaction logic for FormComprasPage.xaml
    /// </summary>
    public partial class FormComprasPage : Page
    {
        public FormComprasPage()
        {
            InitializeComponent();
            ((FormComprasViewModel)DataContext).CloseAction += () =>
            {
                NavigationService.Navigate(new ComprasPage());
            };
        }

        public void SetSelectedCompra(DtoComprasClientes compra)
        {
            ((FormComprasViewModel)DataContext).SelectedCompra = compra;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((FormComprasViewModel)DataContext).LoadValues();
        }

        private void CmbPrecios_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            TxtPrecio.Focus();
        }

        private void TxtPrecio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TxtPeso.Focus();
            }
        }

        private void TxtPeso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TxtImporte.Focus();
            }
        }

        private void TxtImporte_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAgregar.Focus();
            }
        }

        private void GridDetaCompra_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName is "Importe" or "Preciok" or "Peso")
            {
                e.DisplayText =$"{e.Value:N2}";
            }
        }
    }
}