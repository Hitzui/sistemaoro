﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 29/4/2024 02:55 p. m.
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
    public partial class Id {

        public Id()
        {
            OnCreated();
        }

        public int Codcliente { get; set; }

        public int? Codagencia { get; set; }

        public int? Numcompra { get; set; }

        public int? Idadelanto { get; set; }

        public int? Idcompras { get; set; }

        public int? IdAdelantos { get; set; }

        public int? SaldoAnterior { get; set; }

        public int? CierreCompra { get; set; }

        public int? PrestamoEgreso { get; set; }

        public int? PrestamoIngreso { get; set; }

        public int? AnularCompra { get; set; }

        public int? AnularAdelanto { get; set; }

        public bool? VariasCompras { get; set; }

        public string? Recibe { get; set; }

        public int? PagoAdelanto { get; set; }

        public int? Idreserva { get; set; }

        public DateTime? Backup { get; set; }

        public int? Cordobas { get; set; }

        public int? Dolares { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
