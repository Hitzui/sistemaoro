﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 25/11/2024 4:19:40 PM
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
    public partial class Detacierre : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Codcierre;

        private string _Numcompra;

        private decimal _Onzas;

        private decimal _Saldo;

        private DateTime _Fecha;

        private decimal _Cantidad;

        private string _Codagencia;

        public Detacierre()
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

        public decimal Onzas
        {
            get
            {
                return this._Onzas;
            }
            set
            {
                if (this._Onzas != value)
                {
                    this.SendPropertyChanging("Onzas");
                    this._Onzas = value;
                    this.SendPropertyChanged("Onzas");
                }
            }
        }

        public decimal Saldo
        {
            get
            {
                return this._Saldo;
            }
            set
            {
                if (this._Saldo != value)
                {
                    this.SendPropertyChanging("Saldo");
                    this._Saldo = value;
                    this.SendPropertyChanged("Saldo");
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

        public decimal Cantidad
        {
            get
            {
                return this._Cantidad;
            }
            set
            {
                if (this._Cantidad != value)
                {
                    this.SendPropertyChanging("Cantidad");
                    this._Cantidad = value;
                    this.SendPropertyChanged("Cantidad");
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

        public override bool Equals(object obj)
        {
          Detacierre toCompare = obj as Detacierre;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.Codcierre, toCompare.Codcierre))
            return false;
          if (!Object.Equals(this.Numcompra, toCompare.Numcompra))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + Codcierre.GetHashCode();
          hashCode = (hashCode * 7) + Numcompra.GetHashCode();
          return hashCode;
        }

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
