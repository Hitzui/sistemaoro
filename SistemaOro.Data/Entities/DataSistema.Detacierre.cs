﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 28/7/2024 4:09:42 PM
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
    public partial class Detacierre {

        public Detacierre()
        {
            OnCreated();
        }

        public int Codcierre { get; set; }

        public string Numcompra { get; set; }

        public decimal Onzas { get; set; }

        public decimal Saldo { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Cantidad { get; set; }

        public string Codagencia { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          Detacierre toCompare = obj as Detacierre;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.Codcierre, toCompare.Codcierre))
            return false;
          if (!Object.Equals(this.Numcompra, toCompare.Numcompra))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + Codcierre.GetHashCode();
          hashCode = (hashCode * 7) + Numcompra.GetHashCode();
          return hashCode;
        }

        #endregion
    }

}
