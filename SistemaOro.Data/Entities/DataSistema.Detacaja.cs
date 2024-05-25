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
    public partial class Detacaja {

        public Detacaja()
        {
            this.Tipocambio = 1m;
            OnCreated();
        }

        public int Idcaja { get; set; }

        public DateTime Fecha { get; set; }

        public int Idmov { get; set; }

        public string Hora { get; set; }

        public string? Concepto { get; set; }

        public decimal? Efectivo { get; set; }

        public string? Referencia { get; set; }

        public decimal? Cheque { get; set; }

        public decimal? Transferencia { get; set; }

        public string? Codcaja { get; set; }

        public decimal? Tipocambio { get; set; }

        public virtual Caja Caja { get; set; }

        public virtual Mcaja Mcaja { get; set; }

        public virtual Movcaja Movcaja { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          Detacaja toCompare = obj as Detacaja;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.Idcaja, toCompare.Idcaja))
            return false;
          if (!Object.Equals(this.Fecha, toCompare.Fecha))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + Idcaja.GetHashCode();
          hashCode = (hashCode * 7) + Fecha.GetHashCode();
          return hashCode;
        }

        #endregion
    }

}
