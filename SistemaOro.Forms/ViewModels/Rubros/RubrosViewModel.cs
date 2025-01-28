using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using Unity;

namespace SistemaOro.Forms.ViewModels.Rubros;

public class RubrosViewModel : BaseViewModel
{
    private IRubroRepository _rubroRepository;

    public RubrosViewModel()
    {
        _rubroRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IRubroRepository>();
        Title = "Rubros";
        _itemsSource = new List<Rubro>();
    }

    IList<Rubro> _itemsSource;

    public IList<Rubro> ItemsSource
    {
        set => SetValue(ref _itemsSource, value);
        get => _itemsSource;
    }

    [Command]
    public async void ValidateRow(RowValidationArgs args)
    {
        var item = (Rubro)args.Item;
        var save = false;
        if (args.IsNewItem)
        {
            save = await _rubroRepository.AddAsync(item);
        }
        else
        {
            save = await _rubroRepository.UpdateAsync(item);
        }

        if (!save)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _rubroRepository.ErrorSms);
        }
    }

    [Command]
    public async void ValidateRowDeletion(ValidateRowDeletionArgs args)
    {
        var result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.EliminarTitulo, MensajesRubros.Eliminar);
        if (result == MessageBoxResult.Cancel)
        {
            return;
        }

        var item = (Rubro)args.Items.Single();
        var delete = await _rubroRepository.DeleteAsync(item);
        if (!delete)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _rubroRepository.ErrorSms);
        }
    }

    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        Load();
    }

    public async void Load()
    {
        ItemsSource = await _rubroRepository.FindAll();
    }
}