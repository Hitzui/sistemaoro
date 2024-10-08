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
using DevExpress.Xpf.CodeView;
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
                    XtraMessageBox.Show("Ya está ingresando el Quilate seleccionado", "Agregar");
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
                    MontoCheque=Zero;
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
                    MontoPorPagar=Zero;
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
                    MontoAdelanto=Zero;
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

        public Moneda? Moneda
        {
            get=>GetValue<Moneda>(); 
            set=>SetValue(value);
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
            set=> SetValue(value);
        }
        private ObservableCollection<EstadoCompra> _estadoCompras=new();

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

        public async void LoadValues()
        {
            var kilates = await _preciosKilatesRepository.FindAll();
            NumeroCompra = await _compraRepository.CodigoCompra();
            PrecioKilates.AddRange(kilates);
            ItemsSource = new ObservableCollection<DetCompra>();
            var findAll = await _tipoPrecioRepository.FindAll();
            TiposPrecios.AddRange(findAll);
            EstadoCompras.AddRange(Enum.GetValues<EstadoCompra>());
            SelectedEstadoCompra = EstadoCompra.Vigente;
            var findAllMonedas = await _monedaRepository.FindAll();
            Monedas.AddRange(findAllMonedas);
            if (findAllMonedas.Count>0)
            {
                Moneda = findAllMonedas.SingleOrDefault(mo => mo.Default!.Value);
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
            if (ItemsSource.Count <= 0)
            {
                HelpersMessage.MensajeErroResult(MensajesGenericos.GuardarTitulo, MensajesCompras.DetalleCompraVacio);
                return;
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

            if (Moneda is null)
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
            var compra = new Compra
            {
                Numcompra = NumeroCompra,
                Adelantos = MontoAdelanto,
                Subtotal = SubTotal,
                Cheque = MontoCheque,
                Transferencia = MontoTransferencia,
                PorCobrar = 0,
                PorPagar = MontoPorPagar,
                Codagencia = _codagencia,
                Codcaja = _codagencia,
                Codcliente = SelectedCliente.Codcliente,
                Codestado = EstadoCompra.Vigente,
                Usuario = VariablesGlobalesForm.Instance.Usuario.Username,
                Hora = Fecha.ToShortTimeString(),
                FormaPago =  string.Join(", ", _mediosDePago),
                Codmoneda = Moneda.Codmoneda,
                Descuento = SelectedTiposPrecios.Precio,
                Dgnumdes = 0,
                Efectivo = MontoEfectivo,
                Total = Total,
                Fecha = Fecha,
                Peso = ItemsSource.Sum(detCompra => detCompra.Peso)
            };
            var save = await _compraRepository.Create(compra, ItemsSource.ToList());
            if (save)
            {
                HelpersMessage.MensajeInformacionResult("Guardar", "Se ha guardado la compra con exito");
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
            Total = SelectedTiposPrecios is not null ? Add(SubTotal,  Multiply(SubTotal, SelectedTiposPrecios.Precio!.Value)) : SubTotal;
        }
    }
}