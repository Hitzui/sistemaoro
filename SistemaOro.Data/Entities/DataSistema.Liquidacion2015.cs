﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 30/12/2024 11:10:27 AM
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
    public partial class Liquidacion2015 : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Numcompra;

        private string? _Codcliente;

        private string? _Codcaja;

        private decimal? _Peso;

        private decimal? _Total;

        private DateTime? _Fecha;

        private string? _Usuario;

        private string? _Hora;

        private string? _Cliente;

        public Liquidacion2015()
        {
            OnCreated();
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

        public string? Codcliente
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

        public string? Codcaja
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

        public decimal? Peso
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

        public decimal? Total
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

        public DateTime? Fecha
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

        public string? Usuario
        {
            get
            {
                return this._Usuario;
            }
            set
            {
                if (this._Usuario != value)
                {
                    this.SendPropertyChanging("Usuario");
                    this._Usuario = value;
                    this.SendPropertyChanged("Usuario");
                }
            }
        }

        public string? Hora
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

        public string? Cliente
        {
            get
            {
                return this._Cliente;
            }
            set
            {
                if (this._Cliente != value)
                {
                    this.SendPropertyChanging("Cliente");
                    this._Cliente = value;
                    this.SendPropertyChanged("Cliente");
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
