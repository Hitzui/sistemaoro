﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 28/9/2024 10:53:19 AM
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
    public partial class Id : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Codcliente;

        private int? _Codagencia;

        private int? _Numcompra;

        private int? _Idadelanto;

        private int? _Idcompras;

        private int? _IdAdelantos;

        private int? _SaldoAnterior;

        private int? _CierreCompra;

        private int? _PrestamoEgreso;

        private int? _PrestamoIngreso;

        private int? _AnularCompra;

        private int? _AnularAdelanto;

        private bool? _VariasCompras;

        private string? _Recibe;

        private int? _PagoAdelanto;

        private int? _Idreserva;

        private DateTime? _Backup;

        private int? _Cordobas;

        private int? _Dolares;

        private int? _Nocontrato;

        private byte[]? _Logo;

        public Id()
        {
            OnCreated();
        }

        public int Codcliente
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

        public int? Codagencia
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

        public int? Numcompra
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

        public int? Idadelanto
        {
            get
            {
                return this._Idadelanto;
            }
            set
            {
                if (this._Idadelanto != value)
                {
                    this.SendPropertyChanging("Idadelanto");
                    this._Idadelanto = value;
                    this.SendPropertyChanged("Idadelanto");
                }
            }
        }

        public int? Idcompras
        {
            get
            {
                return this._Idcompras;
            }
            set
            {
                if (this._Idcompras != value)
                {
                    this.SendPropertyChanging("Idcompras");
                    this._Idcompras = value;
                    this.SendPropertyChanged("Idcompras");
                }
            }
        }

        public int? IdAdelantos
        {
            get
            {
                return this._IdAdelantos;
            }
            set
            {
                if (this._IdAdelantos != value)
                {
                    this.SendPropertyChanging("IdAdelantos");
                    this._IdAdelantos = value;
                    this.SendPropertyChanged("IdAdelantos");
                }
            }
        }

        public int? SaldoAnterior
        {
            get
            {
                return this._SaldoAnterior;
            }
            set
            {
                if (this._SaldoAnterior != value)
                {
                    this.SendPropertyChanging("SaldoAnterior");
                    this._SaldoAnterior = value;
                    this.SendPropertyChanged("SaldoAnterior");
                }
            }
        }

        public int? CierreCompra
        {
            get
            {
                return this._CierreCompra;
            }
            set
            {
                if (this._CierreCompra != value)
                {
                    this.SendPropertyChanging("CierreCompra");
                    this._CierreCompra = value;
                    this.SendPropertyChanged("CierreCompra");
                }
            }
        }

        public int? PrestamoEgreso
        {
            get
            {
                return this._PrestamoEgreso;
            }
            set
            {
                if (this._PrestamoEgreso != value)
                {
                    this.SendPropertyChanging("PrestamoEgreso");
                    this._PrestamoEgreso = value;
                    this.SendPropertyChanged("PrestamoEgreso");
                }
            }
        }

        public int? PrestamoIngreso
        {
            get
            {
                return this._PrestamoIngreso;
            }
            set
            {
                if (this._PrestamoIngreso != value)
                {
                    this.SendPropertyChanging("PrestamoIngreso");
                    this._PrestamoIngreso = value;
                    this.SendPropertyChanged("PrestamoIngreso");
                }
            }
        }

        public int? AnularCompra
        {
            get
            {
                return this._AnularCompra;
            }
            set
            {
                if (this._AnularCompra != value)
                {
                    this.SendPropertyChanging("AnularCompra");
                    this._AnularCompra = value;
                    this.SendPropertyChanged("AnularCompra");
                }
            }
        }

        public int? AnularAdelanto
        {
            get
            {
                return this._AnularAdelanto;
            }
            set
            {
                if (this._AnularAdelanto != value)
                {
                    this.SendPropertyChanging("AnularAdelanto");
                    this._AnularAdelanto = value;
                    this.SendPropertyChanged("AnularAdelanto");
                }
            }
        }

        public bool? VariasCompras
        {
            get
            {
                return this._VariasCompras;
            }
            set
            {
                if (this._VariasCompras != value)
                {
                    this.SendPropertyChanging("VariasCompras");
                    this._VariasCompras = value;
                    this.SendPropertyChanged("VariasCompras");
                }
            }
        }

        public string? Recibe
        {
            get
            {
                return this._Recibe;
            }
            set
            {
                if (this._Recibe != value)
                {
                    this.SendPropertyChanging("Recibe");
                    this._Recibe = value;
                    this.SendPropertyChanged("Recibe");
                }
            }
        }

        public int? PagoAdelanto
        {
            get
            {
                return this._PagoAdelanto;
            }
            set
            {
                if (this._PagoAdelanto != value)
                {
                    this.SendPropertyChanging("PagoAdelanto");
                    this._PagoAdelanto = value;
                    this.SendPropertyChanged("PagoAdelanto");
                }
            }
        }

        public int? Idreserva
        {
            get
            {
                return this._Idreserva;
            }
            set
            {
                if (this._Idreserva != value)
                {
                    this.SendPropertyChanging("Idreserva");
                    this._Idreserva = value;
                    this.SendPropertyChanged("Idreserva");
                }
            }
        }

        public DateTime? Backup
        {
            get
            {
                return this._Backup;
            }
            set
            {
                if (this._Backup != value)
                {
                    this.SendPropertyChanging("Backup");
                    this._Backup = value;
                    this.SendPropertyChanged("Backup");
                }
            }
        }

        public int? Cordobas
        {
            get
            {
                return this._Cordobas;
            }
            set
            {
                if (this._Cordobas != value)
                {
                    this.SendPropertyChanging("Cordobas");
                    this._Cordobas = value;
                    this.SendPropertyChanged("Cordobas");
                }
            }
        }

        public int? Dolares
        {
            get
            {
                return this._Dolares;
            }
            set
            {
                if (this._Dolares != value)
                {
                    this.SendPropertyChanging("Dolares");
                    this._Dolares = value;
                    this.SendPropertyChanged("Dolares");
                }
            }
        }

        public int? Nocontrato
        {
            get
            {
                return this._Nocontrato;
            }
            set
            {
                if (this._Nocontrato != value)
                {
                    this.SendPropertyChanging("Nocontrato");
                    this._Nocontrato = value;
                    this.SendPropertyChanged("Nocontrato");
                }
            }
        }

        public byte[]? Logo
        {
            get
            {
                return this._Logo;
            }
            set
            {
                if (this._Logo != value)
                {
                    this.SendPropertyChanging("Logo");
                    this._Logo = value;
                    this.SendPropertyChanged("Logo");
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
