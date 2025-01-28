using System;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm.Xpf;
using DevExpress.Utils.Extensions;
using Microsoft.Win32;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using Unity;
using NLog;

namespace SistemaOro.Forms.ViewModels.TipoCambio;

public class TipoCambioViewModel : BaseViewModel
{
    private ITipoCambioRepository _tipoCambioRepository;
    private readonly IPreciosKilatesRepository _preciosKilatesRepository;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public TipoCambioViewModel()
    {
        Title = "Tipo de Cambio";
        _tipoCambioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ITipoCambioRepository>();
        _itemSource = new List<Data.Entities.TipoCambio>();
        SaveCommand = new DelegateCommand(OnSaveCommand);
        DeleteCommand = new DelegateCommand(OnDeleteCommand);
        OpenFileCommand = new DelegateCommand(OnOpenFileCommand);
        _preciosKilatesRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IPreciosKilatesRepository>();
    }

    private async void OnDeleteCommand()
    {
        try
        {
            if (SelectedValue is null)
            {
                return;
            }

            var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.EliminarTitulo, TipoCambioMensajes.Eliminar);
            if (message == MessageBoxResult.Cancel)
            {
                return;
            }

            var delete = await _tipoCambioRepository.DeleteAsync(SelectedValue);
            if (!delete)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _tipoCambioRepository.ErrorSms);
            }

            OnLoad();
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error al eliminar el tipo de cambio");
        }
    }

    private void OnOpenFileCommand()
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
        if (openFileDialog.ShowDialog() == true)
        {
            var filePath = openFileDialog.FileName;
            var readExcel = new ReadExcelTipoCambio();
            var dataTable = readExcel.ReadExcelFile(filePath);
            ItemsSource = dataTable;
        }
    }

    private async void OnSaveCommand()
    {
        try
        {
            var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo, TipoCambioMensajes.Guardar);
            if (message == MessageBoxResult.Cancel)
            {
                return;
            }

            if (Fecha is null)
            {
                return;
            }
        
            if (decimal.Compare(decimal.Zero, TipoCambioAmount) == 0)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.InformacionTitulo, TipoCambioMensajes.TipoCambioZero);
                return;
            }

            var tipoCambio = new Data.Entities.TipoCambio
            {
                Fecha = Fecha.Value,
                Tipocambio = TipoCambioAmount,
                Hora = Fecha.Value.ToString("h:mm:ss tt"),
                PrecioOro = ValorOroAmount
            };
            var findAny = ItemsSource.Any(cambio => cambio.Fecha.Date==tipoCambio.Fecha.Date);
            if (findAny)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, TipoCambioMensajes.TipoCambioExiste);
                return;
            }
            var save = await _tipoCambioRepository.AddAsync(tipoCambio);
            if (!save)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _tipoCambioRepository.ErrorSms);
            }
            else
            {
                OnLoad();
                if (tipoCambio.Fecha.Date == DateTime.Now.Date)
                {
                    await _preciosKilatesRepository.ActualizarPreciosGuardados(tipoCambio);
                }
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error al guardar el tipo de cambio");
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand OpenFileCommand { get; }

    private decimal _valorOroAmount = decimal.Zero;

    public decimal ValorOroAmount
    {
        get => _valorOroAmount;
        set => SetValue(ref _valorOroAmount, value);
    }

    private decimal _tipoCambioAmount = decimal.Zero;

    public decimal TipoCambioAmount
    {
        get => _tipoCambioAmount;
        set => SetProperty(ref _tipoCambioAmount, value, nameof(TipoCambioAmount));
    }

    private DateTime? _fecha = DateTime.Now;

    public DateTime? Fecha
    {
        get => _fecha;
        set
        {
            SetValue(ref _fecha, value);
            OnLoad();
        }
    }

    private Data.Entities.TipoCambio? _selectedValue;

    public Data.Entities.TipoCambio? SelectedValue
    {
        get => _selectedValue;
        set => SetValue(ref _selectedValue, value);
    }

    private IList<Data.Entities.TipoCambio> _itemSource;

    public IList<Data.Entities.TipoCambio> ItemsSource
    {
        get => _itemSource;
        set => SetProperty(ref _itemSource, value, nameof(ItemsSource));
    }

    [Command]
    public async void ValidateRowCommand(RowValidationArgs args)
    {
        try
        {
            var item = (Data.Entities.TipoCambio)args.Item;
            if (args.IsNewItem) return;
            var update = await _tipoCambioRepository.UpdateAsync(item);
            if (!update)
            {
                HelpersMessage.DialogWindow(MensajesGenericos.ErrorTitulo, _tipoCambioRepository.ErrorSms, MessageBoxButton.OK);
            }
            else
            {
                if (item.Fecha.Date == DateTime.Now.Date)
                {
                    await _preciosKilatesRepository.ActualizarPreciosGuardados(item);
                }
                HelpersMessage.DialogWindow(MensajesGenericos.GuardarTitulo, TipoCambioMensajes.ActualizarTipoCambio, MessageBoxButton.OK);
            }

            OnLoad();
        }
        catch (Exception e)
        {
            _logger.Error(e, "Se produjo un error al validar el tipo de cambio");
        }
    }

    [Command]
    public async void ValidateRowDeletionCommand(ValidateRowDeletionArgs args)
    {
        var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.EliminarTitulo, TipoCambioMensajes.Eliminar);
        if (message == MessageBoxResult.Cancel)
        {
            OnLoad();
            return;
        }

        var item = (Data.Entities.TipoCambio)args.Items.Single();
        if (item is null)
        {
            return;
        }

        var delete = await _tipoCambioRepository.DeleteAsync(item.Fecha);
        if (!delete)
        {
            HelpersMessage.DialogWindow(MensajesGenericos.ErrorTitulo, _tipoCambioRepository.ErrorSms, MessageBoxButton.OK);
        }

        OnLoad();
    }

    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        OnLoad();
    }

    [Command]
    public async void ActualizarPreciosCommand()
    {
        var tipoCambio = ItemsSource.SingleOrDefault(cambio => cambio.Fecha.Date == DateTime.Now.Date);
        if (tipoCambio is not null)
        {
            await _preciosKilatesRepository.ActualizarPreciosGuardados(tipoCambio); 
            HelpersMessage.DialogWindow(MensajesGenericos.GuardarTitulo, "Se han actualizado los precios de forma correcta", MessageBoxButton.OK);
        }
    }

    public async void OnLoad()
    {
        try
        {
            IsLoading = true;
            ItemsSource = await _tipoCambioRepository.FindAllByMonth(Fecha!.Value);
            IsLoading = false;
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error al cargar los tipos de cambios");
        }
    }
}