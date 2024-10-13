using DevExpress.Mvvm.Native;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using Unity;
using SistemaOro.Forms.Views.Reportes.Caja;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Views.Reportes.Compras;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class ReportesMaestroCajaViewModel : BaseViewModel
{
    private readonly IMaestroCajaRepository _maestroCajaRepository;
    private List<DtoMovimientosCaja> _findAll;
    private readonly IParametersRepository _parametersRepository;
    private readonly IAgenciaRepository _agenciaRepository;

    public ReportesMaestroCajaViewModel()
    {
        _maestroCajaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMaestroCajaRepository>();
        _parametersRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IParametersRepository>();
        _agenciaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IAgenciaRepository>();
        Title = @"Reportes de movimientos en caja";
        MovimientosCajas = new DXObservableCollection<DtoMovimientosCaja>();
        ReportCommand = new DelegateCommand(OnReportCommand);
        _findAll = new List<DtoMovimientosCaja>();
    }

    private async void OnReportCommand()
    {
        var report = new RptMovimientosCaja();
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
        report.picLogo.ImageSource = new ImageSource(HelpersMethods.LoadDxImage(agencia!.Logo));
        report.lblNombreAgencia.Text = agencia.Nomagencia;
        report.lblDireccion.Text = agencia.Diragencia;
        HelpersMethods.LoadReport(report);
    }

    private bool _cajaActiva;

    public bool CajaActiva
    {
        get => _cajaActiva;
        set => SetValue(ref _cajaActiva, value);
    }

    public DXObservableCollection<DtoMovimientosCaja> MovimientosCajas
    {
        get => GetValue<DXObservableCollection<DtoMovimientosCaja>>();
        set => SetValue(value);
    }

    public ICommand ReportCommand { get; }

    public DateTime FechaDesde
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
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