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
    public partial class Mcaja {

        public Mcaja()
        {
            OnCreated();
        }

        public int Idcaja { get; set; }

        public string Codcaja { get; set; }

        public string? Codagencia { get; set; }

        public DateTime? Fecha { get; set; }

        public decimal? Sinicial { get; set; }

        public decimal? Entrada { get; set; }

        public decimal? Salida { get; set; }

        public decimal? Sfinal { get; set; }

        public int? Estado { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
