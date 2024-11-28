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
    public partial class PrecioKilate : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _IdPrecioKilate;

        private string _Descripcion;

        private decimal _Peso;

        private decimal _Precio;

        public PrecioKilate()
        {
            OnCreated();
        }

        public int IdPrecioKilate
        {
            get
            {
                return this._IdPrecioKilate;
            }
            set
            {
                if (this._IdPrecioKilate != value)
                {
                    this.SendPropertyChanging("IdPrecioKilate");
                    this._IdPrecioKilate = value;
                    this.SendPropertyChanged("IdPrecioKilate");
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

        public decimal Precio
        {
            get
            {
                return this._Precio;
            }
            set
            {
                if (this._Precio != value)
                {
                    this.SendPropertyChanging("Precio");
                    this._Precio = value;
                    this.SendPropertyChanged("Precio");
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
