using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using SistemaOro.Forms.ViewModels.Agencias;
using SistemaOro.Forms.Views;
using SistemaOro.Forms.Views.Agencias;
using SistemaOro.Forms.Views.Cajas;
using SistemaOro.Forms.Views.Clientes;
using SistemaOro.Forms.Views.Rubros;
using Unity;
using Listado = SistemaOro.Forms.Views.Clientes.Listado;

namespace SistemaOro.Forms.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Frame _mainFrame;

        public MainViewModel()
        {
            ListadoMovimientosCajasCommand = new DelegateCommand(ListadoMovimientosCajas);
            AgregarClienteCommand = new DelegateCommand(OnAgregarCliente);
            ListaClientesCommand = new DelegateCommand(OnListadoClientes);
            EditarClienteCommand = new DelegateCommand(OnEditarClienteCommand);
            TiposDocumentosCommand = new DelegateCommand(OnTiposDocumentosCommand);
            DeleteClienteCommand = new DelegateCommand(OnDeleteClienteCommand);
            TipoCambioCommand = new DelegateCommand(OnTipoCambioCommand);
            ListadoCajasCommand = new DelegateCommand(OnListadoCajasCommand);
            AgenciasCommand = new DelegateCommand(OnAgenciasCommand);
            EditAgenciaCommand = new DelegateCommand(OnFormAgenciaCommand);
            NuevaAgenciaCommand = new DelegateCommand(OnNuevaAgenciaCommand);
            EliminarAgenciaCommand = new DelegateCommand(OnEliminarAgenciaCommand);
            AgregarMovimientoCajaCommand = new DelegateCommand(OnAgregarMovimientoCajaCommand);
            EditarMovimientoCajaCommand = new DelegateCommand(OnEditarMovimientoCajaCommand);
            RubroCommand = new DelegateCommand(OnRubroCommand);
            MaestroCajaCommand = new DelegateCommand(OnMaestroCajaCommand);
            _mainFrame = new Frame();
        }

        private void OnMaestroCajaCommand()
        {
            _mainFrame.Navigate(new MaestroCajaPage());
        }

        private void OnRubroCommand()
        {
            _mainFrame.Navigate(new RubrosPage());
        }

        private void OnEditarMovimientoCajaCommand()
        {
            var frm = new AgregarMovimientoCaja();
            frm.ShowDialog();
        }

        private void OnAgregarMovimientoCajaCommand()
        {
            var frm = new AgregarMovimientoCaja();
            VariablesGlobalesForm.Instance.MovCajasDtoSelected = null;
            frm.ShowDialog();
        }

        private void OnEliminarAgenciaCommand()
        {
            var mvm = new FormAgenciaViewModel();
            mvm.DeleteAgencia(VariablesGlobalesForm.Instance.SelectedAgencia);
        }

        private void OnNuevaAgenciaCommand()
        {
            var frm = new FormAgencia
            {
                SelectedAgencia = null
            };
            frm.ShowDialog();
        }

        private void OnFormAgenciaCommand()
        {
            if (VariablesGlobalesForm.Instance.SelectedAgencia is null)
            {
                return;
            }

            var frm = new FormAgencia
            {
                SelectedAgencia = VariablesGlobalesForm.Instance.SelectedAgencia
            };
            frm.ShowDialog();
        }

        private void OnAgenciasCommand()
        {
            _mainFrame.Navigate(new AgenciasPage());
        }

        private void OnListadoCajasCommand()
        {
            var pageCajas = new CajasPage();
            var themedWindow = HelpersWindows.DefaultWindow(pageCajas);
            themedWindow.ShowDialog();
        }

        private void OnTipoCambioCommand()
        {
            var mainWindow = new Views.TipoCambio.ListadoTipoCambio();
            mainWindow.ShowDialog();
        }

        private async void OnDeleteClienteCommand()
        {
            if (VariablesGlobalesForm.Instance.SelectedCliente is null)
            {
                HelpersMessage.DialogWindow(ClienteMessages.TituloEliminarCliente, ClienteMessages.SeleccionarCliente).ShowDialog();
                return;
            }

            var messageBox = HelpersMessage.MensajeConfirmacionResult(ClienteMessages.TituloEliminarCliente, ClienteMessages.EliminarClienteContent);
            if (messageBox == MessageBoxResult.Cancel)
            {
                VariablesGlobalesForm.Instance.SelectedCliente = null;
                return;
            }

            var repositoryCliente = VariablesGlobales.Instance.UnityContainer.Resolve<IClienteRepository>();
            var result = await repositoryCliente.Delete(VariablesGlobalesForm.Instance.SelectedCliente);
            if (result)
            {
                HelpersMessage.DialogWindow(ClienteMessages.TituloEliminarCliente, ClienteMessages.ClienteEliminadoSuccess).ShowDialog();
                VariablesGlobalesForm.Instance.SelectedCliente = null;
            }
            else
            {
                HelpersMessage.DialogWindow(ClienteMessages.TituloEliminarCliente, $"{ClienteMessages.ClienteEliminadoError}\n{repositoryCliente.ErrorSms}").ShowDialog();
                VariablesGlobalesForm.Instance.SelectedCliente = null;
            }
        }

        private void OnTiposDocumentosCommand()
        {
            var page = new Views.TipoDocumento.TipoDocumentoPage();
            page.Show();
        }

        private void OnEditarClienteCommand()
        {
            if (VariablesGlobalesForm.Instance.SelectedCliente is null) return;
            var mainWindow = new Form();
            Form.SelectedCliente = VariablesGlobalesForm.Instance.SelectedCliente;
            mainWindow.ShowDialog();
        }

        private void OnAgregarCliente()
        {
            var mainWindow = new Form();
            Form.SelectedCliente = null;
            mainWindow.ShowDialog();
        }

        private void OnListadoClientes()
        {
            _mainFrame.Navigate(new Listado());
        }


        public ICommand ListaClientesCommand
        {
            get { return GetProperty(() => ListaClientesCommand); }
            set
            {
                SetProperty(() => ListaClientesCommand, value);
                RaisePropertyChanged();
            }
        }

        public ICommand MaestroCajaCommand { get; set; }
        public ICommand RubroCommand { get; set; }
        public ICommand AgregarMovimientoCajaCommand { get; set; }
        public ICommand EditarMovimientoCajaCommand { get; set; }
        public ICommand EliminarAgenciaCommand { get; }
        public ICommand EditAgenciaCommand { get; }
        public ICommand NuevaAgenciaCommand { get; }

        public ICommand AgenciasCommand { get; }
        public ICommand ListadoMovimientosCajasCommand { get; }
        public ICommand ListadoCajasCommand { get; }

        public ICommand TipoCambioCommand { get; set; }

        public ICommand DeleteClienteCommand { get; set; }

        public ICommand EditarClienteCommand { get; set; }

        public ICommand TiposDocumentosCommand { get; set; }

        public ICommand AgregarClienteCommand
        {
            get { return GetProperty(() => AgregarClienteCommand); }
            set
            {
                SetProperty(() => AgregarClienteCommand, value);
                RaisePropertyChanged();
            }
        }

        public void SetMainFrame(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        private void ListadoMovimientosCajas()
        {
            _mainFrame.Navigate(new MovimientosCajasPage());
        }
    }
}