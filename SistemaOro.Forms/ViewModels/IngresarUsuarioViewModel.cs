using System;
using System.Threading.Tasks;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.XtraEditors;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using Unity;

namespace SistemaOro.Forms.ViewModels;

public class IngresarUsuarioViewModel : BaseViewModel
{

    public IngresarUsuarioViewModel()
    {
        Title = "Ingresar Usuario";
    }

    public string Username
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string Clave
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public Action<bool?> CerrarDialogo { get; set; }

    [Command]
    public async void IngresarCommand()
    {
        var usuarioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IUsuarioRepository>();
        if (string.IsNullOrWhiteSpace(Username))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Clave))
        {
            return ;
        }

        var findUsuario = await usuarioRepository.FindByUsuario(Username);
        if (findUsuario != null)
        {
            if (findUsuario.Clave==Clave)
            {
                if (findUsuario.Nivel==Nivel.Administrador)
                {
                    VariablesGlobalesForm.Instance.PermitirEdicionCompra = true;
                    CerrarDialogo(true);
                    return;
                }
                XtraMessageBox.Show("Usuario no posee permisos de edición");
                VariablesGlobalesForm.Instance.PermitirEdicionCompra = false;
                return;
            }

            XtraMessageBox.Show("Clave incorrecta");
            return;
        }

        XtraMessageBox.Show("Nombre de usuario no existe en sistema");
    }
}