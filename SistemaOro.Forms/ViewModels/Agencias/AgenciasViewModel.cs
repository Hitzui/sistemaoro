using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using Unity;

namespace SistemaOro.Forms.ViewModels.Agencias;

public class AgenciasViewModel :BaseViewModel
{
    private IAgenciaRepository _agenciaRepository;
    
    public AgenciasViewModel()
    {
        Title = "Agencias";
        _agenciaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IAgenciaRepository>();
    }

    private Agencia? _selectedAgencia;

    public Agencia? SelectedAgencia
    {
        get => _selectedAgencia;
        set
        {
            SetValue(ref _selectedAgencia, value);
            VariablesGlobalesForm.Instance.SelectedAgencia = value;
        }
    }
    private DXObservableCollection<Agencia>? _itemsSource;

    public DXObservableCollection<Agencia>? ItemsSource
    {
        get => _itemsSource;
        set => SetValue(ref _itemsSource, value);
    }

    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        OnLoad();
    }

    public async void OnLoad()
    {
        var data = await _agenciaRepository.FindAll();
        VariablesGlobalesForm.Instance.AgenciasCollection = new DXObservableCollection<Agencia>(data);
        ItemsSource = VariablesGlobalesForm.Instance.AgenciasCollection;
    }
}