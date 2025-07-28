using System;
using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;
using DevExpress.CodeParser;
using SistemaOro.Forms.Services.Helpers;

namespace SistemaOro.Forms.ViewModels.Monedas;

public class MonedasViewModel : BaseViewModel
{
    private readonly IMonedaRepository _monedaRepository;
    private bool _validate;

    public MonedasViewModel()
    {
        Title = "Monedas";
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _monedaRepository = unitOfWork.MonedaRepository;
        _itemsSource = new ObservableCollection<Moneda>();
    }

    ObservableCollection<Moneda> _itemsSource;

    public ObservableCollection<Moneda> ItemsSource => _itemsSource;

    [Command]
    public void ValidateCell(RowCellValidationArgs args) {
        if (args.FieldName=="Descripcion")
        {
            if (args.NewValue is null)
            {
                return;
            }
            var nuevaDescripcion = args.NewValue.ToString();
            if (args.OldValue is null)
            {
                return;
            }
            var oldDescripcion = args.OldValue.ToString();
            if (string.Equals(nuevaDescripcion, oldDescripcion, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }
            _validate = ItemsSource.Where(moneda => moneda.Codmoneda > 0)
                .Any(moneda => string.Equals(moneda.Descripcion, nuevaDescripcion, StringComparison.CurrentCultureIgnoreCase));
            if (_validate)
            {
                args.Result=new ValidationErrorInfo("Ya existe la Moneda con la descripción indicada", ValidationErrorType.Critical);
            }
        }
    }

    [Command]
    public async void ValidateRow(RowValidationArgs args)
    {
        var item = (Moneda)args.Item;
        if (item is null) return;
        if (!_validate)
        {
            if (args.IsNewItem)
            {
                item.Fecha = DateTime.Now;
            }

            var save = await _monedaRepository.Save(item);
            if (save > 0)
            {
                HelpersMessage.MensajeInformacionResult("Guardar", "Se ha guardado con éxito");
            }
            else
            {
                HelpersMessage.MensajeErroResult("Guardar", _monedaRepository.ErrorSms);
            }
        }

        Load();
    }

    [Command]
    public void ValidateRowDeletion(ValidateRowDeletionArgs args)
    {
        var result = HelpersMessage.MensajeConfirmacionResult("Eliminar", "¿Seguro quiere elminar la moneda seleccionada? Esta acción no se puede revertir");
        if (result == MessageBoxResult.Cancel)
        {
            return;
        }

        var item = (Moneda)args.Items.Single();
        _monedaRepository.DeleteAsync(item);
        Load();
    }

    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        Load();
    }

    public async void Load()
    {
        _itemsSource.Clear();
        var findAll = await _monedaRepository.FindAll();
        _itemsSource = new ObservableCollection<Moneda>(findAll);
        RaisePropertyChanged(nameof(ItemsSource));
    }
}