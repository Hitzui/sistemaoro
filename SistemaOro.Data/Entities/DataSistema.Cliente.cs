﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 15/4/2025 6:49:34 AM
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

#nullable enable annotations
#nullable disable warnings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace SistemaOro.Data.Entities
{
    public partial class Cliente : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Codcliente;

        private string _Nombres;

        private string? _Apellidos;

        private string _Numcedula;

        private DateTime? _FEmision;

        private DateTime? _FVencimiento;

        private string _Direccion;

        private DateTime? _FNacimiento;

        private string? _Estadocivil;

        private string? _Ciudad;

        private string _Telefono;

        private string? _Celular;

        private string? _Email;

        private DateTime? _FIngreso;

        private string? _Ocupacion;

        private string? _DireccionNegocio;

        private string? _TiempoNeg;

        private string? _OtraAe;

        private string? _DescOtra;

        private string? _NomCuenta;

        private string? _NumCuenta;

        private string? _NomBanco;

        private decimal? _MontoMensual;

        private decimal? _TotalOperaciones;

        private string? _ActuaPor;

        private string? _NombreTercero;

        private string? _DireccionTercero;

        private int? _Pica;

        private string? _Ocupacion2;

        private int? _Idtipodocumento;

        private IList<Adelanto> _Adelantos;

        private IList<Compra> _Compras;

        private TipoDocumento _TipoDocumento;

        public Cliente()
        {
            this._Ocupacion2 = @"Cliente Generico";
            this._Idtipodocumento = 0;
            this._Adelantos = new List<Adelanto>();
            this._Compras = new List<Compra>();
            OnCreated();
        }

        public string Codcliente
        {
            get
            {
                return this._Codcliente;
            }
            set
            {
                if (this._Codcliente != value)
                {
                    this.SendPropertyChanging("Codcliente");
                    this._Codcliente = value;
                    this.SendPropertyChanged("Codcliente");
                }
            }
        }

        public string Nombres
        {
            get
            {
                return this._Nombres;
            }
            set
            {
                if (this._Nombres != value)
                {
                    this.SendPropertyChanging("Nombres");
                    this._Nombres = value;
                    this.SendPropertyChanged("Nombres");
                }
            }
        }

        public string? Apellidos
        {
            get
            {
                return this._Apellidos;
            }
            set
            {
                if (this._Apellidos != value)
                {
                    this.SendPropertyChanging("Apellidos");
                    this._Apellidos = value;
                    this.SendPropertyChanged("Apellidos");
                }
            }
        }

        public string Numcedula
        {
            get
            {
                return this._Numcedula;
            }
            set
            {
                if (this._Numcedula != value)
                {
                    this.SendPropertyChanging("Numcedula");
                    this._Numcedula = value;
                    this.SendPropertyChanged("Numcedula");
                }
            }
        }

        public DateTime? FEmision
        {
            get
            {
                return this._FEmision;
            }
            set
            {
                if (this._FEmision != value)
                {
                    this.SendPropertyChanging("FEmision");
                    this._FEmision = value;
                    this.SendPropertyChanged("FEmision");
                }
            }
        }

        public DateTime? FVencimiento
        {
            get
            {
                return this._FVencimiento;
            }
            set
            {
                if (this._FVencimiento != value)
                {
                    this.SendPropertyChanging("FVencimiento");
                    this._FVencimiento = value;
                    this.SendPropertyChanged("FVencimiento");
                }
            }
        }

        public string Direccion
        {
            get
            {
                return this._Direccion;
            }
            set
            {
                if (this._Direccion != value)
                {
                    this.SendPropertyChanging("Direccion");
                    this._Direccion = value;
                    this.SendPropertyChanged("Direccion");
                }
            }
        }

        public DateTime? FNacimiento
        {
            get
            {
                return this._FNacimiento;
            }
            set
            {
                if (this._FNacimiento != value)
                {
                    this.SendPropertyChanging("FNacimiento");
                    this._FNacimiento = value;
                    this.SendPropertyChanged("FNacimiento");
                }
            }
        }

        public string? Estadocivil
        {
            get
            {
                return this._Estadocivil;
            }
            set
            {
                if (this._Estadocivil != value)
                {
                    this.SendPropertyChanging("Estadocivil");
                    this._Estadocivil = value;
                    this.SendPropertyChanged("Estadocivil");
                }
            }
        }

        public string? Ciudad
        {
            get
            {
                return this._Ciudad;
            }
            set
            {
                if (this._Ciudad != value)
                {
                    this.SendPropertyChanging("Ciudad");
                    this._Ciudad = value;
                    this.SendPropertyChanged("Ciudad");
                }
            }
        }

        public string Telefono
        {
            get
            {
                return this._Telefono;
            }
            set
            {
                if (this._Telefono != value)
                {
                    this.SendPropertyChanging("Telefono");
                    this._Telefono = value;
                    this.SendPropertyChanged("Telefono");
                }
            }
        }

        public string? Celular
        {
            get
            {
                return this._Celular;
            }
            set
            {
                if (this._Celular != value)
                {
                    this.SendPropertyChanging("Celular");
                    this._Celular = value;
                    this.SendPropertyChanged("Celular");
                }
            }
        }

        public string? Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                if (this._Email != value)
                {
                    this.SendPropertyChanging("Email");
                    this._Email = value;
                    this.SendPropertyChanged("Email");
                }
            }
        }

        public DateTime? FIngreso
        {
            get
            {
                return this._FIngreso;
            }
            set
            {
                if (this._FIngreso != value)
                {
                    this.SendPropertyChanging("FIngreso");
                    this._FIngreso = value;
                    this.SendPropertyChanged("FIngreso");
                }
            }
        }

        public string? Ocupacion
        {
            get
            {
                return this._Ocupacion;
            }
            set
            {
                if (this._Ocupacion != value)
                {
                    this.SendPropertyChanging("Ocupacion");
                    this._Ocupacion = value;
                    this.SendPropertyChanged("Ocupacion");
                }
            }
        }

        public string? DireccionNegocio
        {
            get
            {
                return this._DireccionNegocio;
            }
            set
            {
                if (this._DireccionNegocio != value)
                {
                    this.SendPropertyChanging("DireccionNegocio");
                    this._DireccionNegocio = value;
                    this.SendPropertyChanged("DireccionNegocio");
                }
            }
        }

        public string? TiempoNeg
        {
            get
            {
                return this._TiempoNeg;
            }
            set
            {
                if (this._TiempoNeg != value)
                {
                    this.SendPropertyChanging("TiempoNeg");
                    this._TiempoNeg = value;
                    this.SendPropertyChanged("TiempoNeg");
                }
            }
        }

        public string? OtraAe
        {
            get
            {
                return this._OtraAe;
            }
            set
            {
                if (this._OtraAe != value)
                {
                    this.SendPropertyChanging("OtraAe");
                    this._OtraAe = value;
                    this.SendPropertyChanged("OtraAe");
                }
            }
        }

        public string? DescOtra
        {
            get
            {
                return this._DescOtra;
            }
            set
            {
                if (this._DescOtra != value)
                {
                    this.SendPropertyChanging("DescOtra");
                    this._DescOtra = value;
                    this.SendPropertyChanged("DescOtra");
                }
            }
        }

        public string? NomCuenta
        {
            get
            {
                return this._NomCuenta;
            }
            set
            {
                if (this._NomCuenta != value)
                {
                    this.SendPropertyChanging("NomCuenta");
                    this._NomCuenta = value;
                    this.SendPropertyChanged("NomCuenta");
                }
            }
        }

        public string? NumCuenta
        {
            get
            {
                return this._NumCuenta;
            }
            set
            {
                if (this._NumCuenta != value)
                {
                    this.SendPropertyChanging("NumCuenta");
                    this._NumCuenta = value;
                    this.SendPropertyChanged("NumCuenta");
                }
            }
        }

        public string? NomBanco
        {
            get
            {
                return this._NomBanco;
            }
            set
            {
                if (this._NomBanco != value)
                {
                    this.SendPropertyChanging("NomBanco");
                    this._NomBanco = value;
                    this.SendPropertyChanged("NomBanco");
                }
            }
        }

        public decimal? MontoMensual
        {
            get
            {
                return this._MontoMensual;
            }
            set
            {
                if (this._MontoMensual != value)
                {
                    this.SendPropertyChanging("MontoMensual");
                    this._MontoMensual = value;
                    this.SendPropertyChanged("MontoMensual");
                }
            }
        }

        public decimal? TotalOperaciones
        {
            get
            {
                return this._TotalOperaciones;
            }
            set
            {
                if (this._TotalOperaciones != value)
                {
                    this.SendPropertyChanging("TotalOperaciones");
                    this._TotalOperaciones = value;
                    this.SendPropertyChanged("TotalOperaciones");
                }
            }
        }

        public string? ActuaPor
        {
            get
            {
                return this._ActuaPor;
            }
            set
            {
                if (this._ActuaPor != value)
                {
                    this.SendPropertyChanging("ActuaPor");
                    this._ActuaPor = value;
                    this.SendPropertyChanged("ActuaPor");
                }
            }
        }

        public string? NombreTercero
        {
            get
            {
                return this._NombreTercero;
            }
            set
            {
                if (this._NombreTercero != value)
                {
                    this.SendPropertyChanging("NombreTercero");
                    this._NombreTercero = value;
                    this.SendPropertyChanged("NombreTercero");
                }
            }
        }

        public string? DireccionTercero
        {
            get
            {
                return this._DireccionTercero;
            }
            set
            {
                if (this._DireccionTercero != value)
                {
                    this.SendPropertyChanging("DireccionTercero");
                    this._DireccionTercero = value;
                    this.SendPropertyChanged("DireccionTercero");
                }
            }
        }

        public int? Pica
        {
            get
            {
                return this._Pica;
            }
            set
            {
                if (this._Pica != value)
                {
                    this.SendPropertyChanging("Pica");
                    this._Pica = value;
                    this.SendPropertyChanged("Pica");
                }
            }
        }

        public string? Ocupacion2
        {
            get
            {
                return this._Ocupacion2;
            }
            set
            {
                if (this._Ocupacion2 != value)
                {
                    this.SendPropertyChanging("Ocupacion2");
                    this._Ocupacion2 = value;
                    this.SendPropertyChanged("Ocupacion2");
                }
            }
        }

        public int? Idtipodocumento
        {
            get
            {
                return this._Idtipodocumento;
            }
            set
            {
                if (this._Idtipodocumento != value)
                {
                    this.SendPropertyChanging("Idtipodocumento");
                    this._Idtipodocumento = value;
                    this.SendPropertyChanged("Idtipodocumento");
                }
            }
        }

        public virtual IList<Adelanto> Adelantos
        {
            get
            {
                return this._Adelantos;
            }
            set
            {
                this._Adelantos = value;
            }
        }

        public virtual IList<Compra> Compras
        {
            get
            {
                return this._Compras;
            }
            set
            {
                this._Compras = value;
            }
        }

        public virtual TipoDocumento TipoDocumento
        {
            get
            {
                return this._TipoDocumento;
            }
            set
            {
                if (this._TipoDocumento != value)
                {
                    this.SendPropertyChanging("TipoDocumento");
                    this._TipoDocumento = value;
                    this.SendPropertyChanged("TipoDocumento");
                }
            }
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion

        public virtual event PropertyChangingEventHandler PropertyChanging;

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            var handler = this.PropertyChanging;
            if (handler != null)
                handler(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanging(System.String propertyName) 
        {
            var handler = this.PropertyChanging;
            if (handler != null)
                handler(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void SendPropertyChanged(System.String propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
