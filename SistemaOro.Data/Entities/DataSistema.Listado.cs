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
    public partial class Listado {

        public Listado()
        {
            OnCreated();
        }

        public int Solicitud { get; set; }

        public int? Ranking { get; set; }

        public string? Cliente { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Recibo { get; set; }

        public decimal? Total { get; set; }

        public string? Codigo { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
