using System.IO;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using Microsoft.Win32;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using Unity;

namespace SistemaOro.Forms.ViewModels.Agencias;

public class FormAgenciaViewModel : BaseViewModel
{
    private IAgenciaRepository _agenciaRepository;

    public FormAgenciaViewModel()
    {
        Title = "Datos Agencia";
        SaveCommand = new DelegateCommand(OnSaveCommand);
        FindImageCommand = new DelegateCommand(OnFindImageCommand);
        _agencia = new Agencia();
        _agenciaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IAgenciaRepository>();
        _imageData = [];
    }

    private async void OnFindImageCommand()
    {
        OpenFileDialog op = new OpenFileDialog();
        op.Title = "Select a picture";
        op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                    "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                    "Portable Network Graphic (*.png)|*.png";
        if (op.ShowDialog() != true) return;
        LogoImageSource = new BitmapImage(new Uri(op.FileName));
        await using FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
        _imageData = new byte[fs.Length];
        var read = fs.Read(_imageData, 0, (int)fs.Length);
    }

    private async void OnSaveCommand()
    {
        if (VariablesGlobalesForm.Instance.Usuario is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedAgencia.Nomagencia))
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, MensajesAgencias.NombreAgencia);
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedAgencia.Diragencia))
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, MensajesAgencias.DireccionAgencia);
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedAgencia.Disagencia))
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, MensajesAgencias.DistritoAgencia);
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedAgencia.Telagencia))
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, MensajesAgencias.TelefonoAgencia);
            return;
        }

        var dialog = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo, MensajesAgencias.Guardar);
        if (dialog == MessageBoxResult.No)
        {
            return;
        }

        IsLoading = true;
        if (_imageData.Length > 0)
        {
            SelectedAgencia.Logo = _imageData;
        }

        if (_isNew)
        {
            SelectedAgencia.Numcompra = 1;
            await _agenciaRepository.AddAsync(SelectedAgencia);
            VariablesGlobalesForm.Instance.AgenciasCollection.Add(SelectedAgencia);
        }
        else
        {
            await _agenciaRepository.UpdateAsync(SelectedAgencia);
        }

        if (!string.IsNullOrWhiteSpace(_agenciaRepository.ErrorSms))
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _agenciaRepository.ErrorSms);
        }
        else
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.GuardarTitulo, MensajesAgencias.DatosGuardados);
            VariablesGlobalesForm.Instance.SelectedAgencia = null;
            CloseAction?.Invoke();
        }

        IsLoading = false;
    }

    private bool _isNew;

    public ICommand SaveCommand { get; }

    public ICommand FindImageCommand { get; }

    private ImageSource? _logoImageSource;

    public ImageSource? LogoImageSource
    {
        get => _logoImageSource;
        set => SetValue(ref _logoImageSource, value);
    }

    private Agencia _agencia;

    public Agencia SelectedAgencia
    {
        get => _agencia;
        set => SetValue(ref _agencia, value);
    }

    private bool _enableCodigoAgencia;

    public bool EnableCodigoAgencia
    {
        get => _enableCodigoAgencia;
        set => SetValue(ref _enableCodigoAgencia, value);
    }

    public async void Load(Agencia? agencia)
    {
        if (agencia is null)
        {
            _isNew = true;
            agencia = new Agencia
            {
                Codagencia = await _agenciaRepository.CodigoAgencia()
            };
        }
        else
        {
            if (agencia.Logo is not null && agencia.Logo.Length > 0)
            {
                LogoImageSource = HelpersMethods.LoadImage(agencia.Logo);
            }
        }

        SelectedAgencia = agencia;

        if (VariablesGlobalesForm.Instance.Usuario is not null)
        {
            EnableCodigoAgencia = VariablesGlobalesForm.Instance.Usuario.Nivel == Nivel.Administrador;
        }

        IsLoading = false;
    }

    private byte[] _imageData;

    public async void DeleteAgencia(Agencia? agencia)
    {
        if (agencia is null)
        {
            return;
        }

        var result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.EliminarTitulo, MensajesAgencias.EliminarResultAgencia);
        if (result != MessageBoxResult.OK)
        {
            return;
        }

        if (VariablesGlobalesForm.Instance.SelectedAgencia== VariablesGlobalesForm.Instance.Agencia)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.EliminarTitulo, MensajesAgencias.Iguales);
            return;
        }
        var delete = await _agenciaRepository.DeleteAsync(agencia.Codagencia);
        
        if (delete)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.EliminarTitulo, MensajesAgencias.EliminarAgencia);
            VariablesGlobalesForm.Instance.AgenciasCollection.Remove(agencia);
            VariablesGlobalesForm.Instance.SelectedAgencia = null;
        }
        else
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _agenciaRepository.ErrorSms);
        }
    }
}