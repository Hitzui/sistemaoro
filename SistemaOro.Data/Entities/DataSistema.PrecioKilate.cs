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
    public partial class PrecioKilate : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _DescKilate;

        private decimal _KilatePeso;

        private decimal _PrecioKilate1;

        public PrecioKilate()
        {
            OnCreated();
        }

        public string DescKilate
        {
            get
            {
                return this._DescKilate;
            }
            set
            {
                if (this._DescKilate != value)
                {
                    this.SendPropertyChanging("DescKilate");
                    this._DescKilate = value;
                    this.SendPropertyChanged("DescKilate");
                }
            }
        }

        public decimal KilatePeso
        {
            get
            {
                return this._KilatePeso;
            }
            set
            {
                if (this._KilatePeso != value)
                {
                    this.SendPropertyChanging("KilatePeso");
                    this._KilatePeso = value;
                    this.SendPropertyChanged("KilatePeso");
                }
            }
        }

        public decimal PrecioKilate1
        {
            get
            {
                return this._PrecioKilate1;
            }
            set
            {
                if (this._PrecioKilate1 != value)
                {
                    this.SendPropertyChanging("PrecioKilate1");
                    this._PrecioKilate1 = value;
                    this.SendPropertyChanged("PrecioKilate1");
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
