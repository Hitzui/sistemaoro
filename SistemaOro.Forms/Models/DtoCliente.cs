using System;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.XtraEditors.DXErrorProvider;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.Services;

namespace SistemaOro.Forms.Models;

public class DtoCliente : ViewModelBase, IDataErrorInfo
{
    public Cliente GetCliente()
    {
        var mapper = MapperConfig.InitializeAutomapper();
        return mapper.Map<Cliente>(this);
    }
    public DtoCliente GetDtoCliente(Cliente cliente)
    {
        var mapper = MapperConfig.InitializeAutomapper();
        return mapper.Map<DtoCliente>(cliente);
    }
    public string Codcliente
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string Nombres
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? Apellidos
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string Numcedula
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public DateTime? FEmision
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public DateTime? FVencimiento
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public string Direccion
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public DateTime? FNacimiento
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public string? Estadocivil
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? Ciudad
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string Telefono
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? Celular
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? Email
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public DateTime? FIngreso
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public string? Ocupacion
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? DireccionNegocio
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? TiempoNeg
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? OtraAe
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? DescOtra
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? NomCuenta
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? NumCuenta
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? NomBanco
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public decimal? MontoMensual
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public decimal? TotalOperaciones
    {
        get => GetValue<decimal>();
        set => SetValue(value);
    }

    public string? ActuaPor
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? NombreTercero
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? DireccionTercero
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public int? Pica
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public string? Ocupacion2
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public TipoDocumento? TipoDocumento
    {
        get => GetValue<TipoDocumento>();
        set => SetValue(value);
    }


    public string this[string columnName]{
        get
        {
            var error = "";
            switch (columnName)
            {
                case nameof(Nombres):
                    if (string.IsNullOrWhiteSpace(Nombres))
                    {
                        error = "El nombre es requerido.";
                    }
                    break;
                case nameof(Apellidos):
                    if (string.IsNullOrWhiteSpace(Apellidos))
                    {
                        error = "El apellido es requerido.";
                    }
                    break;
                case nameof(Email):
                    if (string.IsNullOrWhiteSpace(Email))
                    {
                        error = "El correo electrónico es requerido.";
                    }
                    break;
                case nameof(Numcedula):
                    if (string.IsNullOrWhiteSpace(Numcedula))
                    {
                        error = "El número de identificación es requerido.";
                    }
                    break;
                case nameof(Celular):
                    if (string.IsNullOrWhiteSpace(Celular))
                    {
                        error = "El número de celular es requerido.";
                    }
                    break;
            }
            return error;
        }
    }

    public string Error => "";
}