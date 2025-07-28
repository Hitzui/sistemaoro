using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.XtraEditors;
using NLog;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using SistemaOro.Forms.ViewModels.Agencias;
using SistemaOro.Forms.ViewModels.Clientes;
using SistemaOro.Forms.ViewModels.Compras;
using SistemaOro.Forms.ViewModels.Usuarios;
using SistemaOro.Forms.Views.Agencias;
using SistemaOro.Forms.Views.Cajas;
using SistemaOro.Forms.Views.Compras;
using SistemaOro.Forms.Views.Dashboard;
using SistemaOro.Forms.Views.Descargue;
using SistemaOro.Forms.Views.Monedas;
using SistemaOro.Forms.Views.Parametros;
using SistemaOro.Forms.Views.Precios;
using SistemaOro.Forms.Views.Rubros;
using SistemaOro.Forms.Views.Usuarios;
using Unity;
using Form = SistemaOro.Forms.Views.Clientes.Form;
using Listado = SistemaOro.Forms.Views.Clientes.Listado;

namespace SistemaOro.Forms.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Frame mainFrame;
        private readonly IParametersRepository parametersRepository;
        private readonly IMaestroCajaRepository maestroCajaRepository;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public MainViewModel()
        {
            maestroCajaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMaestroCajaRepository>();
            parametersRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IParametersRepository>();
            usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IUsuarioRepository>();
            ListadoMovimientosCajasCommand = new DelegateCommand(ListadoMovimientosCajas);
            ListaClientesCommand = new DelegateCommand(OnListadoClientes);
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
            RealizarMovimientoCajaCommand = new DelegateCommand(OnRealizarMovimientoCajaCommand);
            ReportesMaestroCajaCommand = new DelegateCommand(OnReportesMaestroCajaCommand);
            ListasCompraCommand = new DelegateCommand(OnListasCompraCommand);
            RealizarCompraCommand = new DelegateCommand(OnRealizarCompraCommand);
            TiposPreciosCommand = new DelegateCommand(OnTiposPreciosCommand);
            PrecioKilateCommand = new DelegateCommand(OnPrecioKilateCommand);
            mainFrame = new Frame();
            VariablesGlobalesForm.Instance.MainFrame = mainFrame;
            VariablesGlobalesForm.Instance.MainViewModel = this;
        }

        [Command]
        public void ReportesComprasCommand()
        {
            var page = new FormReportesCompras();
            mainFrame.Navigate(page);
        }
        [Command]
        public void RealizarDescargueCommand()
        {
            var page = new ListaDescarguesPage();
            mainFrame.Navigate(page);
        }


        [Command]
        public void ParametrosCommand()
        {
            var param = new ParametrosPage();
            mainFrame.Navigate(param);
        }

        [Command]
        public void NewUsuarioCommand()
        {
            var frmUsuario = new UsuarioEditWindow();
            VariablesGlobalesForm.Instance.SelectedUsuario = null;
            frmUsuario.ShowDialog();
            if (frmUsuario.DataContext is not UsuarioEditViewModel model) return;
            if (!model.ResultOk) return;
            var frmUsuarios = new ListaUsuarios();
            mainFrame.Navigate(frmUsuarios);
        }

        [Command]
        public void EditUsuarioCommand()
        {
            var frmUsuario = new UsuarioEditWindow();
            frmUsuario.ShowDialog();
            if (frmUsuario.DataContext is not UsuarioEditViewModel model) return;
            if (!model.ResultOk) return;
            var frmUsuarios = new ListaUsuarios();
            mainFrame.Navigate(frmUsuarios);
        }

        [Command]
        public async void DeleteUsuarioCommand()
        {
            try
            {
                var result = XtraMessageBox.Show("¿Seguro quiere elminar al usuario seleccionado? Esta acción no se puede revertir", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }

                if (VariablesGlobalesForm.Instance.Usuario is null)
                {
                    return;
                }

                var selectedUsuario = VariablesGlobalesForm.Instance.SelectedUsuario;
                if (selectedUsuario != null)
                {
                    if (selectedUsuario.Codoperador == VariablesGlobalesForm.Instance.Usuario.Codoperador)
                    {
                        HelpersMessage.MensajeInformacionResult("Eliminar Usuario", "No se puede eliminar el usuario con el cual se ha iniciado sesión.");
                        return;
                    }

                    if (VariablesGlobalesForm.Instance.Usuario.Nivel == Nivel.Caja)
                    {
                        HelpersMessage.MensajeInformacionResult("Eliminar Usuario", "No puede eliminar el usuario, no cuenta con los permisos.");
                        return;
                    }

                    var delete = await usuarioRepository.DeleteAsync(selectedUsuario);
                    if (delete)
                    {
                        HelpersMessage.MensajeInformacionResult("Eliminar", " Se ha eliminado al usuario");
                        var frmUsuarios = new ListaUsuarios();
                        mainFrame.Navigate(frmUsuarios);
                    }
                    else
                    {
                        HelpersMessage.MensajeInformacionResult("Eliminar", $"Se produjo el siguiente error: {usuarioRepository.ErrorSms}");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error al eliminar usuario");
            }
        }

        [Command]
        public void UsuariosListaCommand()
        {
            var frmusuarios = new ListaUsuarios();
            mainFrame.Navigate(frmusuarios);
        }

        [Command]
        public void MonedasWindowCommand()
        {
            var frmMoneda = new MonedasWindow();
            frmMoneda.ShowDialog();
        }

        private void OnPrecioKilateCommand()
        {
            mainFrame.Navigate(new PreciosKilatePage());
        }

        private void OnListasCompraCommand()
        {
            var comprPage = new ComprasPage();
            mainFrame.Navigate(new ComprasPage());
        }

        private void OnTiposPreciosCommand()
        {
            var frm = new TiposPreciosWindow();
            frm.ShowDialog();
        }

        public FormComprasPage formComprasPage = new ();
        private void OnRealizarCompraCommand()
        {
            RbnEditarCompraVisible = true;
            mainFrame.Navigate(formComprasPage);
        }

        [Command]
        public void SaveCompra()
        {
            var viewModel = (FormComprasViewModel)formComprasPage.DataContext;
            viewModel.SaveCompra();
        }

        [Command]
        public void AnularCompra()
        {
            var viewModel = (FormComprasViewModel)formComprasPage.DataContext;
            viewModel.AnularCompra();
        }

        [Command]
        public void DevolucionCompra()
        {
            var viewModel = (FormComprasViewModel)formComprasPage.DataContext;
            viewModel.DevolucionCompra();
        }
        
        [Command]
        public void NuevaCompra()
        {
            var viewModel = (FormComprasViewModel)formComprasPage.DataContext;
            if (viewModel.NuevaCompra())
            {
                return;
            }
            var result = HelpersMessage.DialogWindow("Nueva compra",
                "Hay datos en la tabla, se limpiarán al crear una nueva y compra y no se guardarán, ¿desea continuar?",
                MessageBoxButton.OKCancel);
            if (result.ShowDialog() != true) return;
            formComprasPage = new();
            mainFrame.Navigate(formComprasPage);
        }

        private void OnReportesMaestroCajaCommand()
        {
            mainFrame.Navigate(new ReportesMaestroCajaPage());
        }

        private void OnRealizarMovimientoCajaCommand()
        {
            var frm = new RealizarMovimientoCajaPage();
            frm.ShowDialog();
        }

        private void OnMaestroCajaCommand()
        {
            mainFrame.Navigate(new MaestroCajaPage());
        }

        private void OnRubroCommand()
        {
            mainFrame.Navigate(new RubrosPage());
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
            mainFrame.Navigate(new AgenciasPage());
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
            try
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
                var result = await repositoryCliente.DeleteAsync(VariablesGlobalesForm.Instance.SelectedCliente);
                if (result)
                {
                    HelpersMessage.DialogWindow(ClienteMessages.TituloEliminarCliente, ClienteMessages.ClienteEliminadoSuccess).ShowDialog();
                    VariablesGlobalesForm.Instance.SelectedCliente = null;
                    OnListadoClientes();
                }
                else
                {
                    HelpersMessage.DialogWindow(ClienteMessages.TituloEliminarCliente, $"{ClienteMessages.ClienteEliminadoError}\n{repositoryCliente.ErrorSms}").ShowDialog();
                    VariablesGlobalesForm.Instance.SelectedCliente = null;
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error al eliminar el cliente");
            }
        }

        private void OnTiposDocumentosCommand()
        {
            var page = new Views.TipoDocumento.TipoDocumentoPage();
            page.Show();
        }

        [Command]
        public void EditarClienteCommand()
        {
            if (VariablesGlobalesForm.Instance.SelectedCliente is null) return;
            var mainWindow = new Form();
            if (mainWindow.DataContext is not ClienteFormViewModel clienteFormViewModel) return;
            clienteFormViewModel.Load(VariablesGlobalesForm.Instance.SelectedCliente);
            mainWindow.ShowDialog();
        }

        [Command]
        public void AgregarClienteCommand()
        {
            var mainWindow = new Form();
            if (mainWindow.DataContext is not ClienteFormViewModel clienteFormViewModel) return;
            clienteFormViewModel.Load(null);
            mainWindow.ShowDialog();
        }

        private void OnListadoClientes()
        {
            mainFrame.Navigate(new Listado());
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

        public ICommand PrecioKilateCommand { get; set; }
        public ICommand ReportesMaestroCajaCommand { get; set; }
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

        public ICommand TiposDocumentosCommand { get; set; }
        public ICommand RealizarMovimientoCajaCommand { get; set; }
        public ICommand ListasCompraCommand { get; set; }
        public ICommand RealizarCompraCommand { get; set; }
        public ICommand TiposPreciosCommand { get; set; }

        private bool _rbnEditarCompraVisible;

        public bool RbnEditarCompraVisible
        {
            get=>_rbnEditarCompraVisible; 
            set=>SetValue(ref _rbnEditarCompraVisible, value);
        }
        public async void SetMainFrame(Frame mainFrame)
        {
            RbnEditarCompraVisible = false;
            this.mainFrame = mainFrame;
            var caja = VariablesGlobalesForm.Instance.VariablesGlobales["CAJA"];
            var agencia = VariablesGlobalesForm.Instance.VariablesGlobales["AGENCIA"];
            VariablesGlobalesForm.Instance.Parametros = await parametersRepository.RecuperarParametros();
            VariablesGlobalesForm.Instance.MaestroCaja = await maestroCajaRepository.FindByCajaAndAgencia(caja, agencia);
            this.mainFrame.Navigate(new DashboardView());
            VariablesGlobalesForm.Instance.MainFrame = mainFrame;
        }

        private void ListadoMovimientosCajas()
        {
            mainFrame.Navigate(new MovimientosCajasPage());
        }

        [Command]
        public void GoDashboard()
        {
            mainFrame.Navigate(new DashboardView());
        }
    }
}