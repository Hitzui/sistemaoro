using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.ViewModels.Compras;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SistemaOro.Forms.Views.Compras
{
    /// <summary>
    /// Interaction logic for FormComprasPage.xaml
    /// </summary>
    public partial class FormComprasPage : Page
    {
        private readonly FormComprasViewModel viewModel;
        public FormComprasPage()
        {
            InitializeComponent();
            viewModel = (FormComprasViewModel)DataContext;
            viewModel.CloseAction += () =>
            {
                NavigationService?.Navigate(new ComprasPage());
            };
        }

        public void SetSelectedCompra(ComprasCliente compra)
        {
            viewModel.SelectedCompra = compra;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.LoadValues();
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
                TxtLectura.Focus();
            }
        }

        private void TxtLectura_KeyDown(object sender, KeyEventArgs e)
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

        private async void CmbPrecios_OnPopupOpened(object sender, RoutedEventArgs re)
        {
            try
            {
                await viewModel.LoadPrecios();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel.Unloaded();
        }
    }
}