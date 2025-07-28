using System;
using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NLog;
using SistemaOro.Forms.Services.Helpers;

namespace SistemaOro.Forms.ViewModels.Clientes;

public class FilterClientesViewModel : BaseViewModel
{
    private readonly IClienteRepository clienteRepository;
    private readonly Logger logger = LogManager.GetCurrentClassLogger();
    public FilterClientesViewModel()
    {
        Title = "Seleccionar Cliente";
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        clienteRepository = unitOfWork.ClienteRepository;
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
        if (SelectedCliente is null)
        {
            HelpersMessage.MensajeInformacionResult("Seleccionar", "Debe seleccionar un cliente");
            return;
        }
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
        try
        {
            _itemsSource.Clear();
            var itemsSource = await clienteRepository.FindAll();
            foreach (var cliente in itemsSource)
            {
                _itemsSource.Add(cliente);
            }
        }
        catch (Exception e)
        {
            logger.Error(e, "Error al cargar los datos");
        }
    }
}