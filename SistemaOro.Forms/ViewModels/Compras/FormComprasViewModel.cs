﻿using System;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;
using System.Collections.ObjectModel;
using System.Windows;
using SistemaOro.Forms.Repository;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using SistemaOro.Forms.ViewModels.Clientes;
using SistemaOro.Forms.Views.Clientes;
using static System.Decimal;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using SistemaOro.Forms.Views.Reportes.Compras;
using SistemaOro.Data.Dto;
using SistemaOro.Forms.Views;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Drawing;
using NLog;
using SistemaOro.Forms.Dto;

namespace SistemaOro.Forms.ViewModels.Compras
{
    public class FormComprasViewModel : BaseViewModel
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IPreciosKilatesRepository _preciosKilatesRepository;
        private readonly ICompraRepository _compraRepository;
        private readonly IDtoTipoPrecioRepository _tipoPrecioRepository;
        private readonly IMonedaRepository _monedaRepository;
        private readonly ITipoCambioRepository _tipoCambioRepository;
        private string _codagencia;
        private string _firma = string.Empty;
        private List<string> _mediosDePago = new();

        public FormComprasViewModel()
        {
            Title = "Realizar Compra";
            AddRowCommand = new DelegateCommand(OnAddRowCommand);
            SelectClienteCommand = new DelegateCommand(OnSelectClienteCommand);
            _preciosKilatesRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IPreciosKilatesRepository>();
            _compraRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ICompraRepository>();
            _monedaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMonedaRepository>();
            _tipoCambioRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ITipoCambioRepository>();
            _tipoPrecioRepository = VariablesGlobalesForm.Instance.DtoTipoPrecioRepository;
            _codagencia = VariablesGlobalesForm.Instance.Agencia!.Codagencia;
            _itemsSource = new ObservableCollection<DetCompra>();
        }

        private void OnSelectClienteCommand()
        {
            var frmSelectedCliente = new FilterClientesWindow();
            frmSelectedCliente.ShowDialog();
            var filterClientesViewModel = frmSelectedCliente.DataContext as FilterClientesViewModel;
            if (filterClientesViewModel!.SelectedCliente is not null)
            {
                SelectedCliente = filterClientesViewModel.SelectedCliente;
            }
        }

        private async void OnAddRowCommand()
        {
            try
            {
                if (SelectedPrecioKilate is null)
                {
                    return;
                }

                if (Precio.CompareTo(Zero) <= 0 || Peso.CompareTo(Zero) <= 0 || Importe.CompareTo(Zero) <= 0)
                {
                    return;
                }

                var message = new DXMessageBoxService();
                var linea = ItemsSource.Count + 1;

                var valorTipoCambio = Zero;
                var tipoCambio = await _tipoCambioRepository.FindByDateNow();
                if (tipoCambio is not null)
                {
                    valorTipoCambio = (tipoCambio.Tipocambio);
                }


                var detCompra = new DetCompra
                {
                    Importe = HelpersMethods.RedondeoHaciaArriba(Importe),
                    Codagencia = _codagencia,
                    Descripcion = string.Empty,
                    Fecha = Fecha,
                    Kilate = $"{SelectedPrecioKilate.Peso}",
                    Kilshowdoc = $"{SelectedPrecioKilate.Peso} kilate",
                    Linea = linea,
                    Numcompra = NumeroCompra!,
                    Numdescargue = 0,
                    Peso = Peso,
                    Preciok = (Precio)
                };
                ItemsSource.Add(detCompra);
                FnCalcularTotal();
                Precio = Zero;
                Peso = Zero;
                Importe = Zero;
                SelectedPrecioKilate = null;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in OnAddRowCommand");
            }
        }

        private bool _habilitarFormaPago;

        public bool HabilitarFormaPago
        {
            get => _habilitarFormaPago;
            set => SetValue(ref _habilitarFormaPago, value);
        }

        private decimal _montoLocal;

        public decimal MontoLocal
        {
            get => _montoLocal;
            set
            {
                SetValue(ref _montoLocal, value);
                CalcularFormaPago();
            }
        }

