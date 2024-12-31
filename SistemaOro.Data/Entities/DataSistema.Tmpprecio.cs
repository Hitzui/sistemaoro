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
    public partial class Tmpprecio : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Codcierre;

        private byte _Linea;

        private string _Codcliente;

        private decimal _Cantidad;

        private DateTime _Fecha;

        public Tmpprecio()
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

        public byte Linea
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

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          Tmpprecio toCompare = obj as Tmpprecio;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.Codcierre, toCompare.Codcierre))
            return false;
          if (!Object.Equals(this.Linea, toCompare.Linea))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + Codcierre.GetHashCode();
          hashCode = (hashCode * 7) + Linea.GetHashCode();
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
