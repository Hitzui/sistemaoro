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
    public partial class Usuario : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private string _Codoperador;

        private string _Username;

        private string _Clave;

        private string _Nombre;

        private string _Pregunta;

        private string _Respuesta;

        private DateTime _Fcreau;

        private Nivel _Nivel;

        private string? _Estado;

        private IList<Compra> _Compras;

        public Usuario()
        {
            this._Compras = new List<Compra>();
            OnCreated();
        }

        public string Codoperador
        {
            get
            {
                return this._Codoperador;
            }
            set
            {
                if (this._Codoperador != value)
                {
                    this.SendPropertyChanging("Codoperador");
                    this._Codoperador = value;
                    this.SendPropertyChanged("Codoperador");
                }
            }
        }

        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                if (this._Username != value)
                {
                    this.SendPropertyChanging("Username");
                    this._Username = value;
                    this.SendPropertyChanged("Username");
                }
            }
        }

        public string Clave
        {
            get
            {
                return this._Clave;
            }
            set
            {
                if (this._Clave != value)
                {
                    this.SendPropertyChanging("Clave");
                    this._Clave = value;
                    this.SendPropertyChanged("Clave");
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

        public string Pregunta
        {
            get
            {
                return this._Pregunta;
            }
            set
            {
                if (this._Pregunta != value)
                {
                    this.SendPropertyChanging("Pregunta");
                    this._Pregunta = value;
                    this.SendPropertyChanged("Pregunta");
                }
            }
        }

        public string Respuesta
        {
            get
            {
                return this._Respuesta;
            }
            set
            {
                if (this._Respuesta != value)
                {
                    this.SendPropertyChanging("Respuesta");
                    this._Respuesta = value;
                    this.SendPropertyChanged("Respuesta");
                }
            }
        }

        public DateTime Fcreau
        {
            get
            {
                return this._Fcreau;
            }
            set
            {
                if (this._Fcreau != value)
                {
                    this.SendPropertyChanging("Fcreau");
                    this._Fcreau = value;
                    this.SendPropertyChanged("Fcreau");
                }
            }
        }

        public Nivel Nivel
        {
            get
            {
                return this._Nivel;
            }
            set
            {
                if (this._Nivel != value)
                {
                    this.SendPropertyChanging("Nivel");
                    this._Nivel = value;
                    this.SendPropertyChanged("Nivel");
                }
            }
        }

        public string? Estado
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
