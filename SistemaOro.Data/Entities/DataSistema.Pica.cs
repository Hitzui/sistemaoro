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
    public partial class Pica : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Codcliente;

        private string _NombreEntidad;

        private string? _TipoRelacion;

        private string? _TiempoMantener;

        private decimal? _IngresoMensual;

        public Pica()
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

        public string NombreEntidad
        {
            get
            {
                return this._NombreEntidad;
            }
            set
            {
                if (this._NombreEntidad != value)
                {
                    this.SendPropertyChanging("NombreEntidad");
                    this._NombreEntidad = value;
                    this.SendPropertyChanged("NombreEntidad");
                }
            }
        }

        public string? TipoRelacion
        {
            get
            {
                return this._TipoRelacion;
            }
            set
            {
                if (this._TipoRelacion != value)
                {
                    this.SendPropertyChanging("TipoRelacion");
                    this._TipoRelacion = value;
                    this.SendPropertyChanged("TipoRelacion");
                }
            }
        }

        public string? TiempoMantener
        {
            get
            {
                return this._TiempoMantener;
            }
            set
            {
                if (this._TiempoMantener != value)
                {
                    this.SendPropertyChanging("TiempoMantener");
                    this._TiempoMantener = value;
                    this.SendPropertyChanged("TiempoMantener");
                }
            }
        }

        public decimal? IngresoMensual
        {
            get
            {
                return this._IngresoMensual;
            }
            set
            {
                if (this._IngresoMensual != value)
                {
                    this.SendPropertyChanging("IngresoMensual");
                    this._IngresoMensual = value;
                    this.SendPropertyChanged("IngresoMensual");
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
