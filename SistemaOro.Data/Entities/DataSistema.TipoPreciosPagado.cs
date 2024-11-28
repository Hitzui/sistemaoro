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
    public partial class TipoPreciosPagado : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Codprecio;

        private string _Descripcion;

        private string _Pesoinicial;

        private string _Pesofinal;

        public TipoPreciosPagado()
        {
            OnCreated();
        }

        public string Codprecio
        {
            get
            {
                return this._Codprecio;
            }
            set
            {
                if (this._Codprecio != value)
                {
                    this.SendPropertyChanging("Codprecio");
                    this._Codprecio = value;
                    this.SendPropertyChanged("Codprecio");
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

        public string Pesoinicial
        {
            get
            {
                return this._Pesoinicial;
            }
            set
            {
                if (this._Pesoinicial != value)
                {
                    this.SendPropertyChanging("Pesoinicial");
                    this._Pesoinicial = value;
                    this.SendPropertyChanged("Pesoinicial");
                }
            }
        }

        public string Pesofinal
        {
            get
            {
                return this._Pesofinal;
            }
            set
            {
                if (this._Pesofinal != value)
                {
                    this.SendPropertyChanging("Pesofinal");
                    this._Pesofinal = value;
                    this.SendPropertyChanged("Pesofinal");
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
