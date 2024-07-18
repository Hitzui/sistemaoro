using System;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm.Xpf;
using Microsoft.Win32;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using Unity;
using DevExpress.Xpf.Core.Commands;

namespace SistemaOro.Forms.ViewModels.TipoCambio;

public class TipoCambioViewModel : BaseViewModel
{
    private ITipoCambioRepository _tipoCambioRepository;

    public TipoCambioViewModel()
    {
        Title = "Tipo de Cambio";
        _tipoCambioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ITipoCambioRepository>();
        _itemSource = new List<Data.Entities.TipoCambio>();
        SaveCommand = new DelegateCommand(OnSaveCommand);
        DeleteCommand = new DelegateCommand(OnDeleteCommand);
        OpenFileCommand = new DelegateCommand(OnOpenFileCommand);
    }

    private async void OnDeleteCommand()
    {
        if (SelectedValue is null)
        {
            return;
        }

        var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.EliminarTitulo, TipoCambioMensajes.Eliminar, MessageBoxImage.Question);
        if (message == MessageBoxResult.Cancel)
        {
            return;
        }

        var delete = await _tipoCambioRepository.DeleteAsync(SelectedValue.Fecha);
        if (!delete)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _tipoCambioRepository.ErrorSms);
        }

        OnLoad();
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
        var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo, TipoCambioMensajes.Guardar, MessageBoxImage.Question);
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
        var save = await _tipoCambioRepository.AddAsync(tipoCambio);
        if (!save)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _tipoCambioRepository.ErrorSms);
        }
        else
        {
            OnLoad();
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
        var item = (Data.Entities.TipoCambio)args.Item;
        if (!args.IsNewItem)
        {
            var update = await _tipoCambioRepository.UpdateAsync(item);
            if (!update)
            {
                HelpersMessage.DialogWindow(MensajesGenericos.ErrorTitulo, _tipoCambioRepository.ErrorSms, MessageBoxButton.OK);
            }

            OnLoad();
        }
    }

    [Command]
    public async void ValidateRowDeletionCommand(ValidateRowDeletionArgs args)
    {
        var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.EliminarTitulo, TipoCambioMensajes.Eliminar, MessageBoxImage.Question);
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

    public async void OnLoad()
    {
        IsLoading = true;
        ItemsSource = await _tipoCambioRepository.FindAllByMonth(Fecha!.Value);
        IsLoading = false;
    }
}