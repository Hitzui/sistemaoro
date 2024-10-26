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
using DevExpress.XtraEditors;
using SistemaOro.Forms.Models;
using SistemaOro.Forms.Repository;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using SistemaOro.Forms.ViewModels.Clientes;
using SistemaOro.Forms.Views.Clientes;
using static System.Decimal;
using System.Collections.Generic;
using DevExpress.Mvvm.Native;
using SistemaOro.Forms.Views.Reportes;
using SistemaOro.Forms.Views.Reportes.Compras;
using DevExpress.XtraReports.UI;
using System.Windows.Forms;
using SistemaOro.Forms.Views.Compras;
using SistemaOro.Data.Dto;
using SistemaOro.Forms.Views;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;

namespace SistemaOro.Forms.ViewModels.Compras
{
    public class FormComprasViewModel : BaseViewModel
    {
        private readonly IPreciosKilatesRepository _preciosKilatesRepository;
        private readonly ICompraRepository _compraRepository;
        private readonly IDtoTipoPrecioRepository _tipoPrecioRepository;
        private readonly IMonedaRepository _monedaRepository;
        private string _codagencia;
        private List<string> _mediosDePago = new();
        public FormComprasViewModel()
        {
            Title = "Realizar Compra";
            AddRowCommand = new DelegateCommand(OnAddRowCommand);
            SelectClienteCommand = new DelegateCommand(OnSelectClienteCommand);
            _preciosKilatesRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IPreciosKilatesRepository>();
            _compraRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ICompraRepository>();
            _monedaRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMonedaRepository>();
            _tipoPrecioRepository = VariablesGlobalesForm.Instance.DtoTipoPrecioRepository;
            _codagencia = VariablesGlobalesForm.Instance.Agencia!.Codagencia;
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
            if (SelectedPrecioKilate is null)
            {
                return;
            }

            if (Precio.CompareTo(Zero) <= 0 || Peso.CompareTo(Zero) <= 0 || Importe.CompareTo(Zero) <= 0)
            {
                return;
            }

            var linea = ItemsSource.Count + 1;
            if (ItemsSource.Count > 0)
            {
                if (ItemsSource.Any(compra => compra.Kilate.Equals(SelectedPrecioKilate.DescKilate)))
                {
                    var message = new DXMessageBoxService();
                    message.ShowMessage("Ya está ingresando el Quilate seleccionado", "Agregar");
                    return;
                }
            }

            var detCompra = new DetCompra
            {
                Importe = Importe,
                Codagencia = _codagencia,
                Descripcion = "",
                Fecha = Fecha,
                Kilate = SelectedPrecioKilate.DescKilate,
                Kilshowdoc = $"{SelectedPrecioKilate.KilatePeso} kilate",
                Linea = linea,
                Numcompra = NumeroCompra!,
                Numdescargue = 0,
                Peso = Peso,
                Preciok = Precio
            };
            ItemsSource.Add(detCompra);
            FnCalcularTotal();
            Precio = Zero;
            Peso = Zero;
            Importe = Zero;
            SelectedPrecioKilate = null;
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
            get => _peso;
            set => SetValue(ref _peso, value, changedCallback: NotifyImporteChanged());
        }

        private decimal _precio = Zero;

        public decimal Precio
        {
            get => _precio;
            set => SetValue(ref _precio, value, changedCallback: NotifyImporteChanged());
        }

        private decimal _importe = Zero;

        public decimal Importe
        {
            get => _importe;
            set => SetValue(ref _importe, value);
        }

        private decimal _total = Zero;

        public decimal Total
        {
            get => _total;
            set
            {
                var t = Math.Floor(value * 100) / 100;
                SetValue(ref _total, t);
            }
        }

        private decimal _subTotal = Zero;
        public decimal SubTotal
        {
            get => _subTotal;
            set => SetValue(ref _subTotal, value);
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
                if (value is not null)
                {
                    Precio = value.PrecioKilate1;
                }

                SetValue(ref _precioKilate, value, changedCallback: NotifyImporteChanged());
            }
        }

        private Action NotifyImporteChanged()
        {
            return () => { Importe = Multiply(_peso, Precio); };
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
            set => SetValue(value);
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

        public ObservableCollection<DetCompra> ItemsSource
        {
            get => GetValue<ObservableCollection<DetCompra>>();
            set => SetValue(value);
        }

        private ObservableCollection<Moneda> _monedas = new();
        public ObservableCollection<Moneda> Monedas => _monedas;

        public ICommand AddRowCommand { get; set; }
        public ICommand SelectClienteCommand { get; set; }

        private DtoComprasClientes? _selectedCompra;
        public DtoComprasClientes? SelectedCompra
        {
            get=>_selectedCompra; 
            set=>SetValue(ref _selectedCompra, value);
        }

        public async void LoadValues()
        {
            var kilates = await _preciosKilatesRepository.FindAll();
            NumeroCompra = await _compraRepository.CodigoCompra();
            kilates.ForEach(PrecioKilates.Add);
            ItemsSource = new ObservableCollection<DetCompra>();
            var findAll = await _tipoPrecioRepository.FindAll();
            findAll.ForEach(TiposPrecios.Add);
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

            if (SelectedCompra is null) return;
            HelpersMessage.MensajeInformacionResult("Compra", "Se ha pasado una compra a editar");
            var compra = await _compraRepository.FindById(SelectedCompra.Numcompra ?? "0000");
            if (compra is null)
            {
                WinUIMessageBox.Show($"No existe la compra con el código {SelectedCompra.Numcompra}", "Compra");
                return;
            }
            NumeroCompra = compra.Numcompra;
            MontoEfectivo = compra.Efectivo;
            MontoCheque=compra.Cheque;
            MontoTransferencia=compra.Transferencia;
            MontoAdelanto = compra.Adelantos;
            SubTotal = compra.Subtotal ?? decimal.Zero;
            Total = compra.Total;
            Fecha = compra.Fecha;
            SelectedCliente = compra.Cliente;
            SelectedEstadoCompra = compra.Codestado;
            SelectedMoneda = findAllMonedas.FirstOrDefault(moneda => moneda.Codmoneda == compra.Codmoneda);
            SelectedTiposPrecios = findAll.FirstOrDefault(precios => precios.IdTipoPrecio == compra.IdTipoPrecio);
            foreach (var detCompra in compra.DetCompras)
            {
                ItemsSource.Add(detCompra);
            }
        }

        private bool ValidarTotal()
        {
            var sumaTotal = MontoEfectivo + MontoCheque + MontoAdelanto + MontoPorPagar + MontoTransferencia + MontoAdelanto;
            return Compare(Total, sumaTotal) != 0;
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
                if (VariablesGlobalesForm.Instance.Usuario.Nivel ==Nivel.Caja)
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
                    HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.CamposVaciosDetalleCompra);
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
            var result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo, MensajesCompras.ConfirmarCompra);
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
            
