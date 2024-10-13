﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 11/10/2024 2:18:02 PM
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
    public partial class Moneda : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Codmoneda;

        private string? _Descripcion;

        private string? _Simbolo;

        private DateTime? _Fecha;

        private bool? _Default;

        private IList<Compra> _Compras;

        public Moneda()
        {
            this._Default = false;
            this._Compras = new List<Compra>();
            OnCreated();
        }

        public int Codmoneda
        {
            get
            {
                return this._Codmoneda;
            }
            set
            {
                if (this._Codmoneda != value)
                {
                    this.SendPropertyChanging("Codmoneda");
                    this._Codmoneda = value;
                    this.SendPropertyChanged("Codmoneda");
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

        public string? Simbolo
        {
            get
            {
                return this._Simbolo;
            }
            set
            {
                if (this._Simbolo != value)
                {
                    this.SendPropertyChanging("Simbolo");
                    this._Simbolo = value;
                    this.SendPropertyChanged("Simbolo");
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

        public bool? Default
        {
            get
            {
                return this._Default;
            }
            set
            {
                if (this._Default != value)
                {
                    this.SendPropertyChanging("Default");
                    this._Default = value;
                    this.SendPropertyChanged("Default");
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
