﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 11/4/2024 10:05 a. m.
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace SistemaOro.Data.Entities
{
    public partial class Tmpprecio {

        public Tmpprecio()
        {
            OnCreated();
        }

        public int Codcierre { get; set; }

        public byte Linea { get; set; }

        public string Codcliente { get; set; }

        public decimal Cantidad { get; set; }

        public DateTime Fecha { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          Tmpprecio toCompare = obj as Tmpprecio;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.Codcierre, toCompare.Codcierre))
            return false;
          if (!Object.Equals(this.Linea, toCompare.Linea))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + Codcierre.GetHashCode();
          hashCode = (hashCode * 7) + Linea.GetHashCode();
          return hashCode;
        }

        #endregion
    }

}
