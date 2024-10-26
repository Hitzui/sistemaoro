using System;
using System.Text.RegularExpressions;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Utils.MVVM.Services;
using DevExpress.Xpf.Core.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Models;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using Unity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SistemaOro.Forms.ViewModels.Usuarios;

public class UsuarioEditViewModel : BaseViewModel
{
    private int _auxiliar = 1;
    private readonly IUsuarioRepository _usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IUsuarioRepository>();

    public UsuarioEditViewModel()
    {
        Title = "Datos Usuario";
        EstadoUsuarios = Enum.GetValues<EstadoUsuario>();
        Niveles = Enum.GetValues<Nivel>();
    }

    public DtoUsuario SelectedUsuario
    {
        get => GetValue<DtoUsuario>();
        private set => SetValue(value);
    }

    [Command]
    public async void SaveCommand()
    {
        var result = HelpersMessage.MensajeConfirmacionResult("Guardar", "¿Guardar datos de usuario?");

        if (result == MessageBoxResult.Cancel)
        {
            return;
        }
        
        if (string.IsNullOrEmpty(SelectedUsuario.Codoperador))
        {
            return;
        }

        if (string.IsNullOrEmpty(SelectedUsuario.Username) || Regex.IsMatch(SelectedUsuario.Username, @"\s"))
        {
            MessageBox.Show("El nombre de usuario no debe contener espacios o estar vacío.");
            return;
        }

        if (string.IsNullOrEmpty(SelectedUsuario.Clave) || Regex.IsMatch(SelectedUsuario.Clave, @"\s"))
        {
            MessageBox.Show("La clave de usuario no debe contener espacios o estar vacío.");
            return;  
        }

        if (string.IsNullOrEmpty(SelectedUsuario.Pregunta))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedUsuario.Respuesta))
        {
            return;
        }

        var usuario = SelectedUsuario.GetUsuario();
        switch (_auxiliar)
        {
            case 1:
                usuario.Fcreau = DateTime.Now;
                var save = await _usuarioRepository.AddAsync(usuario);
                if (save)
                {
                    XtraMessageBox.Show("Se han guardado los datos de forma correcta", "Guardar");
                    CloseAction?.Invoke();
                }
                else
                {
                    XtraMessageBox.Show($"Se produjo el siguiente error: {_usuarioRepository.ErrorSms}", "Error");
                }

                break;
            case 2:
                var edit = await _usuarioRepository.UpdateAsync(usuario);
                if (edit)
                {
                    XtraMessageBox.Show("Se han guardado los datos de forma correcta", "Guardar");
                    CloseAction?.Invoke();
                }
                else
                {
                    XtraMessageBox.Show($"Se produjo el siguiente error: {_usuarioRepository.ErrorSms}", "Error");
                }
                break;
            default:
                XtraMessageBox.Show("Se produjo el siguiente error: no method default in save", "Error");
                break;
        }
    }

    public void Load()
    {
        if (VariablesGlobalesForm.Instance.SelectedUsuario == null)
        {
            SelectedUsuario = new DtoUsuario
            {
                Nivel = Nivel.Caja,
                Estado = EstadoUsuario.Activo
            };
            _auxiliar = 1; //nuevo usuario
        }
        else
        {
            SelectedUsuario = new DtoUsuario().GetDtoUsuario(VariablesGlobalesForm.Instance.SelectedUsuario) ?? new DtoUsuario();
            _auxiliar = 2; //editar usuario
        }
    }

    public Nivel[] Niveles { get; set; }

    public EstadoUsuario[] EstadoUsuarios { get; set; }
}