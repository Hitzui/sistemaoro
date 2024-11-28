﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 27/11/2024 2:58:43 PM
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
    public partial class DetalleCompra : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _TipoDocumento;

        private string? _SimboloMoneda;

        private string _Codcliente;

        private string _Nombres;

        private string? _Apellidos;

        private string _Numcedula;

        private string? _Celular;

        private string? _Email;

        private int? _Idtipodocumento;

        private string _Numcompra;

        private decimal _Peso;

        private decimal _Total;

        private DateTime _FechaCompra;

        private int _Codestado;

        private string _Hora;

        private int? _Nocontrato;

        private int _Linea;

        private string _Descripcion;

        private string _Kilate;

        private decimal _PrecioItem;

        private decimal _PesoItem;

        private decimal? _ImporteItem;

        private string _Direccion;

        private byte[]? _Logo;

        public DetalleCompra()
        {
            OnCreated();
        }

        public string TipoDocumento
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

        public string? SimboloMoneda
        {
            get
            {
                return this._SimboloMoneda;
            }
            set
            {
                if (this._SimboloMoneda != value)
                {
                    this.SendPropertyChanging("SimboloMoneda");
                    this._SimboloMoneda = value;
                    this.SendPropertyChanged("SimboloMoneda");
                }
            }
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

        public string Numcompra
        {
            get
            {
                return this._Numcompra;
            }
            set
            {
                if (this._Numcompra != value)
                {
                    this.SendPropertyChanging("Numcompra");
                    this._Numcompra = value;
                    this.SendPropertyChanged("Numcompra");
                }
            }
        }

        public decimal Peso
        {
            get
            {
                return this._Peso;
            }
            set
            {
                if (this._Peso != value)
                {
                    this.SendPropertyChanging("Peso");
                    this._Peso = value;
                    this.SendPropertyChanged("Peso");
                }
            }
        }

        public decimal Total
        {
            get
            {
                return this._Total;
            }
            set
            {
                if (this._Total != value)
                {
                    this.SendPropertyChanging("Total");
                    this._Total = value;
                    this.SendPropertyChanged("Total");
                }
            }
        }

        public DateTime FechaCompra
        {
            get
            {
                return this._FechaCompra;
            }
            set
            {
                if (this._FechaCompra != value)
                {
                    this.SendPropertyChanging("FechaCompra");
                    this._FechaCompra = value;
                    this.SendPropertyChanged("FechaCompra");
                }
            }
        }

        public int Codestado
        {
            get
            {
                return this._Codestado;
            }
            set
            {
                if (this._Codestado != value)
                {
                    this.SendPropertyChanging("Codestado");
                    this._Codestado = value;
                    this.SendPropertyChanged("Codestado");
                }
            }
        }

        public string Hora
        {
            get
            {
                return this._Hora;
            }
            set
            {
                if (this._Hora != value)
                {
                    this.SendPropertyChanging("Hora");
                    this._Hora = value;
                    this.SendPropertyChanged("Hora");
                }
            }
        }

        public int? Nocontrato
        {
            get
            {
                return this._Nocontrato;
            }
            set
            {
                if (this._Nocontrato != value)
                {
                    this.SendPropertyChanging("Nocontrato");
                    this._Nocontrato = value;
                    this.SendPropertyChanged("Nocontrato");
                }
            }
        }

        public int Linea
        {
            get
            {
                return this._Linea;
            }
            set
            {
                if (this._Linea != value)
                {
                    this.SendPropertyChanging("Linea");
                    this._Linea = value;
                    this.SendPropertyChanged("Linea");
                }
            }
        }

        public string Descripcion
        {
            get
            {
                return this._Descripcion;
            }
            set
            {
                if (this._Descripcion != value)
                {
                    this.SendPropertyChanging("Descripcion");
                    this._Descripcion = value;
                    this.SendPropertyChanged("Descripcion");
                }
            }
        }

        public string Kilate
        {
            get
            {
                return this._Kilate;
            }
            set
            {
                if (this._Kilate != value)
                {
                    this.SendPropertyChanging("Kilate");
                    this._Kilate = value;
                    this.SendPropertyChanged("Kilate");
                }
            }
        }

        public decimal PrecioItem
        {
            get
            {
                return this._PrecioItem;
            }
            set
            {
                if (this._PrecioItem != value)
                {
                    this.SendPropertyChanging("PrecioItem");
                    this._PrecioItem = value;
                    this.SendPropertyChanged("PrecioItem");
                }
            }
        }

        public decimal PesoItem
        {
            get
            {
                return this._PesoItem;
            }
            set
            {
                if (this._PesoItem != value)
                {
                    this.SendPropertyChanging("PesoItem");
                    this._PesoItem = value;
                    this.SendPropertyChanged("PesoItem");
                }
            }
        }

        public decimal? ImporteItem
        {
            get
            {
                return this._ImporteItem;
            }
            set
            {
                if (this._ImporteItem != value)
                {
                    this.SendPropertyChanging("ImporteItem");
                    this._ImporteItem = value;
                    this.SendPropertyChanged("ImporteItem");
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

        public byte[]? Logo
        {
            get
            {
                return this._Logo;
            }
            set
            {
                if (this._Logo != value)
                {
                    this.SendPropertyChanging("Logo");
                    this._Logo = value;
                    this.SendPropertyChanged("Logo");
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
