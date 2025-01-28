using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DevExpress.XtraEditors.DXErrorProvider;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.Services;

namespace SistemaOro.Forms.Models;

public class DtoUsuario : INotifyPropertyChanged, IDataErrorInfo
{
    private string? _codoperador;

    private string? _username;

    private string? _clave;

    private string? _nombre;

    private string? _pregunta;

    private string? _respuesta;

    private DateTime _fcreau;

    private Nivel _nivel;

    private EstadoUsuario? _estado;

    public Usuario GetUsuario()
    {
        var mapper = MapperConfig.InitializeAutomapper();
        return mapper.Map<Usuario>(this);
    }

    public DtoUsuario? GetDtoUsuario(Usuario? usuario)
    {
        if (usuario == null)
        {
            return null;
        }


        var mapper = MapperConfig.InitializeAutomapper();
        return mapper.Map<DtoUsuario>(usuario);
    }

    public string? Codoperador
    {
        get => _codoperador;
        set => SetField(ref _codoperador, value);
    }

    public string? Username
    {
        get => _username;
        set => SetField(ref _username, value);
    }

    public string? Clave
    {
        get => _clave;
        set => SetField(ref _clave, value);
    }

    public string? Nombre
    {
        get => _nombre;
        set => SetField(ref _nombre, value);
    }

    public string? Pregunta
    {
        get => _pregunta;
        set => SetField(ref _pregunta, value);
    }

    public string? Respuesta
    {
        get => _respuesta;
        set => SetField(ref _respuesta, value);
    }

    public DateTime Fcreau
    {
        get => _fcreau;
        set => SetField(ref _fcreau, value);
    }

    public Nivel Nivel
    {
        get => _nivel;
        set => SetField(ref _nivel, value);
    }

    public EstadoUsuario? Estado
    {
        get => _estado;
        set => SetField(ref _estado, value);
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


    public string Error { get; }

    public string this[string columnName]
    {
        get
        {
            if (columnName == "Codoperador" && Codoperador == "" ||
                columnName == "Username" && Username == "" ||
                columnName == "Clave" && Clave == "" ||
                columnName == "Nombre" && Nombre == "" ||
                columnName == "Pregunta" && Pregunta == "" ||
                columnName == "Respuesta" && Respuesta == "")
            {
                return "El campo no puede estar vacío";
            }

            return "";
        }
    }
}