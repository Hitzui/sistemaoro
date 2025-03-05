﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 4/3/2025 9:51:11 AM
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
    public partial class Precio : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Codcliente;

        private decimal _Kilate;

        private decimal _Precio1;

        private decimal _Gramos;

        private decimal _PrecioOro;

        public Precio()
        {
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

        public decimal Kilate
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

        public decimal Precio1
        {
            get
            {
                return this._Precio1;
            }
            set
            {
                if (this._Precio1 != value)
                {
                    this.SendPropertyChanging("Precio1");
                    this._Precio1 = value;
                    this.SendPropertyChanged("Precio1");
                }
            }
        }

        public decimal Gramos
        {
            get
            {
                return this._Gramos;
            }
            set
            {
                if (this._Gramos != value)
                {
                    this.SendPropertyChanging("Gramos");
                    this._Gramos = value;
                    this.SendPropertyChanged("Gramos");
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

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          Precio toCompare = obj as Precio;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.Codcliente, toCompare.Codcliente))
            return false;
          if (!Object.Equals(this.Kilate, toCompare.Kilate))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + Codcliente.GetHashCode();
          hashCode = (hashCode * 7) + Kilate.GetHashCode();
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
