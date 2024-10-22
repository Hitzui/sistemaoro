﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 14/10/2024 9:59:25 AM
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
    public partial class Adelanto : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Idsalida;

        private string _Codcliente;

        private string _Numcompra;

        private DateTime _Fecha;

        private decimal? _Monto;

        private decimal _Saldo;

        private decimal? _Efectivo;

        private decimal? _Cheque;

        private decimal? _Transferencia;

        private string? _Codcaja;

        private string? _Usuario;

        private string? _MontoLetras;

        private string? _Hora;

        private int? _Codmoneda;

        private bool _Estado;

        private Cliente _Cliente;

        private Compra _Compra;

        public Adelanto()
        {
            this._Estado = true;
            OnCreated();
        }

        public string Idsalida
        {
            get
            {
                return this._Idsalida;
            }
            set
            {
                if (this._Idsalida != value)
                {
                    this.SendPropertyChanging("Idsalida");
                    this._Idsalida = value;
                    this.SendPropertyChanged("Idsalida");
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

        public decimal? Efectivo
        {
            get
            {
                return this._Efectivo;
            }
            set
            {
                if (this._Efectivo != value)
                {
                    this.SendPropertyChanging("Efectivo");
                    this._Efectivo = value;
                    this.SendPropertyChanged("Efectivo");
                }
            }
        }

        public decimal? Cheque
        {
            get
            {
                return this._Cheque;
            }
            set
            {
                if (this._Cheque != value)
                {
                    this.SendPropertyChanging("Cheque");
                    this._Cheque = value;
                    this.SendPropertyChanged("Cheque");
                }
            }
        }

        public decimal? Transferencia
        {
            get
            {
                return this._Transferencia;
            }
            set
            {
                if (this._Transferencia != value)
                {
                    this.SendPropertyChanging("Transferencia");
                    this._Transferencia = value;
                    this.SendPropertyChanged("Transferencia");
                }
            }
        }

        public string? Codcaja
        {
            get
            {
                return this._Codcaja;
            }
            set
            {
                if (this._Codcaja != value)
                {
                    this.SendPropertyChanging("Codcaja");
                    this._Codcaja = value;
                    this.SendPropertyChanged("Codcaja");
                }
            }
        }

        public string? Usuario
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

        public string? MontoLetras
        {
            get
            {
                return this._MontoLetras;
            }
            set
            {
                if (this._MontoLetras != value)
                {
                    this.SendPropertyChanging("MontoLetras");
                    this._MontoLetras = value;
                    this.SendPropertyChanged("MontoLetras");
                }
            }
        }

        public string? Hora
        {
            get
            {
                return this._Hora;
            }
            set
            {
                if (this._Hora != value)
                {
                    this.SendPropertyChanging("Hora");
                    this._Hora = value;
                    this.SendPropertyChanged("Hora");
                }
            }
        }

        public int? Codmoneda
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

        public bool Estado
        {
            get
            {
                return this._Estado;
            }
            set
            {
                if (this._Estado != value)
                {
                    this.SendPropertyChanging("Estado");
                    this._Estado = value;
                    this.SendPropertyChanged("Estado");
                }
            }
        }

        public virtual Cliente Cliente
        {
            get
            {
                return this._Cliente;
            }
            set
            {
                if (this._Cliente != value)
                {
                    this.SendPropertyChanging("Cliente");
                    this._Cliente = value;
                    this.SendPropertyChanged("Cliente");
                }
            }
        }

        public virtual Compra Compra
        {
            get
            {
                return this._Compra;
            }
            set
            {
                if (this._Compra != value)
                {
                    this.SendPropertyChanging("Compra");
                    this._Compra = value;
                    this.SendPropertyChanged("Compra");
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
