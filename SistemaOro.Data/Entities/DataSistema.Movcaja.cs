﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 15/4/2025 6:49:34 AM
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
    public partial class Movcaja : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Idmov;

        private string _Descripcion;

        private int? _Codrubro;

        private IList<Detacaja> _Detacajas;

        private Rubro _Rubro;

        public Movcaja()
        {
            this._Detacajas = new List<Detacaja>();
            OnCreated();
        }

        public int Idmov
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

        public int? Codrubro
        {
            get
            {
                return this._Codrubro;
            }
            set
            {
                if (this._Codrubro != value)
                {
                    this.SendPropertyChanging("Codrubro");
                    this._Codrubro = value;
                    this.SendPropertyChanged("Codrubro");
                }
            }
        }

        public virtual IList<Detacaja> Detacajas
        {
            get
            {
                return this._Detacajas;
            }
            set
            {
                this._Detacajas = value;
            }
        }

        public virtual Rubro Rubro
        {
            get
            {
                return this._Rubro;
            }
            set
            {
                if (this._Rubro != value)
                {
                    this.SendPropertyChanging("Rubro");
                    this._Rubro = value;
                    this.SendPropertyChanged("Rubro");
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
