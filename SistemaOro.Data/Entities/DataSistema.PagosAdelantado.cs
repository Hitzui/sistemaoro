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
    public partial class PagosAdelantado {

        public PagosAdelantado()
        {
            OnCreated();
        }

        public int IdPagoefec { get; set; }

        public string Idingreso { get; set; }

        public string? Codagencia { get; set; }

        public DateTime FechaopParcial { get; set; }

        public decimal ValorParcialpagado { get; set; }

        public string Usuario { get; set; }

        public DateTime HoraOp { get; set; }

        public string EstadoOp { get; set; }

        public string? CajaRegadel { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
