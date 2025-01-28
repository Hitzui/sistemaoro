using System.Windows;
using System.Windows.Controls;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using SistemaOro.Forms.ViewModels.Clientes;
using Unity;

namespace SistemaOro.Forms.Views.Clientes
{
    /// <summary>
    /// Interaction logic for Listado.xaml
    /// </summary>
    public partial class Listado : Page
    {
        public Listado()
        {
            InitializeComponent();
        }

        private void OnSearchStringToFilterCriteria(object sender, SearchStringToFilterCriteriaEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SearchString))
            {
                var filter = e.SearchString.Trim().ToLower();
                e.Filter = CriteriaOperator.FromLambda<Cliente>(cliente => cliente.Codcliente.Contains(filter)
                                                                           || cliente.Nombres.Contains(filter)
                                                                           || (cliente.Apellidos != null && cliente.Apellidos.Contains(filter))
                                                                           || cliente.Numcedula.Contains(filter));
            }

            e.ApplyToColumnsFilter = true;
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //llamar a formulario editar cliente
            var frm = new Form();
            if (frm.DataContext is not ClienteFormViewModel clienteFormViewModel) return;
            clienteFormViewModel.Load(VariablesGlobalesForm.Instance.SelectedCliente);
            frm.ShowDialog();
        }

        private async void BarButtonItem_ItemClick_1(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //eliminar el cliente
            var messageBox = HelpersMessage.MensajeConfirmacionResult(ClienteMessages.TituloEliminarCliente, ClienteMessages.EliminarClienteContent);
            if (messageBox == MessageBoxResult.Cancel)
            {
                VariablesGlobalesForm.Instance.SelectedCliente = null;
                return;
            }

            var selectedCliente = (Cliente)GridListadoCliente.CurrentItem;
            var repositoryCliente = VariablesGlobales.Instance.UnityContainer.Resolve<IClienteRepository>();
            var result = await repositoryCliente.DeleteAsync(selectedCliente);
            if (result)
            {
                HelpersMessage.DialogWindow(ClienteMessages.TituloEliminarCliente, ClienteMessages.ClienteEliminadoSuccess).ShowDialog();
            }
            else
            {
                HelpersMessage.DialogWindow(ClienteMessages.TituloEliminarCliente, $"{ClienteMessages.ClienteEliminadoError}\n{repositoryCliente.ErrorSms}").ShowDialog();
            }

            VariablesGlobalesForm.Instance.SelectedCliente = null;
        }
    }
}