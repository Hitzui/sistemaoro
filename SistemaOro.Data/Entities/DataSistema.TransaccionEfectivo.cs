﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 15/3/2025 11:51:12 AM
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
    public partial class TransaccionEfectivo : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Numcompra1;

        private string _Codcliente1;

        private string _Nombres1;

        private string? _Apellidos1;

        private DateTime _Fecha1;

        private decimal _Total1;

        private string _FormaPago;

        public TransaccionEfectivo()
        {
            OnCreated();
        }

        public string Numcompra1
        {
            get
            {
                return this._Numcompra1;
            }
            set
            {
                if (this._Numcompra1 != value)
                {
                    this.SendPropertyChanging("Numcompra1");
                    this._Numcompra1 = value;
                    this.SendPropertyChanged("Numcompra1");
                }
            }
        }

        public string Codcliente1
        {
            get
            {
                return this._Codcliente1;
            }
            set
            {
                if (this._Codcliente1 != value)
                {
                    this.SendPropertyChanging("Codcliente1");
                    this._Codcliente1 = value;
                    this.SendPropertyChanged("Codcliente1");
                }
            }
        }

        public string Nombres1
        {
            get
            {
                return this._Nombres1;
            }
            set
            {
                if (this._Nombres1 != value)
                {
                    this.SendPropertyChanging("Nombres1");
                    this._Nombres1 = value;
                    this.SendPropertyChanged("Nombres1");
                }
            }
        }

        public string? Apellidos1
        {
            get
            {
                return this._Apellidos1;
            }
            set
            {
                if (this._Apellidos1 != value)
                {
                    this.SendPropertyChanging("Apellidos1");
                    this._Apellidos1 = value;
                    this.SendPropertyChanged("Apellidos1");
                }
            }
        }

        public DateTime Fecha1
        {
            get
            {
                return this._Fecha1;
            }
            set
            {
                if (this._Fecha1 != value)
                {
                    this.SendPropertyChanging("Fecha1");
                    this._Fecha1 = value;
                    this.SendPropertyChanged("Fecha1");
                }
            }
        }

        public decimal Total1
        {
            get
            {
                return this._Total1;
            }
            set
            {
                if (this._Total1 != value)
                {
                    this.SendPropertyChanging("Total1");
                    this._Total1 = value;
                    this.SendPropertyChanged("Total1");
                }
            }
        }

        public string FormaPago
        {
            get
            {
                return this._FormaPago;
            }
            set
            {
                if (this._FormaPago != value)
                {
                    this.SendPropertyChanging("FormaPago");
                    this._FormaPago = value;
                    this.SendPropertyChanged("FormaPago");
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
