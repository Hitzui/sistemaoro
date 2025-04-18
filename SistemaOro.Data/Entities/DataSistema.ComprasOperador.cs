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
    public partial class ComprasOperador : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Nombre;

        private string _Numcompra;

        private DateTime _Fecha;

        private decimal _PesoTotal;

        private decimal _Total;

        private string _Codcaja;

        private string _Hora;

        private string _Kilate;

        private decimal _Peso;

        private decimal? _Importe;

        private decimal _Preciok;

        private string _Codcliente;

        private string _Codagencia;

        public ComprasOperador()
        {
            OnCreated();
        }

        public string Nombre
        {
            get
            {
                return this._Nombre;
            }
            set
            {
                if (this._Nombre != value)
                {
                    this.SendPropertyChanging("Nombre");
                    this._Nombre = value;
                    this.SendPropertyChanged("Nombre");
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

        public DateTime Fecha
        {
            get
            {
                return this._Fecha;
            }
            set
            {
                if (this._Fecha != value)
                {
                    this.SendPropertyChanging("Fecha");
                    this._Fecha = value;
                    this.SendPropertyChanged("Fecha");
                }
            }
        }

        public decimal PesoTotal
        {
            get
            {
                return this._PesoTotal;
            }
            set
            {
                if (this._PesoTotal != value)
                {
                    this.SendPropertyChanging("PesoTotal");
                    this._PesoTotal = value;
                    this.SendPropertyChanged("PesoTotal");
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

        public string Codcaja
        {
            get
            {
                return this._Codcaja;
            }
            set
            {
                if (this._Codcaja != value)
                {
                    this.SendPropertyChanging("Codcaja");
                    this._Codcaja = value;
                    this.SendPropertyChanged("Codcaja");
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

        public decimal? Importe
        {
            get
            {
                return this._Importe;
            }
            set
            {
                if (this._Importe != value)
                {
                    this.SendPropertyChanging("Importe");
                    this._Importe = value;
                    this.SendPropertyChanged("Importe");
                }
            }
        }

        public decimal Preciok
        {
            get
            {
                return this._Preciok;
            }
            set
            {
                if (this._Preciok != value)
                {
                    this.SendPropertyChanging("Preciok");
                    this._Preciok = value;
                    this.SendPropertyChanged("Preciok");
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

        public string Codagencia
        {
            get
            {
                return this._Codagencia;
            }
            set
            {
                if (this._Codagencia != value)
                {
                    this.SendPropertyChanging("Codagencia");
                    this._Codagencia = value;
                    this.SendPropertyChanged("Codagencia");
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
