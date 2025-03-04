﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 4/3/2025 8:45:49 AM
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
    public partial class CierrePrecio : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Codcierre;

        private string _Codcliente;

        private bool _Status;

        private decimal _OnzasFinas;

        private decimal _GramosFinos;

        private decimal _PrecioOro;

        private decimal _PrecioBase;

        private decimal _SaldoOnzas;

        private DateTime _Fecha;

        private decimal _Margen;

        public CierrePrecio()
        {
            OnCreated();
        }

        public int Codcierre
        {
            get
            {
                return this._Codcierre;
            }
            set
            {
                if (this._Codcierre != value)
                {
                    this.SendPropertyChanging("Codcierre");
                    this._Codcierre = value;
                    this.SendPropertyChanged("Codcierre");
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

        public bool Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if (this._Status != value)
                {
                    this.SendPropertyChanging("Status");
                    this._Status = value;
                    this.SendPropertyChanged("Status");
                }
            }
        }

        public decimal OnzasFinas
        {
            get
            {
                return this._OnzasFinas;
            }
            set
            {
                if (this._OnzasFinas != value)
                {
                    this.SendPropertyChanging("OnzasFinas");
                    this._OnzasFinas = value;
                    this.SendPropertyChanged("OnzasFinas");
                }
            }
        }

        public decimal GramosFinos
        {
            get
            {
                return this._GramosFinos;
            }
            set
            {
                if (this._GramosFinos != value)
                {
                    this.SendPropertyChanging("GramosFinos");
                    this._GramosFinos = value;
                    this.SendPropertyChanged("GramosFinos");
                }
            }
        }

        public decimal PrecioOro
        {
            get
            {
                return this._PrecioOro;
            }
            set
            {
                if (this._PrecioOro != value)
                {
                    this.SendPropertyChanging("PrecioOro");
                    this._PrecioOro = value;
                    this.SendPropertyChanged("PrecioOro");
                }
            }
        }

        public decimal PrecioBase
        {
            get
            {
                return this._PrecioBase;
            }
            set
            {
                if (this._PrecioBase != value)
                {
                    this.SendPropertyChanging("PrecioBase");
                    this._PrecioBase = value;
                    this.SendPropertyChanged("PrecioBase");
                }
            }
        }

        public decimal SaldoOnzas
        {
            get
            {
                return this._SaldoOnzas;
            }
            set
            {
                if (this._SaldoOnzas != value)
                {
                    this.SendPropertyChanging("SaldoOnzas");
                    this._SaldoOnzas = value;
                    this.SendPropertyChanged("SaldoOnzas");
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

        public decimal Margen
        {
            get
            {
                return this._Margen;
            }
            set
            {
                if (this._Margen != value)
                {
                    this.SendPropertyChanging("Margen");
                    this._Margen = value;
                    this.SendPropertyChanged("Margen");
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
