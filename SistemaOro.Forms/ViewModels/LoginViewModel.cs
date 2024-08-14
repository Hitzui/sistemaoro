using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Microsoft.Win32;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using Unity;

namespace SistemaOro.Forms.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private IUsuarioRepository _usuarioRepository;
    private IAgenciaRepository _agenciaRepository;
    private IParametersRepository _parametersRepository;

    public LoginViewModel()
    {
        _usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IUsuarioRepository>();
        ExitCommand = new DelegateCommand(OnExitCommand);
        FindImageCommand = new DelegateCommand(OnFindImageCommand);
        _password = string.Empty;
        _username = string.Empty;
        _agenciaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IAgenciaRepository>();
        _parametersRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IParametersRepository>();
        _agencias = new List<Agencia>();
    }

    private async void OnFindImageCommand()
    {
        OpenFileDialog op = new OpenFileDialog();
        op.Title = "Select a picture";
        op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                    "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                    "Portable Network Graphic (*.png)|*.png";
        if (op.ShowDialog() == true)
        {
            var param = await _parametersRepository.RecuperarParametros();
            LogoImageSource = new BitmapImage(new Uri(op.FileName));
            await using FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
            var imageData = new byte[fs.Length];
            var result = fs.Read(imageData, 0, (int)fs.Length);
            if (param is not null && result>0)
            {
                param.Logo = imageData;
                await _parametersRepository.ActualizarParametros(param);
            }
        }
    }

    private void OnExitCommand()
    {
        Application.Current.Shutdown();
    }

    public async Task<bool> OnLoginCommand()
    {
        if (!Validate())
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.InformacionTitulo, UsuarioMensajes.EspecificarUsernamePassword);
            return false;
        }

        if (SelectedAgencia is null)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.InformacionTitulo, UsuarioMensajes.SeleccionarAgencia);
            return false;
        }

        var user = await _usuarioRepository.FindByUsuario(Username);
        if (user is null)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.InformacionTitulo, UsuarioMensajes.UsernameIncorrecto);
            return false;
        }

        var validatePassword = user.Clave == Password;
        if (validatePassword)
        {
            VariablesGlobalesForm.Instance.Usuario = user;
            VariablesGlobalesForm.Instance.Agencia = SelectedAgencia;
            return true;
        }

        HelpersMessage.MensajeInformacionResult(MensajesGenericos.InformacionTitulo, UsuarioMensajes.ClaveIncorrecto);
        return false;
    }

    private string _username;

    public string Username
    {
        get => _username;
        set
        {
            SetValue(ref _username, value);
            Validate();
            RaisePropertyChanged(nameof(Enable));
        }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            SetValue(ref _password, value);
            Validate();
            RaisePropertyChanged(nameof(Enable));
        }
    }

    private bool Validate()
    {
        Enable = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        return Enable;
    }

    private bool _enable;

    public bool Enable
    {
        get => _enable;
        set => SetValue(ref _enable, value);
    }

    private Agencia? _selectedAgencia;

    public Agencia? SelectedAgencia
    {
        get => _selectedAgencia;
        set
        {
            SetValue(ref _selectedAgencia, value);
            if (value is not null && value.Logo is not null)
            {
                //LogoImageSource = HelpersMethods.LoadImage(Convert.FromBase64String(value.Logo));
            }
        }
    }

    public ICommand ExitCommand { get; }
    public ICommand FindImageCommand { get; }

    private IList<Agencia> _agencias;

    public IList<Agencia> Agencias
    {
        get => _agencias;
        set => SetValue(ref _agencias, value);
    }

    private ImageSource _logoImageSource;

    public ImageSource LogoImageSource
    {
        get => _logoImageSource;
        set => SetValue(ref _logoImageSource, value);
    }

    public async void OnLoad()
    {
        Agencias = await _agenciaRepository.FindAll();
        var param = await _parametersRepository.RecuperarParametros();
        if (param is not null && param.Logo is not null)
        {
            LogoImageSource = HelpersMethods.LoadImage(param.Logo);
        }
    }
}