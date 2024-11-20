using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using System.Windows.Input;
using DevExpress.Xpf.WindowsUI;
using NLog;
using SistemaOro.Forms.Models;
using SistemaOro.Forms.Services.Mensajes;
using Unity;
using SistemaOro.Forms.Services.Helpers;

namespace SistemaOro.Forms.ViewModels.Clientes;

public partial class ClienteFormViewModel : BaseViewModel
{
    private IClienteRepository _clienteRepository;
    private ITipoDocumentoRepository _tipoDocumentoRepository;
    private bool _isNew;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private Cliente? _cliente;

    public ClienteFormViewModel()
    {
        Title = "Detalle de Cliente";
        _clienteRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IClienteRepository>();
        _tipoDocumentoRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ITipoDocumentoRepository>();
        SaveCommand = new DelegateCommand(Save);
    }

    private ObservableCollection<Data.Entities.TipoDocumento> _tipoDocumentos = new();

    public ObservableCollection<Data.Entities.TipoDocumento> TipoDocumentos
    {
        get => _tipoDocumentos;
        set => SetProperty(ref _tipoDocumentos, value, nameof(TipoDocumentos));
    }

    private Data.Entities.TipoDocumento? _tipoDocumento;

    public Data.Entities.TipoDocumento? SelectedTipoDocumento
    {
        get => _tipoDocumento;
        set => SetValue(ref _tipoDocumento, value);
    }

    private DtoCliente? _selectedCliente;

    public DtoCliente? SelectedCliente
    {
        get => _selectedCliente;
        private set => SetValue(ref _selectedCliente, value);
    }

    private bool _isSiOtraAe;

    public bool IsSiOtraAe
    {
        get => _isSiOtraAe;
        set => SetValue(ref _isSiOtraAe, value);
    }

    private bool _isNoOtraAe;

    public bool IsNoOtraAe
    {
        get => _isNoOtraAe;
        set => SetValue(ref _isNoOtraAe, value);
    }

    private bool _isCuentaPropia;
    private bool _isTercero;

    public bool IsCuentaPropia
    {
        get => _isCuentaPropia;
        set => SetProperty(ref _isCuentaPropia, value, "IsCuentaPropia");
    }

    public bool IsTercero
    {
        get => _isTercero;
        set => SetProperty(ref _isTercero, value, "IsTercero");
    }

    public async void Load(Cliente? cliente)
    {
        try
        {
            var tiposDocumentosFindAll = await _tipoDocumentoRepository.FindAll();
            if (tiposDocumentosFindAll.Count > 0)
            {
                TipoDocumentos.Clear();
                foreach (var tipoDocumento in tiposDocumentosFindAll)
                {
                    TipoDocumentos.Add(tipoDocumento);
                }
            }
            else
            {
                HelpersMessage.MensajeErroResult("Error", $"No hay tipos de documentos registrados en sistemas {_tipoDocumentoRepository.ErrorSms}");
                IsLoading = false;
                return;
            }

            if (cliente is not null)
            {
                _cliente = await _clienteRepository.GetByIdAsync(cliente.Codcliente);
                if (_cliente is null)
                {
                    HelpersMessage.MensajeErroResult("Error", $"Se produjo el siguiente error: {_clienteRepository.ErrorSms}");
                    IsLoading = false;
                    return;
                }

                SelectedCliente = new DtoCliente();
                SelectedCliente = SelectedCliente.GetDtoCliente(_cliente);
                SelectedTipoDocumento = tiposDocumentosFindAll.Find(documento => documento.Idtipodocumento == _cliente.Idtipodocumento);
                _isNew = false;
                NumeroCliente = _cliente.Codcliente;
                if (!string.IsNullOrWhiteSpace(SelectedCliente.OtraAe))
                {
                    IsNoOtraAe = SelectedCliente.OtraAe.Equals("no", StringComparison.InvariantCultureIgnoreCase);
                    IsSiOtraAe = !IsNoOtraAe;
                }

                if (!string.IsNullOrWhiteSpace(SelectedCliente.ActuaPor))
                {
                    IsCuentaPropia = SelectedCliente.ActuaPor.Equals("Cuenta propia", StringComparison.InvariantCultureIgnoreCase);
                    IsTercero = !IsCuentaPropia;
                }
            }
            else
            {
                NumeroCliente = await _clienteRepository.CodCliente();
                SelectedCliente = new DtoCliente()
                {
                    Codcliente = NumeroCliente,
                    FNacimiento = DateTime.Now,
                    FIngreso = DateTime.Now,
                    FEmision = DateTime.Now,
                    FVencimiento = DateTime.Now,
                    Telefono = string.Empty
                };
                SelectedTipoDocumento = tiposDocumentosFindAll.ElementAt(0);
                IsNoOtraAe = true;
                _isNew = true;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Ha ocurrido un error");
            HelpersMessage.MensajeErroResult("Error", ex.Message);
        }

        IsLoading = false;
    }

    private string? _numeroCliente;

    public string? NumeroCliente
    {
        get => _numeroCliente;
        private set => SetValue(ref _numeroCliente, value);
    }

    public ICommand SaveCommand { get; }

    private async void Save()
    {
        if (string.IsNullOrWhiteSpace(_numeroCliente))
        {
            return;
        }

        var dialog = new WinUIDialogWindow(MensajesGenericos.GuardarTitulo, MessageBoxButton.YesNo)
        {
            Content = new TextBlock { Text = ClienteMessages.SaveClienteContent }
        };
        var showDialog = dialog.ShowDialog();
        if (!showDialog!.Value)
        {
            return;
        }

        var winuidialog = new WinUIDialogWindow(MensajesGenericos.InformacionTitulo, MessageBoxButton.OK);
        if (SelectedCliente is null)
        {
            winuidialog.Content = new TextBlock { Text = ClienteMessages.SeleccionarCliente };
            winuidialog.ShowDialog();
            return;
        }

        if (Validate())
        {
            winuidialog.Content = new TextBlock { Text = ClienteMessages.CamposVacios };
            winuidialog.ShowDialog();
            return;
        }

        IsLoading = true;
        if (SelectedTipoDocumento is null)
        {
            winuidialog.Title = MensajesGenericos.ErrorTitulo;
            winuidialog.Content = new TextBlock { Text = ClienteMessages.SeleccionarTipoDocumento };
            IsLoading = false;
            winuidialog.ShowDialog();
            return;
        }

        _cliente = SelectedCliente.GetCliente();
        _cliente.Idtipodocumento = SelectedTipoDocumento.Idtipodocumento;
        bool save;
        if (!_isNew)
        {
            save = await _clienteRepository.UpdateAsync(_cliente);
        }
        else
        {
            save = await _clienteRepository.Create(_cliente);
        }

        if (save)
        {
            winuidialog.Content = new TextBlock { Text = ClienteMessages.DatosGuardadosSuccess };
        }
        else
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, $"Se produjo el siguiente error: {_clienteRepository.ErrorSms}");
        }

        IsLoading = false;
        winuidialog.ShowDialog();
    }

    private bool Validate()
    {
        return string.IsNullOrWhiteSpace(SelectedCliente!.Nombres)
               || string.IsNullOrWhiteSpace(SelectedCliente.Apellidos)
               || string.IsNullOrWhiteSpace(SelectedCliente.Numcedula)
               || string.IsNullOrWhiteSpace(SelectedCliente.Direccion)
               || string.IsNullOrWhiteSpace(SelectedCliente.Email)
               || string.IsNullOrWhiteSpace(SelectedCliente.Celular);
    }
}