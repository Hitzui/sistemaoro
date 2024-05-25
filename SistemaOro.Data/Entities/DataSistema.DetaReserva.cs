﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 24/5/2024 23:11:11
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
    public partial class DetaReserva {

        public DetaReserva()
        {
            OnCreated();
        }

        public int IdReserva { get; set; }

        public int linea { get; set; }

        public DateOnly Fecha { get; set; }

        public decimal Entregadas { get; set; }

        public decimal Diferencias { get; set; }

        public string Usuario { get; set; }

        public string Hora { get; set; }

        public virtual ReservaOro ReservaOro { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          DetaReserva toCompare = obj as DetaReserva;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.IdReserva, toCompare.IdReserva))
            return false;
          if (!Object.Equals(this.linea, toCompare.linea))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + IdReserva.GetHashCode();
          hashCode = (hashCode * 7) + linea.GetHashCode();
          return hashCode;
        }

        #endregion
    }

}