        private decimal _montoExtranjero;

        public decimal MontoExtranjero
        {
            get => _montoExtranjero;
            set
            {
                SetValue(ref _montoExtranjero, value);
                CalcularFormaPago();
            }
        }

        private async void CalcularFormaPago()
        {
            try
            {
                if (SelectedMoneda is null)
                {
                    return;
                }

                var param = await VariablesGlobales.Instance.UnityContainer.Resolve<IParametersRepository>()
                    .RecuperarParametros();
                if (param is null)
                {
                    return;
                }

                var valorTipoCambio = Zero;
                var tipoCambio = await _tipoCambioRepository.FindByDateNow();
                if (tipoCambio is not null)
                {
                    valorTipoCambio = (tipoCambio.Tipocambio);
                }


                if (MontoEfectivo.CompareTo(Zero) <= 0)
                {
                    MontoExtranjero = 0;
                    MontoLocal = 0;
                    return;
                }
                if (SelectedMoneda.Codmoneda == param.Cordobas)
                {
                    var totalX = Subtract(MontoEfectivo, MontoLocal);
                    MontoExtranjero = HelpersMethods.RedondeoHaciaArriba(totalX / valorTipoCambio, 4);
                }
                else
                {
                    var totalX = Subtract(MontoEfectivo, MontoExtranjero);
                    MontoLocal = HelpersMethods.RedondeoHaciaArriba(totalX * valorTipoCambio, 4);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public bool IsEfectivo
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                if (!value)
                {
                    MontoEfectivo = Zero;
                }
            }
        }

        public bool IsCheque
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                if (!value)
                {
                    MontoCheque = Zero;
                }
            }
        }

        public bool IsTransferencia
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                if (!value)
                {
                    MontoTransferencia = Zero;
                }
            }
        }

        public bool IsPorPagar
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                if (!value)
                {
                    MontoPorPagar = Zero;
                }
            }
        }

        public bool IsAdelanto
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                if (!value)
                {
                    MontoAdelanto = Zero;
                }
            }
        }

        private decimal _peso = Zero;

        public decimal Peso
        {
            get => (_peso);
            set => SetValue(ref _peso, (value), changedCallback: NotifyImporteChanged());
        }

        private decimal _precio = Zero;

        public decimal Precio
        {
            get => HelpersMethods.RedondeoHaciaArriba(_precio);
            set => SetValue(ref _precio, HelpersMethods.RedondeoHaciaArriba(value),
                changedCallback: NotifyImporteChanged());
        }

        private decimal _importe = Zero;

        public decimal Importe
        {
            get => HelpersMethods.RedondeoHaciaArriba(_importe);
            set => SetValue(ref _importe, HelpersMethods.RedondeoHaciaArriba(value));
        }

        private decimal _total = Zero;

        public decimal Total
        {
            get => (_total);
            set => SetValue(ref _total, (value));
        }

        private decimal _subTotal = Zero;

        public decimal SubTotal
        {
            get => (_subTotal);
            set => SetValue(ref _subTotal, (value));
        }

        public decimal MontoEfectivo
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal MontoCheque
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal MontoTransferencia
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal MontoPorPagar
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal MontoAdelanto
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        private string? _numeroCompra;

        public string? NumeroCompra
        {
            get => _numeroCompra;
            set => SetValue(ref _numeroCompra, value);
        }

        private DetCompra? _selectedDetCompra;

        public DetCompra? SelectedDetCompra
        {
            get => _selectedDetCompra;
            set => SetValue(ref _selectedDetCompra, value);
        }

        private PrecioKilate? _precioKilate;

        public PrecioKilate? SelectedPrecioKilate
        {
            get => _precioKilate;
            set
            {
                if (SetValue(ref _precioKilate, value, changedCallback: NotifyImporteChanged()))
                {
                    if (value is not null)
                    {
                        Precio = (value.Precio);
                    }
                }
            }
        }

        private Action NotifyImporteChanged()
        {
            return () => { Importe = (Peso * Precio); };
        }

        private bool _hiddeButtonAnular;

        public bool HiddeButtonAnular
        {
            get => _hiddeButtonAnular;
            set => SetProperty(ref _hiddeButtonAnular, value, "HiddeButtonAnular");
        }

        private DateTime _fecha = DateTime.Now;

        public DateTime Fecha
        {
            get => _fecha;
            set => SetValue(ref _fecha, value);
        }

        public Cliente? SelectedCliente
        {
            get => GetValue<Cliente>();
            set => SetValue(value);
        }

        public Moneda? SelectedMoneda
        {
            get => GetValue<Moneda>();
            set
            {
                if (SetValue(value))
                {
                    FnCalcularCambioMoneda(value!);
                }
            }
        }

        private void FnCalcularCambioMoneda(Moneda value)
        {
            FnCalcularTotal();
        }

        public DtoTiposPrecios? SelectedTiposPrecios
        {
            get => GetValue<DtoTiposPrecios>();
            set
            {
                if (SetValue(value))
                {
                    FnCalcularTotal();
                }
            }
        }

        public EstadoCompra SelectedEstadoCompra
        {
            get => GetValue<EstadoCompra>();
            set => SetValue(value);
        }

        private ObservableCollection<EstadoCompra> _estadoCompras = new();

        public ObservableCollection<EstadoCompra> EstadoCompras
        {
            get => _estadoCompras;
            set => SetValue(ref _estadoCompras, value);
        }

        private ObservableCollection<DtoTiposPrecios> _tiposPrecios = new();

        public ObservableCollection<DtoTiposPrecios> TiposPrecios
        {
            get => _tiposPrecios;
            set => SetValue(ref _tiposPrecios, value);
        }

        private ObservableCollection<PrecioKilate> _precioKilates = new();
        public ObservableCollection<PrecioKilate> PrecioKilates => _precioKilates;

        private ObservableCollection<DetCompra> _itemsSource;

        public ObservableCollection<DetCompra> ItemsSource
        {
            get => _itemsSource;
            set => SetValue(ref _itemsSource, value);
        }

        private ObservableCollection<Moneda> _monedas = new();
        public ObservableCollection<Moneda> Monedas => _monedas;

        public ICommand AddRowCommand { get; set; }
        public ICommand SelectClienteCommand { get; set; }

        private DtoComprasClientes? _selectedCompra;

        public DtoComprasClientes? SelectedCompra
        {
            get => _selectedCompra;
            set => SetValue(ref _selectedCompra, value);
        }

        public async void LoadValues()
        {
            var kilates = await _preciosKilatesRepository.FindAll();
            NumeroCompra = await _compraRepository.CodigoCompra();
            kilates.ForEach(PrecioKilates.Add);
            ItemsSource = new ObservableCollection<DetCompra>();
            var findAll = await _tipoPrecioRepository.FindAll();
            LinqExtensions.ForEach(findAll, TiposPrecios.Add);
            //TiposPrecios.AddRange(findAll);
            if (TiposPrecios.Count > 0)
            {
                SelectedTiposPrecios = TiposPrecios.FirstOrDefault();
            }

            foreach (var estadoCompra in Enum.GetValues<EstadoCompra>())
            {
                EstadoCompras.Add(estadoCompra);
            }

            SelectedEstadoCompra = EstadoCompra.Vigente;
            var findAllMonedas = await _monedaRepository.FindAll();
            findAllMonedas.ForEach(Monedas.Add);
            if (findAllMonedas.Count > 0)
            {
                SelectedMoneda = findAllMonedas.SingleOrDefault(mo => mo.Default!.Value);
            }

            if (SelectedCompra is null)
            {
                HiddeButtonAnular = false;
                return;
            }

            HiddeButtonAnular = true;
            HelpersMessage.MensajeInformacionResult("Compra", "Se ha pasado una compra a editar");
            var compra = await _compraRepository.FindById(SelectedCompra.Numcompra ?? "0000");
            if (compra is null)
            {
                WinUIMessageBox.Show($"No existe la compra con el código {SelectedCompra.Numcompra}", "Compra");
                return;
            }

            NumeroCompra = compra.Numcompra;
            MontoEfectivo = compra.Efectivo;
            MontoCheque = compra.Cheque;
            MontoTransferencia = compra.Transferencia;
            MontoAdelanto = compra.Adelantos;
            SubTotal = compra.Subtotal ?? Zero;
            Total = compra.Total;
            Fecha = compra.Fecha;
            SelectedCliente = compra.Cliente;
            SelectedEstadoCompra = compra.Codestado;
            SelectedMoneda = findAllMonedas.FirstOrDefault(moneda => moneda.Codmoneda == compra.Codmoneda);
            SelectedTiposPrecios = findAll.FirstOrDefault(precios => precios.IdTipoPrecio == compra.IdTipoPrecio);
            var formPago = await _compraRepository.FindFormaPago(NumeroCompra!);
            if (formPago is not null)
            {
                HabilitarFormaPago = true;
                MontoLocal = formPago.Monto1 ?? Zero;
                MontoExtranjero = formPago.Monto2 ?? Zero;
            }

            foreach (var detCompra in compra.DetCompras)
            {
                ItemsSource.Add(detCompra);
            }
        }

        private async Task<bool> ValidarTotal()
        {
            var sumaTotal = MontoEfectivo + MontoCheque + MontoAdelanto + MontoPorPagar + MontoTransferencia +
                            MontoAdelanto;
            var tipoCambio = await _tipoCambioRepository.FindByDateNow();
            var param = await VariablesGlobales
                .Instance.UnityContainer
                .Resolve<IParametersRepository>()
                .RecuperarParametros();
            if (param is null)
            {
                await XtraMessageBox.ShowAsync("No hay parametros configurados en el sistema");
                return false;
            }

            if (tipoCambio is null)
            {
                await XtraMessageBox.ShowAsync("No hay tipo de cambio ingresado en el sistema");
                return false;
            }

            if (SelectedMoneda is null)
            {
                await XtraMessageBox.ShowAsync("No ha seleccionado una moneda");
                return false;
            }

            var validarMontosFomaPago = false;
            if (HabilitarFormaPago)
            {
                var montoLocalX = MontoLocal;
                var montoDolaresX = MontoExtranjero;
                if (SelectedMoneda.Codmoneda == param.Cordobas)
                {
                    montoDolaresX = HelpersMethods.RedondeoHaciaArriba(MontoExtranjero * tipoCambio.Tipocambio);
                }
                else
                {
                    montoLocalX = HelpersMethods.RedondeoHaciaArriba(MontoLocal / tipoCambio.Tipocambio);
                }

                var suma = Add(montoLocalX, montoDolaresX);
                validarMontosFomaPago = Compare(MontoEfectivo, suma) != 0;
            }

            return Compare(Total, sumaTotal) != 0 ||
                   HabilitarFormaPago && validarMontosFomaPago;
        }

        [Command]
        public async void SaveCompra()
        {
            if (VariablesGlobalesForm.Instance.Usuario is null)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, "Usuario no existe en el sistema");
                return;
            }

            if (SelectedCompra is not null)
            {
                if (VariablesGlobalesForm.Instance.Usuario.Nivel == Nivel.Caja)
                {
                    var ingresarUsuario = new IngresarUsuarioModal();
                    ingresarUsuario.ShowDialog();
                    if (!VariablesGlobalesForm.Instance.PermitirEdicionCompra)
                    {
                        HelpersMessage.DialogWindow("Editar", "No posee los permisos de edicion", MessageBoxButton.OK);
                        return;
                    }
                }
            }

            if (ItemsSource.Count <= 0)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.DetalleCompraVacio);
                return;
            }
            else
            {
                if (ItemsSource.Any(detCompra => string.IsNullOrWhiteSpace(detCompra.Descripcion)))
                {
                    HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo,
                        MensajesCompras.CamposVaciosDetalleCompra);
                    return;
                }
            }

            if (await ValidarTotal())
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.TotalNoValido);
                return;
            }

            if (SelectedCliente is null)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.ClienteVacio);
                return;
            }

            if (SelectedMoneda is null)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.MonedaVacia);
                return;
            }

            if (SelectedTiposPrecios is null)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.TipoPrecioVacio);
                return;
            }

            var result =
                HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo,
                    MensajesCompras.ConfirmarCompra);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            if (MontoEfectivo > 0)
            {
                _mediosDePago.Add("Efectivo");
            }

            if (MontoCheque > 0)
            {
                _mediosDePago.Add("Cheque");
            }

            if (MontoTransferencia > 0)
            {
                _mediosDePago.Add("Transferencia");
            }

            if (MontoPorPagar > 0)
            {
                _mediosDePago.Add("PorPagar");
            }

            if (MontoAdelanto > 0)
            {
                _mediosDePago.Add("Adelanto");
            }

            FormaPago? formaPago = null;
            if (HabilitarFormaPago)
            {
                formaPago = new FormaPago
                {
                    Monto1 = MontoLocal,
                    Monto2 = MontoExtranjero,
                    Total = Total,
                    Fecha = DateTime.Now
                };
            }

            bool save;
            Compra? compra;
            if (SelectedCompra is null)
            {
                compra = new Compra
                {
                    Numcompra = NumeroCompra ?? "",
                    Adelantos = MontoAdelanto,
                    Subtotal = SubTotal,
                    Cheque = MontoCheque,
                    Transferencia = MontoTransferencia,
                    PorCobrar = 0,
                    PorPagar = MontoPorPagar,
                    Codcliente = SelectedCliente.Codcliente,
                    Codestado = EstadoCompra.Vigente,
                    Usuario = VariablesGlobalesForm.Instance.Usuario.Codoperador,
                    Hora = Fecha.ToShortTimeString(),
                    FormaPago = string.Join(", ", _mediosDePago),
                    Codmoneda = SelectedMoneda.Codmoneda,
                    Descuento = SelectedTiposPrecios.Precio,
                    Dgnumdes = null,
                    Efectivo = MontoEfectivo,
                    Total = Total,
                    Fecha = Fecha,
                    Peso = ItemsSource.Sum(detCompra => detCompra.Peso),
                    IdTipoPrecio = SelectedTiposPrecios.IdTipoPrecio,
                    Firma = _firma
                };
                save = await _compraRepository.Create(compra, ItemsSource.ToList(), formaPago);
            }
            else
            {
                compra = await _compraRepository.FindById(SelectedCompra.Numcompra!);
                if (compra is null)
                {
                    HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.NoExisteCompra);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(_firma))
                {
                    compra.Firma = _firma;
                }
                compra.Adelantos = MontoAdelanto;
                compra.Subtotal = SubTotal;
                compra.Cheque = MontoCheque;
                compra.Transferencia = MontoTransferencia;
                compra.PorCobrar = 0;
                compra.PorPagar = MontoPorPagar;
                compra.Codcliente = SelectedCliente.Codcliente;
                compra.Codestado = EstadoCompra.Vigente;
                compra.Usuario = VariablesGlobalesForm.Instance.Usuario.Codoperador;
                compra.Hora = Fecha.ToShortTimeString();
                compra.FormaPago = string.Join(", ", _mediosDePago);
                compra.Codmoneda = SelectedMoneda.Codmoneda;
                compra.Descuento = SelectedTiposPrecios.Precio;
                compra.Efectivo = MontoEfectivo;
                compra.Total = Total;
                compra.Peso = ItemsSource.Sum(detCompra => detCompra.Peso);
                compra.IdTipoPrecio = SelectedTiposPrecios.IdTipoPrecio;
                save = await _compraRepository.UpdateByDetaCompra(compra, ItemsSource.ToList(), formaPago);
                NumeroCompra = SelectedCompra.Numcompra;
            }

            if (save)
            {
                HelpersMessage.MensajeInformacionResult("Guardar", "Se ha guardado la compra con exito");
                result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo,
                    MensajesCompras.ImprimirCompra);
                if (result == MessageBoxResult.OK)
                {
                    var findCompra = await _compraRepository.DetalleCompraImprimir(NumeroCompra ?? "");
                    _logger.Info($"Numero de compra {NumeroCompra} - Cantidad de datos: {findCompra.Count}");
                    if (findCompra.Count > 0)
                    {
                        foreach (var detalleCompra in findCompra)
                        {
                            _logger.Info($"Datos: {detalleCompra.Numcompra} - {detalleCompra.Nocontrato}");
                        }
                    }

                    //Reporte Anexo
                    var reporteAnexo = new ReporteAnexo();
                    reporteAnexo.DataSource = findCompra;
                    if (!string.IsNullOrWhiteSpace(compra.Firma))
                    {
                        var image = HelpersMethods.LoadSigImage(compra.Firma, NumeroCompra ?? "");
                        //reporteAnexo.imgFirma.ImageSource = new ImageSource(image);
                    }
                    
                    HelpersMethods.LoadReport(reporteAnexo);
                    //Reporte Contrato Contra Venta
                    var reporteContrantoContraVenta = new ReporteContratoContraVenta();
                    reporteContrantoContraVenta.DataSource = findCompra;
                    HelpersMethods.LoadReport(reporteContrantoContraVenta);
                    //Reporte Contrato Prestamo
                    var reporteContrantoPrestamo = new ReporteContrantoPrestamo();
                    reporteContrantoPrestamo.DataSource = findCompra;
                    HelpersMethods.LoadReport(reporteContrantoPrestamo);
                    //Reporte Comprobante de compra
                    var reporteComprobanteCompra = new ReporteComprobanteCompra();
                    reporteComprobanteCompra.Parameters["parNumcompra"].Value = NumeroCompra;
                    HelpersMethods.LoadReport(reporteComprobanteCompra);
                }

                CloseAction?.Invoke();
            }
            else
            {
                HelpersMessage.MensajeErroResult("Guardar", _compraRepository.ErrorSms!);
            }
        }

        [Command]
        public async void AnularCompra()
        {
            var result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo,
                MensajesCompras.AnularCompraConfirmacion);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            var user = VariablesGlobalesForm.Instance.Usuario!;
            VariablesGlobales.Instance.Usuario = VariablesGlobalesForm.Instance.Usuario!;
            if (user.Nivel == Nivel.Caja)
            {
                var ingresarUsuario = new IngresarUsuarioModal();
                ingresarUsuario.ShowDialog();
                if (!VariablesGlobalesForm.Instance.PermitirEdicionCompra)
                {
                    HelpersMessage.DialogWindow("Editar", "No posee los permisos de edicion", MessageBoxButton.OK);
                    return;
                }
            }

            if (SelectedCompra is null)
            {
                return;
            }

            var anularCompra = await _compraRepository.AnularCompra(SelectedCompra.Numcompra!);
            if (anularCompra)
            {
                HelpersMessage.MensajeInformacionResult("Anular", "Se ha anulado la compra con exito");
                CloseAction?.Invoke();
            }
            else
            {
                HelpersMessage.MensajeErroResult("Error",
                    $"Se produjo el siguiente error: {_compraRepository.ErrorSms}");
            }
        }

        [Command]
        public void CapturarFirma()
        {
            var frmCapturarFirma = new FormCapturarFirma();
            var resultDialog = frmCapturarFirma.ShowDialog() ?? false;
            if (resultDialog)
            {
                _firma= frmCapturarFirma.SigString ?? string.Empty;
            }
        }

        [Command]
        public void RemoveRow()
        {
            if (ItemsSource.Count <= 0) return;
            if (SelectedDetCompra is null) return;
            if (ItemsSource.Remove(SelectedDetCompra))
            {
                FnCalcularTotal();
            }
        }

        [Command]
        public void RemoveCliente()
        {
            SelectedCliente = null;
        }


        [Command]
        public void CellValueChangedDetaCompra(CellValueChangedArgs args)
        {
            var argsItem = args.Item as DetCompra;
            if (argsItem != null)
            {
                argsItem.Importe = Multiply(argsItem.Peso, argsItem.Preciok);
                FnCalcularTotal();
            }
        }

        [Command]
        public void ValidateCellDetaCompra(RowCellValidationArgs args)
        {
            var newValue = args.NewValue.ToString();
            switch (args.FieldName)
            {
                case nameof(DetCompra.Descripcion):
                {
                    if (string.IsNullOrWhiteSpace(newValue))
                    {
                        args.Result = new ValidationErrorInfo("Debe especificar una descripcion para continuar",
                            ValidationErrorType.Critical);
                    }

                    break;
                }
                case nameof(DetCompra.Preciok):
                    if (string.IsNullOrWhiteSpace(newValue))
                    {
                        args.Result = new ValidationErrorInfo("Debe especificar un precio para continuar",
                            ValidationErrorType.Critical);
                        return;
                    }

                    var precio = Parse(newValue);
                    if (precio.CompareTo(Zero) <= 0)
                    {
                        args.Result =
                            new ValidationErrorInfo("No puede ser cero el precio", ValidationErrorType.Critical);
                    }

                    break;
                case nameof(DetCompra.Peso):
                    if (string.IsNullOrWhiteSpace(newValue))
                    {
                        args.Result = new ValidationErrorInfo("Debe especificar un peso para continuar",
                            ValidationErrorType.Critical);
                        return;
                    }

                    var peso = Parse(newValue);
                    if (peso.CompareTo(Zero) <= 0)
                    {
                        args.Result =
                            new ValidationErrorInfo("No puede ser cero el peso", ValidationErrorType.Critical);
                    }

                    break;
            }
        }

        private async void FnCalcularTotal()
        {
            if (SelectedMoneda is null)
            {
                return;
            }

            if (SelectedTiposPrecios is null)
            {
                HelpersMessage.MensajeErroResult("Compra", "No hay tipo de precios especificados");
                return;
            }

            try
            {
                var tipoCambio = await _tipoCambioRepository.FindByDateNow();
                var tipoCambioValue = (tipoCambio?.Tipocambio ?? Zero);
                var parametros = VariablesGlobalesForm.Instance.Parametros;
                if (parametros is null)
                {
                    HelpersMessage.MensajeErroResult("Compra", "No hay parametros configurados en el sistema");
                    return;
                }

                var precio = SelectedTiposPrecios.Precio ?? Zero;
                if (ItemsSource.Count > 0)
                {
                    foreach (var detCompra in ItemsSource)
                    {
                        var findPrecio =
                            await _preciosKilatesRepository.FindByPeso(Convert.ToDecimal(detCompra.Kilate));
                        if (findPrecio == null) continue;
                        var precioHaciaArriba = HelpersMethods.RedondeoHaciaArriba(findPrecio.Precio);
                        var tempPrecio = precioHaciaArriba / precio;
                        var tempImporte = tempPrecio * detCompra.Peso;

                        if (parametros.Cordobas!.Value == SelectedMoneda.Codmoneda)
                        {
                            tempPrecio = HelpersMethods.RedondeoHaciaArriba(tempPrecio * tipoCambioValue);
                            tempImporte = HelpersMethods.RedondeoHaciaArriba(tempPrecio * detCompra.Peso);
                        }

                        detCompra.Preciok = HelpersMethods.RedondeoHaciaArriba(tempPrecio);
                        detCompra.Importe = HelpersMethods.RedondeoHaciaArriba(tempImporte);
                    }

                    SubTotal = HelpersMethods.RedondeoHaciaArriba(ItemsSource.Sum(compra => compra.Importe ?? Zero) *
                                                                  precio);
                    Total = HelpersMethods.RedondeoHaciaArriba(ItemsSource.Sum(compra => compra.Importe ?? Zero));
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error al calcular el total");
            }
        }
    }
}