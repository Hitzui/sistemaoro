using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Mvvm.Xpf;

namespace SistemaOro.Forms.ViewModels.Agencias;

public class AgenciasViewModel :BaseViewModel
{
    public AgenciasViewModel()
    {
        Title = "Agencias";
    }
    DataContext _Context;
    IList<Agencia> _ItemsSource;
    public IList<Agencia> ItemsSource
    {
        get
        {
            if (_ItemsSource == null && !DevExpress.Mvvm.ViewModelBase.IsInDesignMode)
            {
                _Context = new DataContext();
                _ItemsSource = _Context.Agencias.ToList();
            }
            return _ItemsSource;
        }
    }
    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        _ItemsSource = null;
        _Context = null;
        RaisePropertyChanged(nameof(ItemsSource));
    }
}