using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services.Helpers;
using Unity;

namespace SistemaOro.Forms.ViewModels.Precios;

public class PrecioKilateViewModel : BaseViewModel
{
    private readonly IPreciosKilatesRepository _preciosKilatesRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IPreciosKilatesRepository>();

    public PrecioKilateViewModel()
    {
        Title = "Precio Kilate";
    }

    IList<PrecioKilate>? _itemsSource;

    public IList<PrecioKilate> ItemsSource
    {
        get
        {
            if (_itemsSource == null && !IsInDesignMode)
            {
                DataSourceRefresh(new DataSourceRefreshArgs());
            }

            return _itemsSource!;
        }
    }

    [Command]
    public async void ValidateRow(RowValidationArgs args)
    {
        var item = (PrecioKilate)args.Item;
        if (item == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(item.DescKilate))
        {
            args.Result = new ValidationErrorInfo("Descripcion no puede estar vacia", ValidationErrorType.Critical);
            return;
        }

        if (decimal.Compare(item.KilatePeso, decimal.Zero) <= 0)
        {
            args.Result = new ValidationErrorInfo("El peso debe ser mayor a cero", ValidationErrorType.Critical);
            return;
        }

        if (decimal.Compare(item.PrecioKilate1, decimal.Zero) <= 0)
        {
            args.Result = new ValidationErrorInfo("El precio debe ser mayor a cero", ValidationErrorType.Critical);
            return;
        }

        if (args.IsNewItem)
        {
            await _preciosKilatesRepository.AddAsync(item);
        }
    }

    [Command]
    public async void ValidateRowDeletion(ValidateRowDeletionArgs args)
    {
        var result = HelpersMessage.MensajeConfirmacionResult("Eliminar", "¿Seguro quiere eliminar el precio seleccionado?");
        if (result is MessageBoxResult.Yes or MessageBoxResult.OK)
        {
            var item = (PrecioKilate)args.Items.Single();
            await _preciosKilatesRepository.DeleteAsync(item.DescKilate);
        }

        DataSourceRefresh(new DataSourceRefreshArgs());
    }

    [Command]
    public async void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        _itemsSource = await _preciosKilatesRepository.FindAll();
        RaisePropertyChanged(nameof(ItemsSource));
    }
}