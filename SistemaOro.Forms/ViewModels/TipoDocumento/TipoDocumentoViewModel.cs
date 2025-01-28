using System;
using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Mvvm.Xpf;
using NLog;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;

namespace SistemaOro.Forms.ViewModels.TipoDocumento;

public class TipoDocumentoViewModel : BaseViewModel
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public TipoDocumentoViewModel()
    {
        Title = "Lista de tipos de documentos";
        _context = new DataContext();
    }

    readonly DataContext _context;

    IList<Data.Entities.TipoDocumento>? _itemsSource;

    public IList<Data.Entities.TipoDocumento>? ItemsSource
    {
        get
        {
            if (_itemsSource == null && !IsInDesignMode)
            {
                _itemsSource = _context.TipoDocumentos.ToList();
            }

            return _itemsSource;
        }
    }

    [Command]
    public async void ValidateRow(RowValidationArgs args)
    {
        try
        {
            var item = (Data.Entities.TipoDocumento)args.Item;
            if (args.IsNewItem)
                _context.TipoDocumentos.Add(item);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error validating data");
        }
    }

    [Command]
    public void ValidateRowDeletion(ValidateRowDeletionArgs args)
    {
        var item = (Data.Entities.TipoDocumento)args.Items.Single();
        _context.TipoDocumentos.Remove(item);
        _context.SaveChanges();
    }

    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        _itemsSource = null;
        RaisePropertyChanged(nameof(ItemsSource));
    }
}