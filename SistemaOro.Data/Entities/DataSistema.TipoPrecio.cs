﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 26/10/2024 6:22:50 AM
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
    public partial class TipoPrecio : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Idtipoprecio;

        private string? _Descripcion;

        private decimal? _Precio;

        private IList<Compra> _Compras;

        public TipoPrecio()
        {
            this._Compras = new List<Compra>();
            OnCreated();
        }

        public int Idtipoprecio
        {
            get
            {
                return this._Idtipoprecio;
            }
            set
            {
                if (this._Idtipoprecio != value)
                {
                    this.SendPropertyChanging("Idtipoprecio");
                    this._Idtipoprecio = value;
                    this.SendPropertyChanged("Idtipoprecio");
                }
            }
        }

        public string? Descripcion
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

        public decimal? Precio
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
