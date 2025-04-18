﻿using DevExpress.Mvvm.Native;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Drawing;
using Unity;
using SistemaOro.Forms.Views.Reportes.Caja;
using SistemaOro.Forms.Services.Helpers;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class ReportesMaestroCajaViewModel : BaseViewModel
{
    private readonly IMaestroCajaRepository _maestroCajaRepository;
    private List<DtoMovimientosCaja> _findAll;
    private readonly IParametersRepository _parametersRepository;
    private readonly IAgenciaRepository _agenciaRepository;
    private readonly IMonedaRepository _monedaRepository;

    public ReportesMaestroCajaViewModel()
    {
        _maestroCajaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMaestroCajaRepository>();
        _parametersRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IParametersRepository>();
        _agenciaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IAgenciaRepository>();
        _monedaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMonedaRepository>();
        Title = @"Reportes de movimientos en caja";
        MovimientosCajas = [];
        ReportCommand = new DelegateCommand(OnReportCommand);
        _findAll = new List<DtoMovimientosCaja>();
    }

    private async void OnReportCommand()
    {
        try
        {
            var report = new RptMovimientosCaja();
            var monedas = await _monedaRepository.FindAll();
            var param = await _parametersRepository.RecuperarParametros();
            if (param is null)
            {
                return;
            }
            if (MovimientosCajaRad)
            {
                if (CajaActiva)
                {
                    _findAll = await _maestroCajaRepository.FindAllByFechaDesdeActiva(FechaDesde);
                }
                else
                {
                    _findAll = await _maestroCajaRepository.FindAllByFechaDesde(FechaDesde);
                }
                report.DataSource = _findAll;
                var agencia = await _agenciaRepository.GetByIdAsync(VariablesGlobalesForm.Instance.VariablesGlobales["AGENCIA"]!);
                if (agencia!.Logo is not null)
                {
                    report.picLogo.ImageSource = new ImageSource(HelpersMethods.LoadDxImage(agencia!.Logo)); 
                }

                var monedaLocal =await _monedaRepository.GetByIdAsync(param.Cordobas ?? 0);
                var monedaExt = await _monedaRepository.GetByIdAsync(param.Dolares ?? 0);
                report.lblNombreAgencia.Text = agencia.Nomagencia;
                report.lblDireccion.Text = agencia.Diragencia;
                report.lblRuc.Text=agencia.Ruc;
                report.lblMonedalocal.Text = monedaLocal is not null ? monedaLocal.Simbolo : "";
                report.lblMonedaExt.Text = monedaExt is not null ? monedaExt.Simbolo : "";
                HelpersMethods.LoadReport(report);
            }

            if (ConsolidadoCajaRad)
            {
                XtraMessageBox.Show("Consolidado caja");
            }

            if (ComprobanteMovimientoRad)
            {
                var parametros = VariablesGlobalesForm.Instance.Parametros;
                var sum = SelectedMovimiento.Efectivo + SelectedMovimiento.Cheque + SelectedMovimiento.Transferencia;
                var cantidadLetras = HelpersMethods.ConvertirNumeroADecimalATexto(sum);
                var reporteMovimientoCaja = new ReporteMovimientoCaja();
                reporteMovimientoCaja.Parameters["parIdDetaCaja"].Value = SelectedMovimiento.IdDetacaja;
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

    private bool _cajaActiva;

    public bool CajaActiva
    {
        get => _cajaActiva;
        set => SetValue(ref _cajaActiva, value);
    }

    private bool _movimientosCajaRad;
    private bool _consolidadoCajaRad;
    private bool _comprobanteMovimientoRad;

    public bool MovimientosCajaRad
    {
        get => _movimientosCajaRad;
        set => SetProperty(ref _movimientosCajaRad, value, "MovimientosCajaRad");
    }

    public bool ConsolidadoCajaRad
    {
        get => _consolidadoCajaRad;
        set => SetProperty(ref _consolidadoCajaRad, value, "ConsolidadoCajaRad");
    }

    public bool ComprobanteMovimientoRad
    {
        get => _comprobanteMovimientoRad;
        set => SetProperty(ref _comprobanteMovimientoRad, value, "ComprobanteMovimientoRad");
    }

    public DXObservableCollection<DtoMovimientosCaja> MovimientosCajas
    {
        get => GetValue<DXObservableCollection<DtoMovimientosCaja>>();
        set => SetValue(value);
    }

    private DtoMovimientosCaja _selectedMovimiento;

    public DtoMovimientosCaja SelectedMovimiento
    {
        get => _selectedMovimiento;
        set => SetProperty(ref _selectedMovimiento, value, "SelectedMovimiento");
    }

    public ICommand ReportCommand { get; }

    private DateTime _fechaDesde;
    public DateTime FechaDesde
    {
        get => _fechaDesde;
        set => SetValue(ref _fechaDesde, value,  ChangeDateMovimientosCaja);
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
            XtraMessageBox.Show(e.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            _findAll = await _maestroCajaRepository.FindAllByIdMaestroCaja(VariablesGlobalesForm.Instance.MaestroCaja.Idcaja);
            MovimientosCajas.AddRange(_findAll);
            FechaDesde = DateTime.Now;
            FechaHasta = DateTime.Now;
        }
    }
}