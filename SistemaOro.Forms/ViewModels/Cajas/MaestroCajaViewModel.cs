using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using Unity;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class MaestroCajaViewModel : BaseViewModel
{
    private IMaestroCajaRepository _maestroCajaRepository;
    private string? _caja;
    private string? _agencia;
    public MaestroCajaViewModel()
    {
        _maestroCajaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMaestroCajaRepository>();
        _caja = VariablesGlobalesForm.Instance.VariablesGlobales["CAJA"];
        _agencia =VariablesGlobalesForm.Instance.VariablesGlobales["AGENCIA"];
        Title = "Maestro de caja";
        CerrarCajaCommand = new DelegateCommand(OnCerrarCajaCommand);
        AperturarCajaCommand = new AsyncCommand(OnAperturarCajaCommand);
    }

    private Task OnAperturarCajaCommand()
    {
        var result = HelpersMessage.MensajeConfirmacionResult(MensajesMaestroCaja.AperturarCajaTitulo, MensajesMaestroCaja.AperturarCajaQ);
        if (result == MessageBoxResult.Cancel)
        {
            return Task.CompletedTask;
        }
        return Task.Run(async () =>
        {
            var open=await _maestroCajaRepository.AbrirCaja(_caja, _agencia);
            if (!open)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _maestroCajaRepository.ErrorSms);
            }
            else
            {
                HelpersMessage.MensajeInformacionResult(MensajesMaestroCaja.AperturarCajaTitulo, MensajesMaestroCaja.AperturarCaja);
                Load();
            }
        });
    }

    private async void OnCerrarCajaCommand()
    {
        var result = HelpersMessage.MensajeConfirmacionResult(MensajesMaestroCaja.CerrarCajaTitulo, MensajesMaestroCaja.CerrarCajaQ);
        if (result == MessageBoxResult.Cancel)
        {
            return;
        }

        var close = await _maestroCajaRepository.CerrarCaja(_caja, _agencia);
        if (!close)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _maestroCajaRepository.ErrorSms);
        }
        else
        {
            HelpersMessage.MensajeInformacionResult(MensajesMaestroCaja.CerrarCajaTitulo, MensajesMaestroCaja.CerrarrCaja);
            Load();
        }
    }

    public bool IsClose
    {
        get => GetValue<bool>();
        set=>SetValue(value);
    }
    public bool IsOpen
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }
    public Mcaja? MaestroCaja
    {
        get =>GetValue<Mcaja>();
        set => SetValue(value);
    }

    public async void Load()
    {
        VariablesGlobalesForm.Instance.MaestroCaja = await _maestroCajaRepository.FindByCajaAndAgencia(_caja, _agencia);
        MaestroCaja=VariablesGlobalesForm.Instance.MaestroCaja;
        if (MaestroCaja is null)
        {
            IsOpen = true;
            IsClose = false;
        }
        else
        {
            IsClose = true;
            IsOpen = false;
        }
    }

    public ICommand CerrarCajaCommand { get; }

    public ICommand AperturarCajaCommand { get; }
}