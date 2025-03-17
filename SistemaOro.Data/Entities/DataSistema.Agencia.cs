﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 15/3/2025 11:51:12 AM
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
    public partial class Agencia : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Codagencia;

        private string _Nomagencia;

        private string _Diragencia;

        private string _Disagencia;

        private string _Telagencia;

        private int? _Numcompra;

        private byte[]? _Logo;

        private string? _Ruc;

        private IList<Compra> _Compras;

        private IList<Mcaja> _Mcajas;

        public Agencia()
        {
            this._Compras = new List<Compra>();
            this._Mcajas = new List<Mcaja>();
            OnCreated();
        }

        public string Codagencia
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

        public string Nomagencia
        {
            get
            {
                return this._Nomagencia;
            }
            set
            {
                if (this._Nomagencia != value)
                {
                    this.SendPropertyChanging("Nomagencia");
                    this._Nomagencia = value;
                    this.SendPropertyChanged("Nomagencia");
                }
            }
        }

        public string Diragencia
        {
            get
            {
                return this._Diragencia;
            }
            set
            {
                if (this._Diragencia != value)
                {
                    this.SendPropertyChanging("Diragencia");
                    this._Diragencia = value;
                    this.SendPropertyChanged("Diragencia");
                }
            }
        }

        public string Disagencia
        {
            get
            {
                return this._Disagencia;
            }
            set
            {
                if (this._Disagencia != value)
                {
                    this.SendPropertyChanging("Disagencia");
                    this._Disagencia = value;
                    this.SendPropertyChanged("Disagencia");
                }
            }
        }

        public string Telagencia
        {
            get
            {
                return this._Telagencia;
            }
            set
            {
                if (this._Telagencia != value)
                {
                    this.SendPropertyChanging("Telagencia");
                    this._Telagencia = value;
                    this.SendPropertyChanged("Telagencia");
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

        public string? Ruc
        {
            get
            {
                return this._Ruc;
            }
            set
            {
                if (this._Ruc != value)
                {
                    this.SendPropertyChanging("Ruc");
                    this._Ruc = value;
                    this.SendPropertyChanged("Ruc");
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

        public virtual IList<Mcaja> Mcajas
        {
            get
            {
                return this._Mcajas;
            }
            set
            {
                this._Mcajas = value;
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
