using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using System.Windows.Input;
using DevExpress.Xpf.WindowsUI;
using SistemaOro.Forms.Services.Mensajes;
using Unity;
using SistemaOro.Forms.Models;

namespace SistemaOro.Forms.ViewModels.Clientes;

public class ClienteFormViewModel : BaseViewModel
{
    private IClienteRepository _clienteRepository;
    private ITipoDocumentoRepository _tipoDocumentoRepository;
    private bool _isNew;

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


    public DtoCliente? SelectedCliente
    {
        get => GetValue<DtoCliente>();
        set => SetValue(value);
    }

    public async void Load(Cliente? cliente)
    {
        SelectedCliente = new DtoCliente();
        if (cliente is not null)
        {
            SelectedCliente = SelectedCliente.GetDtoCliente(cliente);
        }

        if (string.IsNullOrWhiteSpace(SelectedCliente!.Codcliente))
        {
            NumeroCliente = await _clienteRepository.CodCliente();
            SelectedCliente.Codcliente = NumeroCliente;
            _isNew = true;
        }
        else
        {
            _isNew = false;
            NumeroCliente = SelectedCliente.Codcliente;
            SelectedCliente.TipoDocumento = await _tipoDocumentoRepository.GetByIdAsync(cliente!.Idtipodocumento!);
        }

        TipoDocumentos.Clear();
        var tiposDocumentosFindAll = await _tipoDocumentoRepository.FindAll();
        foreach (var tipoDocumento in tiposDocumentosFindAll)
        {
            TipoDocumentos.Add(tipoDocumento);
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
        if (SelectedCliente.TipoDocumento is null)
        {
            winuidialog.Title = MensajesGenericos.ErrorTitulo;
            winuidialog.Content = new TextBlock { Text = ClienteMessages.SeleccionarTipoDocumento };
            IsLoading = false;
            winuidialog.ShowDialog();
            return;
        }

        var cliente = SelectedCliente.GetCliente();
        cliente.Idtipodocumento = SelectedCliente.TipoDocumento.Idtipodocumento;
        bool save;
        if (!_isNew)
        {
            save = await _clienteRepository.Update(cliente);
        }
        else
        {
            save = await _clienteRepository.Create(cliente);
        }

        if (save)
        {
            winuidialog.Content = new TextBlock { Text = ClienteMessages.DatosGuardadosSuccess };
        }
        else
        {
            winuidialog.Title = MensajesGenericos.ErrorTitulo;
            winuidialog.Content = new TextBlock { Text = $"{ClienteMessages.ClienteGuardarError}\n{_clienteRepository.ErrorSms}" };
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