using System;
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
using System.Windows.Forms;
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
using SistemaOro.Forms.Views.Compras;

namespace SistemaOro.Forms.ViewModels.Compras
{
    public class FormComprasViewModel : BaseViewModel
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IPreciosKilatesRepository preciosKilatesRepository;
        private readonly ICompraRepository compraRepository;
        private readonly IDtoTipoPrecioRepository tipoPrecioRepository;
        private readonly IMonedaRepository monedaRepository;
        private readonly ITipoCambioRepository tipoCambioRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly string codagencia;
        private string firma = string.Empty;
        private readonly List<string> _mediosDePago = new();
        private Id? parametros;

        public FormComprasViewModel()
        {
            Title = "Realizar Compra";
            unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
            AddRowCommand = new DelegateCommand(OnAddRowCommand);
            SelectClienteCommand = new DelegateCommand(OnSelectClienteCommand);
            preciosKilatesRepository = unitOfWork.PreciosKilatesRepository;
            compraRepository = unitOfWork.CompraRepository;
            monedaRepository = unitOfWork.MonedaRepository;
            tipoCambioRepository = unitOfWork.TipoCambioRepository;
            tipoPrecioRepository = new DtoTipoPrecioRepository();
            codagencia = VariablesGlobalesForm.Instance.Agencia!.Codagencia;
            tipoCambio = tipoCambioRepository.FindByDateNow();
            Task.Run(async () =>
            {
                var parametersRepository = unitOfWork.ParametersRepository;
                parametros = await parametersRepository.RecuperarParametros();
            });
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

        private void OnAddRowCommand()
        {
            try
            {
                if (SelectedPrecioKilate is null)
                {
                    return;
                }

                if (Precio.CompareTo(Zero) <= 0 || Peso.CompareTo(Zero) <= 0 || Importe.CompareTo(Zero) <= 0 ||
                    Lectura <= 0)
                {
                    HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.CamposVacios);
                    return;
                }

                var linea = ItemsSource.Count + 1;

                var detCompra = new DetCompra
                {
                    Importe = HelpersMethods.RedondeoHaciaArriba(Importe),
                    Codagencia = codagencia,
                    Descripcion = string.Empty,
                    Fecha = Fecha,
                    Kilate = $"{SelectedPrecioKilate.Peso}",
                    Kilshowdoc = $"{SelectedPrecioKilate.Peso} kilate",
                    Linea = linea,
                    Numcompra = NumeroCompra!,
                    Numdescargue = 0,
                    Peso = Peso,
                    Preciok = Precio,
                    Lectura = Lectura
                };
                ItemsSource.Add(detCompra);
                FnCalcularTotal();
                Precio = Zero;
                Peso = Zero;
                Importe = Zero;
                Lectura = 0;
                SelectedPrecioKilate = null;
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in OnAddRowCommand");
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
            set => SetValue(ref _montoLocal, value, CalcularFormaPago);
        }

        private decimal _montoExtranjero;

        public decimal MontoExtranjero
        {
            get => _montoExtranjero;
            set => SetValue(ref _montoExtranjero, value, CalcularFormaPago);
        }

        private void CalcularFormaPago()
        {
            try
            {
                if (SelectedMoneda is null)
                {
                    return;
                }


                if (parametros is null)
                {
                    return;
                }

                var valorTipoCambio = 0m;
                tipoCambio = tipoCambioRepository.FindByDateNow();
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

                if (SelectedMoneda.Codmoneda == parametros.Cordobas)
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
                logger.Error(e);
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

        private int _lectura;

        public int Lectura
        {
            get => _lectura;
            set => SetValue(ref _lectura, value);
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

        private Moneda? _selectedMoneda;

        public Moneda? SelectedMoneda
        {
            get => _selectedMoneda;
            set => SetValue(ref _selectedMoneda, value, FnCalcularTotal);
        }

        private DtoTiposPrecios? _selectedTiposPrecios;
        public DtoTiposPrecios? SelectedTiposPrecios
        {
            get => _selectedTiposPrecios;
            set => SetValue(ref _selectedTiposPrecios, value, FnCalcularTotal);
        }

        private EstadoCompra _selectedEstadoCompra;

        public EstadoCompra SelectedEstadoCompra
        {
            get => _selectedEstadoCompra;
            set => SetValue(ref _selectedEstadoCompra, value);
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

        public ObservableCollection<PrecioKilate> PrecioKilates
        {
            get => _precioKilates;
            set => SetValue(ref _precioKilates, value);
        }

        private ObservableCollection<DetCompra> _itemsSource = new();

        public ObservableCollection<DetCompra> ItemsSource
        {
            get => _itemsSource;
            set => SetValue(ref _itemsSource, value);
        }

        private ObservableCollection<Moneda> _monedas = new();

        public ObservableCollection<Moneda> Monedas
        {
            get => _monedas;
            set => SetValue(ref _monedas, value);
        }

        public ICommand AddRowCommand { get; set; }
        public ICommand SelectClienteCommand { get; set; }

        private ComprasCliente? _selectedCompra;
        private byte[]? _huella;
        private Data.Entities.TipoCambio? tipoCambio;

        public ComprasCliente? SelectedCompra
        {
            get => _selectedCompra;
            set => SetValue(ref _selectedCompra, value);
        }

        public async Task LoadPrecios()
        {
            var kilates = await preciosKilatesRepository.FindAll();
            PrecioKilates = new ObservableCollection<PrecioKilate>(kilates);
        }

        public async void LoadValues()
        {
            IsLoading = true;
            try
            {
                VariablesGlobalesForm.Instance.MainViewModel.RbnEditarCompraVisible = true;
                NumeroCompra = await compraRepository.CodigoCompra();
                var findAll = await tipoPrecioRepository.FindAll();
                TiposPrecios.Clear();
                foreach (var dtoTiposPreciose in findAll)
                {
                    TiposPrecios.Add(dtoTiposPreciose);
                }

                //TiposPrecios.AddRange(findAll);
                if (TiposPrecios.Count > 0)
                {
                    SelectedTiposPrecios = TiposPrecios.FirstOrDefault();
                }

                EstadoCompras.Clear();
                foreach (var estadoCompra in Enum.GetValues<EstadoCompra>())
                {
                    EstadoCompras.Add(estadoCompra);
                }

                SelectedEstadoCompra = EstadoCompra.Vigente;
                var findAllMonedas = await monedaRepository.FindAll();
                Monedas.Clear();
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
                //HelpersMessage.MensajeInformacionResult("Compra", "Se ha pasado una compra a editar");
                var compra = await compraRepository.FindById(SelectedCompra.Numcompra ?? "0000");
                if (compra is null)
                {
                    WinUIMessageBox.Show($"No existe la compra con el código {SelectedCompra.Numcompra}", "Compra");
                    return;
                }

                SelectedCompra.Huella = compra.Huella;
                NumeroCompra = compra.Numcompra;
                MontoEfectivo = compra.Efectivo;
                MontoCheque = compra.Cheque;
                MontoTransferencia = compra.Transferencia;
                if (compra.Transferencia > decimal.Zero)
                {
                    IsTransferencia = true;
                }

                if (compra.Efectivo > decimal.Zero)
                {
                    IsEfectivo = true;
                }

                MontoAdelanto = compra.Adelantos;
                SubTotal = compra.Subtotal ?? Zero;
                Total = compra.Total;
                Fecha = compra.Fecha;
                SelectedCliente = compra.Cliente;
                SelectedEstadoCompra = compra.Codestado;
                SelectedMoneda = findAllMonedas.FirstOrDefault(moneda => moneda.Codmoneda == compra.Codmoneda);
                SelectedTiposPrecios = findAll.FirstOrDefault(precios => precios.IdTipoPrecio == compra.IdTipoPrecio);
                var formPago = compraRepository.FindFormaPago(NumeroCompra!);
                if (formPago is not null)
                {
                    HabilitarFormaPago = true;
                    MontoLocal = formPago.Monto1 ?? Zero;
                    MontoExtranjero = formPago.Monto2 ?? Zero;
                }

                var detCompras = await compraRepository.FindDetaCompra(compra.Numcompra);
                ItemsSource.Clear();
                foreach (var detCompra in detCompras)
                {
                    ItemsSource.Add(detCompra);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error LoadValues");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ValidarTotal()
        {
            var sumaTotal = MontoEfectivo + MontoTransferencia;
            if (parametros is null)
            {
                HelpersMessage.MensajeErroResult("Error", "No hay parametros configurados en el sistema");
                return true;
            }

            if (tipoCambio is null)
            {
                HelpersMessage.MensajeErroResult("Error", "No hay tipo de cambio ingresado en el sistema");
                return true;
            }

            if (SelectedMoneda is null)
            {
                HelpersMessage.MensajeErroResult("Error", "No ha seleccionado una moneda");
                return true;
            }

            var validarMontosFomaPago = false;
            if (!HabilitarFormaPago)
                return Compare(Total, sumaTotal) != 0 || HabilitarFormaPago && validarMontosFomaPago;

            var montoLocalX = MontoLocal;
            var montoDolaresX = MontoExtranjero;
            if (SelectedMoneda.Codmoneda == parametros.Cordobas)
            {
                montoDolaresX = HelpersMethods.RedondeoHaciaArriba(MontoExtranjero * tipoCambio.Tipocambio);
            }
            else
            {
                montoLocalX = HelpersMethods.RedondeoHaciaArriba(MontoLocal / tipoCambio.Tipocambio);
            }

            var suma = HelpersMethods.RedondeoHaciaArriba(Add(montoLocalX, montoDolaresX));
            validarMontosFomaPago = Compare(MontoEfectivo, suma) != 0;
            logger.Info($"Validando total: {sumaTotal} - {Total}; Forma de pago: {HabilitarFormaPago}, monto {suma}");
            return Compare(Total, sumaTotal) != 0 || (HabilitarFormaPago && validarMontosFomaPago);
        }

        [Command]
        public async void SaveCompra()
        {
            try
            {
                if (VariablesGlobalesForm.Instance.Usuario is null)
                {
                    HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo,
                        "Usuario no existe en el sistema");
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
                            HelpersMessage.DialogWindow("Editar", "No posee los permisos de edicion",
                                MessageBoxButton.OK);
                            return;
                        }
                    }
                }

                if (ItemsSource.Count <= 0)
                {
                    HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo,
                        MensajesCompras.DetalleCompraVacio);
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

                if (ValidarTotal())
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

                var result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo,
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
                await unitOfWork.BeginTransactionAsync();
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
                        Firma = firma,
                        Huella = _huella
                    };
                    save = await compraRepository.Create(compra, ItemsSource.ToList(), formaPago);
                }
                else
                {
                    compra = await compraRepository.FindById(SelectedCompra.Numcompra!);
                    if (compra is null)
                    {
                        HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo,
                            MensajesCompras.NoExisteCompra);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(firma))
                    {
                        compra.Firma = firma;
                    }

                    if (_huella is not null && _huella.Length > 0)
                    {
                        compra.Huella = _huella;
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
                    save = await compraRepository.UpdateByDetaCompra(compra, ItemsSource.ToList(), formaPago);
                    NumeroCompra = SelectedCompra.Numcompra;
                }

                if (!save)
                {
                    HelpersMessage.MensajeErroResult("Error", compraRepository.ErrorSms ?? "No fue posible guardar la compra, revisar Log o solicitar asistencia");
                    return;
                }
                try
                {
                    await unitOfWork.CommitAsync();
                    HelpersMessage.MensajeInformacionResult("Guardar", "Se ha guardado la compra con éxito");
                    result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo,
                        MensajesCompras.ImprimirCompra);
                    if (result == MessageBoxResult.OK)
                    {
                        var findCompra = await compraRepository.DetalleCompraImprimir(NumeroCompra ?? "");
                        logger.Info($"Numero de compra {NumeroCompra} - Cantidad de datos: {findCompra.Count}");
                        if (findCompra.Count > 0)
                        {
                            foreach (var detalleCompra in findCompra)
                            {
                                logger.Info($"Datos: {detalleCompra.Numcompra} - {detalleCompra.Nocontrato}");
                            }
                        }

                        HelpersMethods.ImprimirReportesCompra(compra, findCompra);
                    }

                    CloseAction?.Invoke();
                }
                catch (Exception e)
                {
                    await unitOfWork.RollbackAsync();
                    logger.Error(e, "Error al guardar la compra, realizando el RollBack");
                }
            }
            catch (Exception e)
            {
                HelpersMessage.MensajeErroResult("Error", $"Error al guardar la compra: {e.Message}");
                logger.Error(e, "Error al guardar la compra");
            }
        }

        public bool NuevaCompra()
        {
            return ItemsSource.Count <= 0;
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
                HelpersMessage.DialogWindow("Anular", "No hay datos de compra a anular, seleccione una para proceder",
                    MessageBoxButton.OK).ShowDialog();
                return;
            }

            var anularCompra = await compraRepository.AnularCompra(SelectedCompra.Numcompra!);
            if (anularCompra)
            {
                HelpersMessage.MensajeInformacionResult("Anular", "Se ha anulado la compra con exito");
                CloseAction?.Invoke();
            }
            else
            {
                HelpersMessage.MensajeErroResult("Error",
                    $"Se produjo el siguiente error: {compraRepository.ErrorSms}");
            }
        }

        [Command]
        public void CapturarHuella()
        {
            var frmCapturarHuella = new FormCapturarHuella();
            if (SelectedCompra is not null && SelectedCompra.Huella is not null && SelectedCompra.Huella.Length > 0)
            {
                frmCapturarHuella.ImageHuella = SelectedCompra.Huella;
            }

            var resultDialog = frmCapturarHuella.ShowDialog() ?? false;
            if (resultDialog)
            {
                _huella = frmCapturarHuella.ImageHuella;
            }
        }

        [Command]
        public void CapturarFirma()
        {
            var frmCapturarFirma = new FormCapturarFirma();
            var resultDialog = frmCapturarFirma.ShowDialog() ?? false;
            if (resultDialog)
            {
                firma = frmCapturarFirma.SigString ?? string.Empty;
            }
        }

        [Command]
        public void DevolucionCompra()
        {
            var frmDevolucion = new FormDevolucionCompra();
            frmDevolucion.ShowDialog();
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

        private void FnCalcularTotal()
        {
            try
            {
                if (SelectedMoneda is null)
                {
                    return;
                }

                if (SelectedTiposPrecios is null)
                {
                    //  HelpersMessage.MensajeErroResult("Compra", "No hay tipo de precios especificados");
                    return;
                }

                tipoCambio = tipoCambioRepository.FindByDateNow();
                var tipoCambioValue = tipoCambio?.Tipocambio ?? 0m;
                if (parametros is null)
                {
                    HelpersMessage.MensajeErroResult("Compra", "No hay parametros configurados en el sistema");
                    return;
                }

                var precio = SelectedTiposPrecios.Precio ?? Zero;

                if (ItemsSource.Count<=0)
                {
                    return;
                }
                foreach (var detCompra in ItemsSource)
                {
                    var findPrecio = preciosKilatesRepository.FindByPeso(Convert.ToDecimal(detCompra.Kilate));
                    if (findPrecio == null) continue;
                    var precioHaciaArriba = HelpersMethods.RedondeoHaciaArriba(findPrecio.Precio);
                    var tempPrecio = precioHaciaArriba / precio;
                    var tempImporte = tempPrecio * detCompra.Peso;
                    tempPrecio = HelpersMethods.RedondeoHaciaArriba(tempPrecio, 2);
                    if (parametros.Cordobas!.Value == SelectedMoneda.Codmoneda)
                    {
                        tempPrecio *= tipoCambioValue;
                        tempPrecio = HelpersMethods.RedondeoHaciaArriba(tempPrecio, 2);
                        tempImporte = tempPrecio * detCompra.Peso;
                    }

                    detCompra.Preciok = tempPrecio;
                    detCompra.Importe = HelpersMethods.RedondeoHaciaArriba(tempImporte, 2);
                }

                SubTotal = HelpersMethods.RedondeoHaciaArriba(ItemsSource.Sum(compra => compra.Importe ?? Zero) *
                                                              precio);
                Total = HelpersMethods.RedondeoHaciaArriba(ItemsSource.Sum(compra => compra.Importe ?? Zero));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error al calcular el total");
            }
        }

        public void Unloaded()
        {
            VariablesGlobalesForm.Instance.MainViewModel.RbnEditarCompraVisible = false;
        }
    }
}