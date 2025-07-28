using System;
using System.Linq;
using DevExpress.Mvvm.DataAnnotations;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.ViewModels.Clientes;
using SistemaOro.Forms.Views.Clientes;
using SistemaOro.Forms.Views.Reportes.Compras;
using Unity;

namespace SistemaOro.Forms.ViewModels.Compras;

public class ReporteComprasViewModel : BaseViewModel
{
    private readonly ICompraRepository _comprasRepository;
    private readonly IParametersRepository _parametersRepository;
    private readonly IMonedaRepository _monedaRepository;
    public ReporteComprasViewModel()
    {
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _comprasRepository = unitOfWork.CompraRepository;
        _parametersRepository = unitOfWork.ParametersRepository;
        _monedaRepository = unitOfWork.MonedaRepository;
    }

    private int _selectedValueIndex=-1;

    public int SelectedValueIndex
    {
        get=>_selectedValueIndex;
        set=>SetValue(ref _selectedValueIndex, value);
    }

    private DateTime _fechaDesde=DateTime.Now;

    public DateTime FechaDesde
    {
        get=>_fechaDesde;
        set=>SetValue(ref _fechaDesde, value);
    }

    private DateTime _fechaHasta=DateTime.Now;

    public DateTime FechaHasta
    {
        get=>_fechaHasta;
        set=>SetValue(ref _fechaHasta, value);
    }

    private Cliente? _selectedCliente;

    public Cliente? SelectedCliente
    {
        get=>_selectedCliente; 
        set=>SetValue(ref _selectedCliente, value);
    }

    [Command]
    public void BuscarCliente()
    {
        var frmSelectedCliente = new FilterClientesWindow();
        frmSelectedCliente.ShowDialog();
        var filterClientesViewModel = frmSelectedCliente.DataContext as FilterClientesViewModel;
        if (filterClientesViewModel!.SelectedCliente is not null)
        {
            SelectedCliente = filterClientesViewModel.SelectedCliente;
        }   
    }
    
    [Command]
    public async void VerReporte()
    {
        try
        {
            // Implementar la lógica para generar el reporte de compras
            // utilizando las fechas seleccionadas.
            if (SelectedValueIndex<0)
            {
                return;
            }

            var param = await _parametersRepository.RecuperarParametros();
            if (param is null)
            {
                HelpersMessage.MensajeInformacionResult("Sistema Oro", "No se encontraron los parámetros del sistema");
                return;
            }

            var monedaLocal = await _monedaRepository.GetByIdAsync(param.Cordobas);
            var monedaExt = await _monedaRepository.GetByIdAsync(param.Dolares);
            switch (SelectedValueIndex)
            {
                case 0: 
                    var reportGeneralOnzas = new ReporteQuilatesOnzas();
                    var getData = await _comprasRepository.ListadoComprasOnzas(FechaDesde, FechaHasta);
                    if (getData.Count<=0)
                    {
                        HelpersMessage.MensajeErroResult("Reporte", "No hay datos en el rango de fechas seleccionado");
                        return;
                    }
                    var count = getData.GroupBy(onza => onza.Numcompra).Count();
                    var sumLocal = getData.Where(onza => onza.MonedaLocal == 1).Sum(onza => onza.Importe);
                    var sumExt = getData.Where(onza => onza.MonedaLocal == 0).Sum(onza => onza.Importe);
                    reportGeneralOnzas.DataSource = getData;
                    reportGeneralOnzas.Parameters["parDesde"].Value = FechaDesde;
                    reportGeneralOnzas.Parameters["parHasta"].Value = FechaHasta;
                    reportGeneralOnzas.Parameters["parCantidad"].Value = count;
                    reportGeneralOnzas.Parameters["sumTotalLocal"].Value = sumLocal;
                    reportGeneralOnzas.Parameters["sumTotalExt"].Value = sumExt;
                    reportGeneralOnzas.Parameters["parMonedaLocal"].Value = monedaLocal.Simbolo;
                    reportGeneralOnzas.Parameters["parMonedaExt"].Value = monedaExt.Simbolo;
                    HelpersMethods.LoadReport(reportGeneralOnzas);
                    break;
                case 1:
                    if (SelectedCliente is null)
                    {
                        HelpersMessage.MensajeErroResult("Seleccionar", "Debe seleccionar un cliente para continuar");
                        return;
                    }
                    var data = await _comprasRepository.DetalleCompraImprimirPorClientePorFecha(SelectedCliente!.Codcliente, FechaDesde, FechaHasta);
                    if (data.Count<=0)
                    {
                        HelpersMessage.MensajeErroResult("Seleccionar", "No hay datos en el rango de fecha seleccionado");
                        return;
                    }
                    var reporteComprasClientePorFecha = new ReporteComprasClienteRangoFecha();
                    reporteComprasClientePorFecha.lblFechaDesde.Text = FechaDesde.ToString("dd/MM/yyyy");
                    reporteComprasClientePorFecha.lblFechaHasta.Text = FechaHasta.ToString("dd/MM/yyyy");
                    reporteComprasClientePorFecha.objectDataSourceDetalleCompra.DataSource = data;
                    HelpersMethods.LoadReport(reporteComprasClientePorFecha);
                    break;
            }
        }
        catch (Exception e)
        {
            var sms = "";
            if (e.InnerException is not null)
            {
                sms = e.InnerException.Message;
            }
            HelpersMessage.MensajeErroResult("Error", $"{e.Message} - {sms}");
        }
    }
}