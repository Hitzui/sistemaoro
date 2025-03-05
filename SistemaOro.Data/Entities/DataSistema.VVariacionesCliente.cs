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
    public partial class VVariacionesCliente : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Codcliente;

        private string _Nombres;

        private string? _Apellidos;

        private string? _Mes;

        private decimal? _Monto;

        private decimal? _MontoMensual;

        private decimal? _Variacion;

        private DateTime _Fecha;

        public VVariacionesCliente()
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

        public string Nombres
        {
            get
            {
                return this._Nombres;
            }
            set
            {
                if (this._Nombres != value)
                {
                    this.SendPropertyChanging("Nombres");
                    this._Nombres = value;
                    this.SendPropertyChanged("Nombres");
                }
            }
        }

        public string? Apellidos
        {
            get
            {
                return this._Apellidos;
            }
            set
            {
                if (this._Apellidos != value)
                {
                    this.SendPropertyChanging("Apellidos");
                    this._Apellidos = value;
                    this.SendPropertyChanged("Apellidos");
                }
            }
        }

        public string? Mes
        {
            get
            {
                return this._Mes;
            }
            set
            {
                if (this._Mes != value)
                {
                    this.SendPropertyChanging("Mes");
                    this._Mes = value;
                    this.SendPropertyChanged("Mes");
                }
            }
        }

        public decimal? Monto
        {
            get
            {
                return this._Monto;
            }
            set
            {
                if (this._Monto != value)
                {
                    this.SendPropertyChanging("Monto");
                    this._Monto = value;
                    this.SendPropertyChanged("Monto");
                }
            }
        }

        public decimal? MontoMensual
        {
            get
            {
                return this._MontoMensual;
            }
            set
            {
                if (this._MontoMensual != value)
                {
                    this.SendPropertyChanging("MontoMensual");
                    this._MontoMensual = value;
                    this.SendPropertyChanged("MontoMensual");
                }
            }
        }

        public decimal? Variacion
        {
            get
            {
                return this._Variacion;
            }
            set
            {
                if (this._Variacion != value)
                {
                    this.SendPropertyChanging("Variacion");
                    this._Variacion = value;
                    this.SendPropertyChanged("Variacion");
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
