﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 16/9/2024 6:30:04 AM
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
    public partial class TipoDocumento : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Idtipodocumento;

        private string _Nombre;

        private string _Simbolo;

        public TipoDocumento()
        {
            OnCreated();
        }

        public int Idtipodocumento
        {
            get
            {
                return this._Idtipodocumento;
            }
            set
            {
                if (this._Idtipodocumento != value)
                {
                    this.SendPropertyChanging("Idtipodocumento");
                    this._Idtipodocumento = value;
                    this.SendPropertyChanged("Idtipodocumento");
                }
            }
        }

        public string Nombre
        {
            get
            {
                return this._Nombre;
            }
            set
            {
                if (this._Nombre != value)
                {
                    this.SendPropertyChanging("Nombre");
                    this._Nombre = value;
                    this.SendPropertyChanged("Nombre");
                }
            }
        }

        public string Simbolo
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
