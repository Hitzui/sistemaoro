﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 28/7/2024 4:09:41 PM
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
    public partial class Adelanto {

        public Adelanto()
        {
            this.Estado = true;
            OnCreated();
        }

        public string Idsalida { get; set; }

        public string Codcliente { get; set; }

        public string Numcompra { get; set; }

        public DateTime Fecha { get; set; }

        public decimal? Monto { get; set; }

        public decimal Saldo { get; set; }

        public decimal? Efectivo { get; set; }

        public decimal? Cheque { get; set; }

        public decimal? Transferencia { get; set; }

        public string? Codcaja { get; set; }

        public string? Usuario { get; set; }

        public string? MontoLetras { get; set; }

        public string? Hora { get; set; }

        public int? Codmoneda { get; set; }

        public bool Estado { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
