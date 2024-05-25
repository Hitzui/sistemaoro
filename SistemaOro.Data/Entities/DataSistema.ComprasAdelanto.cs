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
    public partial class ComprasAdelanto {

        public ComprasAdelanto()
        {
            OnCreated();
        }

        public int IdcomprasAdelantos { get; set; }

        public string Numcompra { get; set; }

        public string Idadelanto { get; set; }

        public string? Codcliente { get; set; }

        public decimal Sinicial { get; set; }

        public decimal Monto { get; set; }

        public decimal Sfinal { get; set; }

        public DateTime Fecha { get; set; }

        public string Codcaja { get; set; }

        public string Usuario { get; set; }

        public TimeSpan Hora { get; set; }

        public string? Codagencia { get; set; }

        public int? Codmoneda { get; set; }

        public virtual Agencia Agencia { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Adelanto Adelanto { get; set; }

        public virtual Caja Caja { get; set; }

        public virtual Compra Compra { get; set; }

        public virtual Moneda Moneda { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
