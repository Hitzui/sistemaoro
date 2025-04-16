using System.Collections.Generic;
using System.Windows.Navigation;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.XtraPrinting.Drawing;
using NLog;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Views.Compras;
using SistemaOro.Forms.Views.Reportes.Compras;
using Unity;
using System.Drawing;
using System.IO;

namespace SistemaOro.Forms.ViewModels.Compras;

public class ComprasViewModel : BaseViewModel
{
    private readonly ICompraRepository _compraRepository;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    public ComprasViewModel()
    {
        _compraRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ICompraRepository>();
        Title = "Compras";
    }


    public DtoComprasClientes? SelectedCompra
    {
        get => GetValue<DtoComprasClientes>();
        set => SetValue(value);
    }

    public IList<DtoComprasClientes> ComprasClientesList
    {
        get => GetValue<List<DtoComprasClientes>>();
        set => SetValue(value);
    }

    [Command]
    public void EditarCompraCommand(NavigationService? navigationService)
    {
        if (SelectedCompra is null)
        {
            HelpersMessage.MensajeInformacionResult("Editar", "No ha seleccionado una compra a editar, intente nuevamente");
            return;
        }

        if (navigationService is null)
        {
            return;
        }
        var frmCompra = new FormComprasPage();
        frmCompra.SetSelectedCompra(SelectedCompra);
        navigationService.Navigate(frmCompra);
    }
    public async void ImprimirCompra()
    {
        if (SelectedCompra is null)
        {
            HelpersMessage.MensajeErroResult("Imprimir", "No hay una compra seleccionada a imprimir");
            return;
        }

        var findCompra = await _compraRepository.DetalleCompraImprimir(SelectedCompra.Numcompra!);
        _logger.Info($"Numero de compra {SelectedCompra.Numcompra} - Cantidad de datos en la lista: {findCompra.Count}");
        if (findCompra.Count>0)
        {
            foreach (var detalleCompra in findCompra)
            {
                _logger.Info($"Datos: {detalleCompra.Numcompra} - {detalleCompra.Nocontrato}");
            }
        }
        //Reporte Anexo
        var reporteAnexo = new ReporteAnexo();
        reporteAnexo.DataSource = findCompra;
        if (!string.IsNullOrWhiteSpace(SelectedCompra.Firma))
        {
            var image = HelpersMethods.LoadSigImage(SelectedCompra.Firma, SelectedCompra.Numcompra ?? "");
            reporteAnexo.imgFirma.ImageSource = new ImageSource(image);
        }
        HelpersMethods.LoadReport(reporteAnexo,"Reporte de Anexo");
        //Reporte Contrato Contra Venta
        var reporteContrantoContraVenta = new ReporteContratoContraVenta();
        reporteContrantoContraVenta.DataSource = findCompra;
        HelpersMethods.LoadReport(reporteContrantoContraVenta,"Reporte de Contrato de Contra Venta");
        //Reporte Contrato Prestamo
        var reporteContrantoPrestamo = new ReporteContrantoPrestamo();
        reporteContrantoPrestamo.DataSource = findCompra;
        HelpersMethods.LoadReport(reporteContrantoPrestamo, "Reporte de Contrato de Prestamo");
        //Reporte Comprobante de compra
        var reporteComprobanteCompra = new ReporteComprobanteCompra();
        reporteComprobanteCompra.DataSource = findCompra;
        //reporteComprobanteCompra.Parameters["parNumCompra"].Value = SelectedCompra.Numcompra;
        HelpersMethods.LoadReport(reporteComprobanteCompra);
    }

    public async void Load()
    {
        ComprasClientesList = await _compraRepository.FindComprasClientes();
    }

    public void ImprimirTicketCompra()
    {
        if (SelectedCompra is null)
        {
            HelpersMessage.MensajeErroResult("Imprimir", "No hay una compra seleccionada a imprimir");
            return;
        }
        var reporteCompraTicket = new ReporteCompraTicket();
        reporteCompraTicket.Parameters["parNumeroCompra"].Value = SelectedCompra.Numcompra;
        HelpersMethods.LoadReport(reporteCompraTicket);
    }
}