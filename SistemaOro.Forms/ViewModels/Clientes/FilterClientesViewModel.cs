using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DevExpress.Xpf.CodeView;

namespace SistemaOro.Forms.ViewModels.Clientes;

public class FilterClientesViewModel : BaseViewModel
{
    private readonly IClienteRepository _clienteRepository;

    public FilterClientesViewModel()
    {
        Title = "Seleccionar Cliente";
        _clienteRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IClienteRepository>();
    }

    public Cliente? SelectedCliente
    {
        get => GetValue<Cliente>();
        set => SetValue(value);
    }

    private ObservableCollection<Cliente> _itemsSource = new ObservableCollection<Cliente>();

    public ObservableCollection<Cliente> ItemsSource => _itemsSource;

    [Command]
    public void SelectedClienteCommand()
    {
        Debug.WriteLine(SelectedCliente.Codcliente);
        CloseAction?.Invoke();
    }

    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        Load();
        RaisePropertyChanged(nameof(ItemsSource));
    }

    public async void Load()
    {
        _itemsSource.Clear();
        var itemsSource = await _clienteRepository.FindAll();
        _itemsSource.AddRange(itemsSource);
    }
}