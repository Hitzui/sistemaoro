using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using SistemaOro.Forms.Views.Reportes.Caja;
using SistemaOro.Forms.Views.Reportes.Compras;
using Unity;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class RealizarMovimientoCajaViewModel : BaseViewModel
{
    private IMovimientosRepository _movimientosRepository;
    private IMaestroCajaRepository _maestroCajaRepository;
    private IMonedaRepository _monedaRepository;
    public RealizarMovimientoCajaViewModel()
    {
        Title = "Realizar Movimiento Caja";
        _movimientosRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMovimientosRepository>();
        _maestroCajaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMaestroCajaRepository>();
        _monedaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMonedaRepository>();
        SaveCommand = new DelegateCommand(OnSaveCommand);
        IsEfectivo = true;
    }

    private async void OnSaveCommand()
    {
        var result = HelpersMessage.MensajeConfirmacionResult(MensajesRealizarMovimientos.MovimientoCajaTitulo,
            MensajesRealizarMovimientos.RealizarMovimientoCaja);
        if (result == MessageBoxResult.Cancel)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Referencia))
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo,
                MensajesRealizarMovimientos.ReferenciaVacia);
            return;
        }

        if (Monto is null || decimal.Compare(Monto!.Value, decimal.Zero) <= 0)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo,
                MensajesRealizarMovimientos.MontoVacio);
            return;
        }

        if (SelectedMovcaja is null)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo,
                MensajesRealizarMovimientos.MovimientoVacio);
            return;
        }

        if (SelectedMoneda is null)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo,
                "Seleccionar moneda");
            return;
        }

        var parametros = VariablesGlobalesForm.Instance.Parametros!;
        if (parametros.Idcompras == SelectedMovcaja.Idmov || parametros.AnularCompra == SelectedMovcaja.Idmov)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo,
                MensajesRealizarMovimientos.ValidarMovimientoCompra);
            return;
        }

        if (parametros.IdAdelantos == SelectedMovcaja.Idmov || parametros.AnularAdelanto == SelectedMovcaja.Idmov ||
            parametros.PagoAdelanto == SelectedMovcaja.Idmov)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo,
                MensajesRealizarMovimientos.ValidarMovimientoAdelanto);
            return;
        }

        var caja = VariablesGlobalesForm.Instance.VariablesGlobales["CAJA"];
        var agencia = VariablesGlobalesForm.Instance.VariablesGlobales["AGENCIA"];
        var validarCaja = await _maestroCajaRepository.ValidarCajaAbierta(caja!, agencia!);
        if (!validarCaja)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo, MensajesMaestroCaja.CajaAbierta);
            return;
        }

        var mcaja = await _maestroCajaRepository.FindByCajaAndAgencia(caja, agencia);
        if (mcaja is null)
        {
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.ErrorTitulo, MensajesMaestroCaja.CajaNoValida);
            return;
        }

        var cheque = decimal.Zero;
        var efectivo = decimal.Zero;
        var transferencia = decimal.Zero;
        var chequeExt = decimal.Zero;
        var efectivoExt = decimal.Zero;
        var transferenciaExt = decimal.Zero;
        if (parametros.Dolares == SelectedMoneda.Codmoneda)
        {
            if (IsCheque)
            {
                chequeExt = Monto ?? decimal.Zero;
            }
            else if (IsEfectivo)
            {
                efectivoExt = Monto ?? decimal.Zero;
            }
            else if (IsTransferencia)
            {
                transferenciaExt = Monto ?? decimal.Zero;
            }
        }
        else
        {
            if (IsCheque)
            {
                cheque = Monto ?? decimal.Zero;
            }
            else if (IsEfectivo)
            {
                efectivo = Monto ?? decimal.Zero;
            }
            else if (IsTransferencia)
            {
                transferencia = Monto ?? decimal.Zero;
            }
        }

        var detalleMovimiento = new Detacaja
        {
            Referencia = Referencia,
            Concepto = Referencia,
            Idcaja = mcaja.Idcaja,
            Idmov = SelectedMovcaja.Idmov,
            Fecha = DateTime.Now,
            Cheque = cheque,
            Efectivo = efectivo,
            Transferencia = transferencia,
            ChequeExt = chequeExt,
            EfectivoExt = efectivoExt,
            TransferenciaExt = transferenciaExt,
            Hora = DateTime.Now.ToShortTimeString(),
            Codcaja = caja,
            Idmoeda = SelectedMoneda.Codmoneda,
            Codoperador = VariablesGlobalesForm.Instance.Usuario.Codoperador
        };
        var save = await _maestroCajaRepository.GuardarDatosDetaCaja(detalleMovimiento, SelectedMovcaja, mcaja);
        if (save <= 0)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _maestroCajaRepository.ErrorSms);
        }
        else
        {
            VariablesGlobalesForm.Instance.MaestroCaja =
                await _maestroCajaRepository.FindByCajaAndAgencia(caja, agencia);
            HelpersMessage.MensajeInformacionResult(MensajesGenericos.InformacionTitulo,
                MensajesRealizarMovimientos.MovimientoRealizadoCorrecto);
            result = HelpersMessage.MensajeConfirmacionResult(MensajesRealizarMovimientos.MovimientoCajaTitulo,
                MensajesRealizarMovimientos.ImprimirComprobante);
            if (result == MessageBoxResult.Cancel)
            {
                CloseAction!.Invoke();
            }
            else
            {
                var sum = detalleMovimiento.Efectivo + detalleMovimiento.Cheque + detalleMovimiento.Transferencia;
                var cantidadLetras = HelpersMethods.ConvertirNumeroADecimalATexto(sum ?? decimal.Zero);
                var reporteMovimientoCaja = new ReporteMovimientoCaja();
                reporteMovimientoCaja.Parameters["parIdDetaCaja"].Value = save;
                reporteMovimientoCaja.Parameters["parCantidadLetras"].Value = cantidadLetras;
                reporteMovimientoCaja.Parameters["parNombre"].Value = parametros.Recibe;
                HelpersMethods.LoadReport(reporteMovimientoCaja, "Reporte de Movimiento Caja");
                CloseAction!.Invoke();
            }
        }
    }

    public ICommand SaveCommand { get; set; }

    public string? Referencia
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public decimal? Monto
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public IList<Movcaja> MovimientosCaja
    {
        get => GetValue<IList<Movcaja>>();
        set => SetValue(value);
    }

    public IList<Moneda> Monedas
    {
        get => GetValue<IList<Moneda>>();
        set => SetValue(value);
    }

    public Movcaja? SelectedMovcaja
    {
        get => GetValue<Movcaja>();
        set => SetValue(value);
    }

    public Moneda? SelectedMoneda
    {
        get => GetValue<Moneda>();
        set => SetValue(value);
    }
    public bool IsCheque
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsTransferencia
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsEfectivo
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public string ReferenciaValidacion
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public async void Load()
    {
        MovimientosCaja = await _movimientosRepository.FindAll();
        Monedas = await _monedaRepository.FindAll();
    }
}