﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 13/4/2024 12:33 p. m.
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
    public partial class CierrePrecio {

        public CierrePrecio()
        {
            OnCreated();
        }

        public int Codcierre { get; set; }

        public string Codcliente { get; set; }

        public bool Status { get; set; }

        public decimal OnzasFinas { get; set; }

        public decimal GramosFinos { get; set; }

        public decimal PrecioOro { get; set; }

        public decimal PrecioBase { get; set; }

        public decimal SaldoOnzas { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Margen { get; set; }

        public virtual Cliente Cliente { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
