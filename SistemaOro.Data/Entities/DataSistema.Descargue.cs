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
    public partial class Descargue : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private int _Dgnumdes;

        private string _Dgcodage;

        private string _Dgcodcaj;

        private string _Dgusuari;

        private int _Dgcancom;

        private decimal _Dgpesbrt;

        private decimal _Dgpesntt;

        private decimal _Dgimptcom;

        private DateTime _Dgfecdes;

        private DateTime _Dgfecgen;

        private IList<Compra> _Compras;

        public Descargue()
        {
            this._Compras = new List<Compra>();
            OnCreated();
        }

        public int Dgnumdes
        {
            get
            {
                return this._Dgnumdes;
            }
            set
            {
                if (this._Dgnumdes != value)
                {
                    this.SendPropertyChanging("Dgnumdes");
                    this._Dgnumdes = value;
                    this.SendPropertyChanged("Dgnumdes");
                }
            }
        }

        public string Dgcodage
        {
            get
            {
                return this._Dgcodage;
            }
            set
            {
                if (this._Dgcodage != value)
                {
                    this.SendPropertyChanging("Dgcodage");
                    this._Dgcodage = value;
                    this.SendPropertyChanged("Dgcodage");
                }
            }
        }

        public string Dgcodcaj
        {
            get
            {
                return this._Dgcodcaj;
            }
            set
            {
                if (this._Dgcodcaj != value)
                {
                    this.SendPropertyChanging("Dgcodcaj");
                    this._Dgcodcaj = value;
                    this.SendPropertyChanged("Dgcodcaj");
                }
            }
        }

        public string Dgusuari
        {
            get
            {
                return this._Dgusuari;
            }
            set
            {
                if (this._Dgusuari != value)
                {
                    this.SendPropertyChanging("Dgusuari");
                    this._Dgusuari = value;
                    this.SendPropertyChanged("Dgusuari");
                }
            }
        }

        public int Dgcancom
        {
            get
            {
                return this._Dgcancom;
            }
            set
            {
                if (this._Dgcancom != value)
                {
                    this.SendPropertyChanging("Dgcancom");
                    this._Dgcancom = value;
                    this.SendPropertyChanged("Dgcancom");
                }
            }
        }

        public decimal Dgpesbrt
        {
            get
            {
                return this._Dgpesbrt;
            }
            set
            {
                if (this._Dgpesbrt != value)
                {
                    this.SendPropertyChanging("Dgpesbrt");
                    this._Dgpesbrt = value;
                    this.SendPropertyChanged("Dgpesbrt");
                }
            }
        }

        public decimal Dgpesntt
        {
            get
            {
                return this._Dgpesntt;
            }
            set
            {
                if (this._Dgpesntt != value)
                {
                    this.SendPropertyChanging("Dgpesntt");
                    this._Dgpesntt = value;
                    this.SendPropertyChanged("Dgpesntt");
                }
            }
        }

        public decimal Dgimptcom
        {
            get
            {
                return this._Dgimptcom;
            }
            set
            {
                if (this._Dgimptcom != value)
                {
                    this.SendPropertyChanging("Dgimptcom");
                    this._Dgimptcom = value;
                    this.SendPropertyChanged("Dgimptcom");
                }
            }
        }

        public DateTime Dgfecdes
        {
            get
            {
                return this._Dgfecdes;
            }
            set
            {
                if (this._Dgfecdes != value)
                {
                    this.SendPropertyChanging("Dgfecdes");
                    this._Dgfecdes = value;
                    this.SendPropertyChanged("Dgfecdes");
                }
            }
        }

        public DateTime Dgfecgen
        {
            get
            {
                return this._Dgfecgen;
            }
            set
            {
                if (this._Dgfecgen != value)
                {
                    this.SendPropertyChanging("Dgfecgen");
                    this._Dgfecgen = value;
                    this.SendPropertyChanged("Dgfecgen");
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
