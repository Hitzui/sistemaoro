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
    public partial class RubrosCaja : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Rucodrub;

        private string _Rucodope;

        private string _Rudescri;

        private string _Rudeha;

        public RubrosCaja()
        {
            OnCreated();
        }

        public string Rucodrub
        {
            get
            {
                return this._Rucodrub;
            }
            set
            {
                if (this._Rucodrub != value)
                {
                    this.SendPropertyChanging("Rucodrub");
                    this._Rucodrub = value;
                    this.SendPropertyChanged("Rucodrub");
                }
            }
        }

        public string Rucodope
        {
            get
            {
                return this._Rucodope;
            }
            set
            {
                if (this._Rucodope != value)
                {
                    this.SendPropertyChanging("Rucodope");
                    this._Rucodope = value;
                    this.SendPropertyChanged("Rucodope");
                }
            }
        }

        public string Rudescri
        {
            get
            {
                return this._Rudescri;
            }
            set
            {
                if (this._Rudescri != value)
                {
                    this.SendPropertyChanging("Rudescri");
                    this._Rudescri = value;
                    this.SendPropertyChanged("Rudescri");
                }
            }
        }

        public string Rudeha
        {
            get
            {
                return this._Rudeha;
            }
            set
            {
                if (this._Rudeha != value)
                {
                    this.SendPropertyChanging("Rudeha");
                    this._Rudeha = value;
                    this.SendPropertyChanged("Rudeha");
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
