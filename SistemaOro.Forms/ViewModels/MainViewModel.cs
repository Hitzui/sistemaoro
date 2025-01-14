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
using SistemaOro.Forms.ViewModels.Usuarios;
using SistemaOro.Forms.Views.Agencias;
using SistemaOro.Forms.Views.Cajas;
using SistemaOro.Forms.Views.Compras;
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
        private Frame _mainFrame;
        private readonly IParametersRepository _parametersRepository;
        private readonly IMaestroCajaRepository _maestroCajaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public MainViewModel()
        {
            _maestroCajaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMaestroCajaRepository>();
            _parametersRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IParametersRepository>();
            _usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IUsuarioRepository>();
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
            _mainFrame = new Frame();
            VariablesGlobalesForm.Instance.MainFrame = _mainFrame;
        }
        
        [Command]
        public void RealizarDescargueCommand()
        {
            var page = new ListaDescarguesPage();
            _mainFrame.Navigate(page);
        }


        [Command]
        public void ParametrosCommand()
        {
            var param = new ParametrosPage();
            _mainFrame.Navigate(param);
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
            _mainFrame.Navigate(frmUsuarios);
        }

        [Command]
        public void EditUsuarioCommand()
        {
            var frmUsuario = new UsuarioEditWindow();
            frmUsuario.ShowDialog();
            if (frmUsuario.DataContext is not UsuarioEditViewModel model) return;
            if (!model.ResultOk) return;
            var frmUsuarios = new ListaUsuarios();
            _mainFrame.Navigate(frmUsuarios);
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

                    var delete = await _usuarioRepository.DeleteAsync(selectedUsuario);
                    if (delete)
                    {
                        HelpersMessage.MensajeInformacionResult("Eliminar", " Se ha eliminado al usuario");
                        var frmUsuarios = new ListaUsuarios();
                        _mainFrame.Navigate(frmUsuarios);
                    }
                    else
                    {
                        HelpersMessage.MensajeInformacionResult("Eliminar", $"Se produjo el siguiente error: {_usuarioRepository.ErrorSms}");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error al eliminar usuario");
            }
        }

        [Command]
        public void UsuariosListaCommand()
        {
            var frmusuarios = new ListaUsuarios();
            _mainFrame.Navigate(frmusuarios);
        }

        [Command]
        public void MonedasWindowCommand()
        {
            var frmMoneda = new MonedasWindow();
            frmMoneda.ShowDialog();
        }

        private void OnPrecioKilateCommand()
        {
            _mainFrame.Navigate(new PreciosKilatePage());
        }

        private void OnListasCompraCommand()
        {
            _mainFrame.Navigate(new ComprasPage());
        }

        private void OnTiposPreciosCommand()
        {
            var frm = new TiposPreciosWindow();
            frm.ShowDialog();
        }

        private void OnRealizarCompraCommand()
        {
            _mainFrame.Navigate(new FormComprasPage());
        }

        private void OnReportesMaestroCajaCommand()
        {
            _mainFrame.Navigate(new ReportesMaestroCajaPage());
        }

        private void OnRealizarMovimientoCajaCommand()
        {
            var frm = new RealizarMovimientoCajaPage();
            frm.ShowDialog();
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
                _logger.Error(e, "Error al eliminar el cliente");
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

        public async void SetMainFrame(Frame mainFrame)
        {
            _mainFrame = mainFrame;
            var caja = VariablesGlobalesForm.Instance.VariablesGlobales["CAJA"];
            var agencia = VariablesGlobalesForm.Instance.VariablesGlobales["AGENCIA"];
            VariablesGlobalesForm.Instance.Parametros = await _parametersRepository.RecuperarParametros();
            VariablesGlobalesForm.Instance.MaestroCaja = await _maestroCajaRepository.FindByCajaAndAgencia(caja, agencia);
        }

        private void ListadoMovimientosCajas()
        {
            _mainFrame.Navigate(new MovimientosCajasPage());
        }
    }
}