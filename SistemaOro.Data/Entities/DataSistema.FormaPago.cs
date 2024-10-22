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
    public partial class FormaPago : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Idmov;

        private decimal? _Descripcion;

        private decimal? _Naturaleza;

        public FormaPago()
        {
            OnCreated();
        }

        public string Idmov
        {
            get
            {
                return this._Idmov;
            }
            set
            {
                if (this._Idmov != value)
                {
                    this.SendPropertyChanging("Idmov");
                    this._Idmov = value;
                    this.SendPropertyChanged("Idmov");
                }
            }
        }

        public decimal? Descripcion
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

        public decimal? Naturaleza
        {
            get
            {
                return this._Naturaleza;
            }
            set
            {
                if (this._Naturaleza != value)
                {
                    this.SendPropertyChanging("Naturaleza");
                    this._Naturaleza = value;
                    this.SendPropertyChanged("Naturaleza");
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
