using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Mvvm.Xpf;

namespace SistemaOro.Forms.ViewModels.TipoDocumento;

public class TipoDocumentoViewModel : BaseViewModel
{
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
    public void ValidateRow(RowValidationArgs args)
    {
        var item = (Data.Entities.TipoDocumento)args.Item;
        if (args.IsNewItem)
            _context.TipoDocumentos.Add(item);
        _context.SaveChanges();
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