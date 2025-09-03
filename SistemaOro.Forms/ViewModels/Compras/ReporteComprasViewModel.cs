using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.Spreadsheet;
using NLog;
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
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    public ReporteComprasViewModel()
    {
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _comprasRepository = unitOfWork.CompraRepository;
        _parametersRepository = unitOfWork.ParametersRepository;
        _monedaRepository = unitOfWork.MonedaRepository;
    }

    private int _selectedValueIndex = -1;

    public int SelectedValueIndex
    {
        get => _selectedValueIndex;
        set => SetValue(ref _selectedValueIndex, value);
    }

    private DateTime _fechaDesde = DateTime.Now;

    public DateTime FechaDesde
    {
        get => _fechaDesde;
        set => SetValue(ref _fechaDesde, value);
    }

    private DateTime _fechaHasta = DateTime.Now;

    public DateTime FechaHasta
    {
        get => _fechaHasta;
        set => SetValue(ref _fechaHasta, value);
    }

    private Cliente? _selectedCliente;

    public Cliente? SelectedCliente
    {
        get => _selectedCliente;
        set => SetValue(ref _selectedCliente, value);
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
            if (SelectedValueIndex < 0)
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
                    if (getData.Count <= 0)
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

                    var data = await _comprasRepository.DetalleCompraImprimirPorClientePorFecha(
                        SelectedCliente!.Codcliente, FechaDesde, FechaHasta);
                    if (data.Count <= 0)
                    {
                        HelpersMessage.MensajeErroResult("Seleccionar",
                            "No hay datos en el rango de fecha seleccionado");
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

    [Command]
    public async Task GenerarExcelOnza()
    {
        // Crear y mostrar el SplashScreen al inicio
        var splash = SplashScreenManager.CreateWaitIndicator(topmost: true);
        splash.Show(startupLocation: WindowStartupLocation.CenterOwner);
        var spreadsheetControl = new SpreadsheetControl();
        var workbook = spreadsheetControl.Document;
        try
        {
            if (SelectedValueIndex < 0)
            {
                splash.Close();
                return;
            }

            var param = await _parametersRepository.RecuperarParametros();
            if (param is null)
            {
                splash.Close();
                HelpersMessage.MensajeInformacionResult("Sistema Oro", "No se encontraron los parámetros del sistema");
                return;
            }

            var monedaLocal = await _monedaRepository.GetByIdAsync(param.Cordobas);
            var monedaExt = await _monedaRepository.GetByIdAsync(param.Dolares);

            var reportGeneralOnzas = new ReporteQuilatesOnzas();
            var getData = await _comprasRepository.ListadoComprasOnzas(FechaDesde, FechaHasta);

            if (getData.Count <= 0)
            {
                splash.Close();
                HelpersMessage.MensajeErroResult("Reporte", "No hay datos en el rango de fechas seleccionado");
                return;
            }

            var moneda = (int codmoneda) => codmoneda == param.Cordobas ? monedaLocal.Simbolo : monedaExt.Simbolo;
            var count = getData.GroupBy(onza => onza.Numcompra).Count();
            var sumPeso = getData.Sum(onza => onza.Peso);
            var sumOnzas = getData.Sum(onza => onza.Onzas);
            var sumLocal = getData.Where(onza => onza.MonedaLocal == 1).Sum(onza => onza.Importe);
            var sumExt = getData.Where(onza => onza.MonedaLocal == 0).Sum(onza => onza.Importe);
            var worksheet = workbook.Worksheets[0];
            splash.ViewModel.Status = "Creando archivo excel...";
            // Configuración del documento Excel (igual que antes)
            var titulo = worksheet["A1:N1"];
            titulo.Merge();
            titulo.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            titulo.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            titulo.Font.Bold = true;
            titulo.Value = "REPORTE GENERAL POR ONZAS";
            titulo.Font.Size = 18;
            worksheet["A1"].RowHeight = 250;
            worksheet["C2:d2"].Merge();
            worksheet["C2:d2"].Value = "Rango de fecha";
            worksheet["F2:G2"].Merge();
            worksheet["F2:G2"].Value = "Desde";
            worksheet["h2:i2"].Merge();
            worksheet["h2:i2"].Value = FechaDesde.ToString("dd/MM/yyyy");
            worksheet["j2:k2"].Merge();
            worksheet["j2:k2"].Value = "Hasta";
            worksheet["l2:m2"].Merge();
            worksheet["l2:m2"].Value = FechaHasta.ToString("dd/MM/yyyy");

            var headers = new[]
            {
                "Fecha", "Lectura", "No Compra", "Quilate", "Peso",
                "Precio", "", "Importe", "T/C", "Precio Oro",
                "Margen", "Onzas", "Cliente", "Caja"
            };

            const int headerRow = 2;
            for (var i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[headerRow, i].Value = headers[i];
            }

            var headerRange = worksheet.Range.FromLTRB(0, headerRow, headers.Length - 1, headerRow);
            headerRange.Font.Bold = true;
            headerRange.Fill.BackgroundColor = Color.LightGray;

            var initRow = 3;
            foreach (var data in getData)
            {
                worksheet.Cells[initRow, 0].Value = data.Fecha;
                worksheet.Cells[initRow, 1].Value = data.Lectura;
                worksheet.Cells[initRow, 2].Value = data.Numcompra;
                worksheet.Cells[initRow, 3].Value = data.Kilate;
                worksheet.Cells[initRow, 4].Value = data.Peso;
                worksheet.Cells[initRow, 5].Value = data.Preciok;
                worksheet.Cells[initRow, 6].Value = moneda(data.Codmoneda);
                worksheet.Cells[initRow, 7].Value = data.Importe;
                worksheet.Cells[initRow, 8].Value = data.Tipocambio;
                worksheet.Cells[initRow, 9].Value = data.PrecioOro;
                worksheet.Cells[initRow, 10].Value = data.VMargen;
                worksheet.Cells[initRow, 11].Value = data.Onzas;
                worksheet.Cells[initRow, 12].Value = $"{data.Nombres} {data.Apellidos}";
                worksheet.Cells[initRow, 13].Value = data.Codcaja;
                initRow++;
            }

            initRow += 2;
            worksheet[$"B{initRow}:C{initRow}"].Value = "Cantidad de compras:";
            worksheet[$"B{initRow}:C{initRow}"].Merge();
            worksheet[$"D{initRow}"].Value = count;
            worksheet[$"F{initRow}"].Value = "Peso:";
            worksheet[$"G{initRow}:H{initRow}"].Value = sumPeso;
            worksheet[$"G{initRow}:H{initRow}"].Merge();
            worksheet[$"J{initRow}"].Value = "Importe:";
            worksheet[$"K{initRow}:L{initRow}"].Value = $"{monedaLocal.Simbolo} {sumLocal}";
            worksheet[$"K{initRow}:L{initRow}"].Merge();
            worksheet[$"K{initRow + 1}:L{initRow + 1}"].Value = $"{monedaExt.Simbolo} {sumExt}";
            worksheet[$"K{initRow + 1}:L{initRow + 1}"].Merge();
            worksheet[$"M{initRow}"].Value = $"Onzas: {sumOnzas}";
            worksheet.Columns.AutoFit(0, initRow + 1);

            // Mostrar el diálogo de guardado
            splash.ViewModel.Status = "Preparando para guardar archivo excel...";
            using var saveFileDialog = new DXSaveFileDialog()
            {
                Title = "Guardar como...",
                Filter = "Archivos de Excel (*.xlsx)|*.xlsx|Todos los archivos (*.*)|*.*",
                DefaultExt = "xlsx",
                AddExtension = true
            };

            // Cerrar el splash antes de mostrar el diálogo
            splash.Close();
            var showDialog = saveFileDialog.ShowDialog();
            if (showDialog.HasValue && showDialog.Value)
            {
                splash.Show();
                splash.ViewModel.Status= "Guardando archivo...";
                
                spreadsheetControl.Document.SaveDocument(saveFileDialog.FileName, DocumentFormat.Xlsx);
                splash.Close();
                var confirm = HelpersMessage.MensajeConfirmacionResult("Guardar",
                    "Archivo Guardado correctamente, ¿Abrir archivo y visualizar contenido?");
            
                if (confirm == MessageBoxResult.OK)
                {
                    // Ejecutar en el hilo de la UI
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                        }
                        catch (Exception ex)
                        {
                            HelpersMessage.MensajeErroResult("Error", $"No se pudo abrir el archivo: {ex.Message}");
                        }
                    });
                }
                splash.Close();
            }
        }
        catch (Exception e)
        {
            logger.Error(e, "Error al generar el archivo Excel");
            splash.Close();
            var sms = e.InnerException?.Message ?? string.Empty;
            HelpersMessage.MensajeErroResult("Error", $"{e.Message} - {sms}");
        }
        finally
        {
            // Asegurarse de cerrar el splash en cualquier caso
            if (splash.State == SplashScreenState.Shown)
            {
                splash.Close();
            }
            workbook.Dispose();
            spreadsheetControl.Dispose();
        }
    }
}