using DevExpress.Mvvm.Native;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Drawing;
using SistemaOro.Data.Entities;
using Unity;
using SistemaOro.Forms.Views.Reportes.Caja;
using SistemaOro.Forms.Services.Helpers;
using RptMovimientosCaja = SistemaOro.Forms.Views.Reportes.Caja.RptMovimientosCaja;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class ReportesMaestroCajaViewModel : BaseViewModel
{
    private readonly IMaestroCajaRepository _maestroCajaRepository;
    private List<MovimientosCajaSelect> _findAll;
    private readonly IParametersRepository _parametersRepository;
    private readonly IAgenciaRepository _agenciaRepository;
    private readonly IMonedaRepository _monedaRepository;

    public ReportesMaestroCajaViewModel()
    {
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _maestroCajaRepository = unitOfWork.MaestroCajaRepository;
        _parametersRepository = unitOfWork.ParametersRepository;
        _agenciaRepository = unitOfWork.AgenciaRepository;
        _monedaRepository = unitOfWork.MonedaRepository;
        Title = @"Reportes de movimientos en caja";
        MovimientosCajas = [];
        ReportCommand = new DelegateCommand(OnReportCommand);
        _findAll = new List<MovimientosCajaSelect>();
    }

    private async void OnReportCommand()
    {
        try
        {
            var codAgencia = VariablesGlobalesForm.Instance.VariablesGlobales["AGENCIA"] ?? "";
            var monedas = await _monedaRepository.FindAll();
            var param = await _parametersRepository.RecuperarParametros();
            if (param is null)
            {
                return;
            }

            if (MovimientosCajaRad)
            {
                var report = new RptMovimientosCaja();
                _findAll = await _maestroCajaRepository.FindAllByFechaDesde(FechaDesde);
                var sumEfectivoLocal = _findAll.Where(select => select.Efectivo1.Value > 0).Sum(select => select.Efectivo1);
                report.DataSource = _findAll;
                var agencia =
                    await _agenciaRepository.GetByIdAsync(codAgencia);
                if (agencia!.Logo is not null)
                {
                    report.picLogo.ImageSource = new ImageSource(HelpersMethods.LoadDxImage(agencia!.Logo));
                }

                var monedaLocal = await _monedaRepository.GetByIdAsync(param.Cordobas ?? 0);
                var monedaExt = await _monedaRepository.GetByIdAsync(param.Dolares ?? 0);
                report.lblNombreAgencia.Text = agencia.Nomagencia;
                report.lblDireccion.Text = agencia.Diragencia;
                report.lblMonedalocal.Text = monedaLocal is not null ? monedaLocal.Simbolo : "";
                report.lblMonedaExt.Text = monedaExt is not null ? monedaExt.Simbolo : "";
                //report.Parameters["parEntradaEfectivoLocal"].Value = sumEfectivoLocal;
                HelpersMethods.LoadReport(report);
            }

            if (MovimientosCajaFechaRad)
            {
                var findFechaCaja =
                    await _maestroCajaRepository.FindAllByFechaDesdeAndFechaHasta(FechaDesde, FechaHasta);
                var reportMovimientosCajaFechas = new ReporteMovimientosCajaFechas();
                reportMovimientosCajaFechas.DataSource = findFechaCaja;
                var agencia =
                    await _agenciaRepository.GetByIdAsync(VariablesGlobalesForm.Instance.VariablesGlobales["AGENCIA"]!);
                if (agencia!.Logo is not null)
                {
                    reportMovimientosCajaFechas.picLogo.ImageSource =
                        new ImageSource(HelpersMethods.LoadDxImage(agencia.Logo));
                }

                var monedaLocal = await _monedaRepository.GetByIdAsync(param.Cordobas ?? 0);
                var monedaExt = await _monedaRepository.GetByIdAsync(param.Dolares ?? 0);
                reportMovimientosCajaFechas.lblNombreAgencia.Text = agencia.Nomagencia;
                reportMovimientosCajaFechas.lblDireccion.Text = agencia.Diragencia;
                reportMovimientosCajaFechas.Parameters["parMonedaLocal"].Value = monedaLocal is not null ? monedaLocal.Simbolo : "";
                reportMovimientosCajaFechas.Parameters["parMonedaExt"].Value = monedaExt is not null ? monedaExt.Simbolo : "";
                reportMovimientosCajaFechas.lblFechaDesde.Text = FechaDesde.ToShortDateString();
                reportMovimientosCajaFechas.lblFechaHasta.Text = FechaHasta.ToShortDateString();
                HelpersMethods.LoadReport(reportMovimientosCajaFechas);
            }

            if (ConsolidadoCajaRad)
            {
                XtraMessageBox.Show("Consolidado caja");
            }

            if (ComprobanteMovimientoRad)
            {
                if (SelectedMovimiento is null)
                {
                    HelpersMessage.MensajeErroResult("Error", "NO se ha seleccionado un movimiento a re-imprimir");
                    return;
                }
                var parametros = VariablesGlobalesForm.Instance.Parametros;
                var sum = 0m;
                var detaMovimiento = await _maestroCajaRepository.FindDetaCajaById(SelectedMovimiento.IdDetaCaja);
                if (detaMovimiento is null)
                {
                    HelpersMessage.MensajeErroResult("Error", "NO se ha seleccionado un movimiento a re-imprimir");
                    return;
                }

                if (param.Dolares.Value == detaMovimiento.Idmoeda.Value)
                {
                    sum += SelectedMovimiento.EfectivoExt ?? 0;
                    sum += SelectedMovimiento.TransferenciaExt ?? 0;
                }
                else
                {
                    sum += SelectedMovimiento.Efectivo1 ?? 0;
                    sum += SelectedMovimiento.Transferencia1 ?? 0;
                }

                var cantidadLetras = HelpersMethods.ConvertirNumeroADecimalATexto(sum);
                var reporteMovimientoCaja = new ReporteMovimientoCaja();
                reporteMovimientoCaja.Parameters["parIdDetaCaja"].Value = SelectedMovimiento.IdDetaCaja;
                reporteMovimientoCaja.Parameters["parCantidadLetras"].Value = cantidadLetras;
                reporteMovimientoCaja.Parameters["parNombre"].Value = parametros.Recibe;
                HelpersMethods.LoadReport(reporteMovimientoCaja, "Reporte de Movimiento Caja");
            }
        }
        catch (Exception e)
        {
            XtraMessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool _cajaActiva = true;

    public bool CajaActiva
    {
        get => _cajaActiva;
        set => SetValue(ref _cajaActiva, value);
    }

    private bool _movimientosCajaRad;
    private bool _movimientosCajaFechasRad;
    private bool _consolidadoCajaRad;
    private bool _comprobanteMovimientoRad;

    public bool MovimientosCajaRad
    {
        get => _movimientosCajaRad;
        set => SetValue(ref _movimientosCajaRad, value);
    }

    public bool MovimientosCajaFechaRad
    {
        get => _movimientosCajaFechasRad;
        set => SetValue(ref _movimientosCajaFechasRad, value);
    }

    public bool ConsolidadoCajaRad
    {
        get => _consolidadoCajaRad;
        set => SetValue(ref _consolidadoCajaRad, value);
    }

    public bool ComprobanteMovimientoRad
    {
        get => _comprobanteMovimientoRad;
        set => SetValue(ref _comprobanteMovimientoRad, value);
    }

    public DXObservableCollection<MovimientosCajaSelect> MovimientosCajas
    {
        get => GetValue<DXObservableCollection<MovimientosCajaSelect>>();
        set => SetValue(value);
    }

    private MovimientosCajaSelect? _selectedMovimiento;

    public MovimientosCajaSelect? SelectedMovimiento
    {
        get => _selectedMovimiento;
        set => SetValue(ref _selectedMovimiento, value);
    }

    public ICommand ReportCommand { get; }

    private DateTime _fechaDesde;

    public DateTime FechaDesde
    {
        get => _fechaDesde;
        set => SetValue(ref _fechaDesde, value, ChangeDateMovimientosCaja);
    }

    private async void ChangeDateMovimientosCaja()
    {
        try
        {
            MovimientosCajas.Clear();
            _findAll = await _maestroCajaRepository.FindAllByFechaDesde(FechaDesde);
            MovimientosCajas.AddRange(_findAll);
        }
        catch (Exception e)
        {
            XtraMessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public DateTime FechaHasta
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public async void Load()
    {
        if (VariablesGlobalesForm.Instance.MaestroCaja is not null)
        {
            _findAll = await _maestroCajaRepository.FindAllByIdMaestroCaja(VariablesGlobalesForm.Instance.MaestroCaja
                .Idcaja);
            MovimientosCajas.AddRange(_findAll);
            FechaDesde = DateTime.Now;
            FechaHasta = DateTime.Now;
        }
    }
}