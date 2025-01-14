using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using NLog;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services.Helpers;
using Unity;

namespace SistemaOro.Forms.ViewModels.Descargue;

public class ListaDescarguesViewModels : BaseViewModel
{
    private Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly ICompraRepository _compraRepository;
    private readonly IDescarguesRepository _descarguesRepository;

    public ListaDescarguesViewModels()
    {
        Title = "Lista de compras a descarga";
        _compraRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ICompraRepository>();
        _descarguesRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IDescarguesRepository>();
        _fecha = DateTime.Now;
        _dtoComprasClientes = new();
        FilterCommand = new DelegateCommand(OnFilterCommand);
        SelectAllCommand = new DelegateCommand(OnSelectAllCommand);
        DeselectAllCommand = new DelegateCommand(OnDeselectAllCommand);
        SaveCommand = new DelegateCommand(OnSaveCommand);
    }

    public ICommand SaveCommand { get; }
    public ICommand FilterCommand { get; }
    public ICommand SelectAllCommand { get; }
    public ICommand DeselectAllCommand { get; }

    private DateTime _fecha;

    public DateTime Fecha
    {
        get => _fecha;
        set => SetValue(ref _fecha, value);
    }

    private List<DtoComprasClientes> _dtoComprasClientes;

    public List<DtoComprasClientes> DtoComprasClientes
    {
        get => _dtoComprasClientes;
        set => SetValue(ref _dtoComprasClientes, value);
    }

    public async void OnFilterCommand()
    {
        try
        {
            DtoComprasClientes = await _compraRepository.FindComprasClientesFechaAndCerrada(Fecha);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error al cargar los datos para descargue");
        }
    }

    public void OnSelectAllCommand()
    {
        if (DtoComprasClientes.Count <= 0)
        {
            return;
        }

        foreach (var dtoComprasClientese in DtoComprasClientes)
        {
            dtoComprasClientese.IsChecked = true;
        }
    }

    public void OnDeselectAllCommand()
    {
        if (DtoComprasClientes.Count <= 0)
        {
            return;
        }

        foreach (var dtoComprasClientese in DtoComprasClientes)
        {
            dtoComprasClientese.IsChecked = false;
        }
    }

    private void OnSaveCommand()
    {
        if (DtoComprasClientes.Count <= 0)
        {
            HelpersMessage.MensajeInformacionResult("Guardar", "No hay datos a guardar");
            return;
        }

        foreach (var data in DtoComprasClientes.Where(data => data.IsChecked))
        {
            _descarguesRepository.GenerarLote(DateTime.Now);
        }
    }
}