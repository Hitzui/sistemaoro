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
    public partial class PagosAdelantado : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _IdPagoefec;

        private string _Idingreso;

        private string? _Codagencia;

        private DateTime _FechaopParcial;

        private decimal _ValorParcialpagado;

        private string _Usuario;

        private DateTime _HoraOp;

        private string _EstadoOp;

        private string? _CajaRegadel;

        public PagosAdelantado()
        {
            OnCreated();
        }

        public int IdPagoefec
        {
            get
            {
                return this._IdPagoefec;
            }
            set
            {
                if (this._IdPagoefec != value)
                {
                    this.SendPropertyChanging("IdPagoefec");
                    this._IdPagoefec = value;
                    this.SendPropertyChanged("IdPagoefec");
                }
            }
        }

        public string Idingreso
        {
            get
            {
                return this._Idingreso;
            }
            set
            {
                if (this._Idingreso != value)
                {
                    this.SendPropertyChanging("Idingreso");
                    this._Idingreso = value;
                    this.SendPropertyChanged("Idingreso");
                }
            }
        }

        public string? Codagencia
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

        public DateTime FechaopParcial
        {
            get
            {
                return this._FechaopParcial;
            }
            set
            {
                if (this._FechaopParcial != value)
                {
                    this.SendPropertyChanging("FechaopParcial");
                    this._FechaopParcial = value;
                    this.SendPropertyChanged("FechaopParcial");
                }
            }
        }

        public decimal ValorParcialpagado
        {
            get
            {
                return this._ValorParcialpagado;
            }
            set
            {
                if (this._ValorParcialpagado != value)
                {
                    this.SendPropertyChanging("ValorParcialpagado");
                    this._ValorParcialpagado = value;
                    this.SendPropertyChanged("ValorParcialpagado");
                }
            }
        }

        public string Usuario
        {
            get
            {
                return this._Usuario;
            }
            set
            {
                if (this._Usuario != value)
                {
                    this.SendPropertyChanging("Usuario");
                    this._Usuario = value;
                    this.SendPropertyChanged("Usuario");
                }
            }
        }

        public DateTime HoraOp
        {
            get
            {
                return this._HoraOp;
            }
            set
            {
                if (this._HoraOp != value)
                {
                    this.SendPropertyChanging("HoraOp");
                    this._HoraOp = value;
                    this.SendPropertyChanged("HoraOp");
                }
            }
        }

        public string EstadoOp
        {
            get
            {
                return this._EstadoOp;
            }
            set
            {
                if (this._EstadoOp != value)
                {
                    this.SendPropertyChanging("EstadoOp");
                    this._EstadoOp = value;
                    this.SendPropertyChanged("EstadoOp");
                }
            }
        }

        public string? CajaRegadel
        {
            get
            {
                return this._CajaRegadel;
            }
            set
            {
                if (this._CajaRegadel != value)
                {
                    this.SendPropertyChanging("CajaRegadel");
                    this._CajaRegadel = value;
                    this.SendPropertyChanged("CajaRegadel");
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