            bool save;
            if (SelectedCompra is null)
            {
                var compra = new Compra
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
                    IdTipoPrecio = SelectedTiposPrecios.IdTipoPrecio
                };
                save=   await _compraRepository.Create(compra, ItemsSource.ToList()); 
            }
            else
            {
                var findCompra = await _compraRepository.FindById(SelectedCompra.Numcompra!);
                findCompra.Adelantos = MontoAdelanto;
                findCompra.Subtotal = SubTotal;
                findCompra.Cheque = MontoCheque;
                findCompra.Transferencia = MontoTransferencia;
                findCompra.PorCobrar = 0;
                findCompra.PorPagar = MontoPorPagar;
                findCompra.Codcliente = SelectedCliente.Codcliente;
                findCompra.Codestado = EstadoCompra.Vigente;
                findCompra.Usuario = VariablesGlobalesForm.Instance.Usuario.Codoperador;
                findCompra.Hora = Fecha.ToShortTimeString();
                findCompra.FormaPago = string.Join(", ", _mediosDePago);
                findCompra.Codmoneda = SelectedMoneda.Codmoneda;
                findCompra.Descuento = SelectedTiposPrecios.Precio;
                findCompra.Efectivo = MontoEfectivo;
                findCompra.Total = Total;
                findCompra.Peso = ItemsSource.Sum(detCompra => detCompra.Peso);
                findCompra.IdTipoPrecio = SelectedTiposPrecios.IdTipoPrecio;
                save = await _compraRepository.UpdateByDetaCompra(findCompra, ItemsSource.ToList());
                NumeroCompra = SelectedCompra.Numcompra;
            }
            if (save)
            {
                HelpersMessage.MensajeInformacionResult("Guardar", "Se ha guardado la compra con exito");
                result = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo, MensajesCompras.ImprimirCompra);
                if (result == MessageBoxResult.OK)
                {
                    var findCompra = await _compraRepository.DetalleCompraImprimir(NumeroCompra);
                    //Reporte Anexo
                    var reporteAnexo = new ReporteAnexo();
                    reporteAnexo.DataSource=findCompra;
                    HelpersMethods.LoadReport(reporteAnexo);
                    //Reporte Contrato Contra Venta
                    var reporteContrantoContraVenta = new ReporteContratoContraVenta();
                    reporteContrantoContraVenta.DataSource=findCompra;
                    HelpersMethods.LoadReport(reporteContrantoContraVenta);
                    //Reporte Contrato Prestamo
                    var reporteContrantoPrestamo = new ReporteContrantoPrestamo();
                    reporteContrantoPrestamo.DataSource=findCompra;
                    HelpersMethods.LoadReport(reporteContrantoPrestamo);
                }

                CloseAction?.Invoke();
            }
            else
            {
                HelpersMessage.MensajeErroResult("Guardar", _compraRepository.ErrorSms!);
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
                            args.Result = new ValidationErrorInfo("Debe especificar una descripcion para continuar", ValidationErrorType.Critical);
                        }

                        break;
                    }
                case nameof(DetCompra.Preciok):
                    if (string.IsNullOrWhiteSpace(newValue))
                    {
                        args.Result = new ValidationErrorInfo("Debe especificar un precio para continuar", ValidationErrorType.Critical);
                        return;
                    }

                    var precio = Parse(newValue);
                    if (precio.CompareTo(Zero) <= 0)
                    {
                        args.Result = new ValidationErrorInfo("No puede ser cero el precio", ValidationErrorType.Critical);
                    }

                    break;
                case nameof(DetCompra.Peso):
                    if (string.IsNullOrWhiteSpace(newValue))
                    {
                        args.Result = new ValidationErrorInfo("Debe especificar un peso para continuar", ValidationErrorType.Critical);
                        return;
                    }

                    var peso = Parse(newValue);
                    if (peso.CompareTo(Zero) <= 0)
                    {
                        args.Result = new ValidationErrorInfo("No puede ser cero el peso", ValidationErrorType.Critical);
                    }

                    break;
            }
        }

        private void FnCalcularTotal()
        {
            SubTotal = ItemsSource.Count > 0 ? ItemsSource.Sum(compra => compra.Importe)!.Value : Zero;
            Total = SelectedTiposPrecios is not null ? Divide(SubTotal, SelectedTiposPrecios.Precio!.Value) : SubTotal;
        }
    }
}