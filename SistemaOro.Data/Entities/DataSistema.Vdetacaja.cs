﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 11/7/2024 3:50:38 PM
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
    public partial class Vdetacaja {

        public Vdetacaja()
        {
            OnCreated();
        }

        public decimal Idcaja { get; set; }

        public int Idmov { get; set; }

        public string Hora { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Concepto { get; set; }

        public decimal? Efectivo { get; set; }

        public decimal? Cheque { get; set; }

        public decimal? Transferencia { get; set; }

        public string? Codcaja { get; set; }

        public string? Referencia { get; set; }

        public decimal? Sinicial { get; set; }

        public decimal? Sfinal { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
