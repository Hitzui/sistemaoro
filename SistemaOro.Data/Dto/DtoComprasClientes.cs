using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SistemaOro.Data.Dto;

public class DtoComprasClientes : INotifyPropertyChanged
{
    private bool _isChecked;
    private string? _numcompra;
    private string? _codcliente;
    private DateTime? _fecha;
    private decimal? _total;
    private string? _nombre;
    private string? _apellido;
    private int? _nocontrato;
    private decimal _peso;
    private string? _firma;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetField(ref _isChecked, value);
    }

    public string? Numcompra
    {
        get => _numcompra;
        set => SetField(ref _numcompra, value);
    }

    public string? Codcliente
    {
        get => _codcliente;
        set => SetField(ref _codcliente, value);
    }

    public DateTime? Fecha
    {
        get => _fecha;
        set => SetField(ref _fecha, value);
    }

    public decimal? Total
    {
        get => _total;
        set => SetField(ref _total, value);
    }

    public string? Nombre
    {
        get => _nombre;
        set => SetField(ref _nombre, value);
    }

    public string? Apellido
    {
        get => _apellido;
        set => SetField(ref _apellido, value);
    }

    public int? Nocontrato
    {
        get => _nocontrato;
        set => SetField(ref _nocontrato, value);
    }

    public decimal Peso
    {
        get => _peso;
        set => SetField(ref _peso, value);
    }

    public string? Firma
    {
        get => _firma;
        set => SetField(ref _firma, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}