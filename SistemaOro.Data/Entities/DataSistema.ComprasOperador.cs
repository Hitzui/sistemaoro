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
    public partial class ComprasOperador {

        public ComprasOperador()
        {
            OnCreated();
        }

        public string Nombre { get; set; }

        public string Numcompra { get; set; }

        public DateTime Fecha { get; set; }

        public decimal PesoTotal { get; set; }

        public decimal Total { get; set; }

        public string Codcaja { get; set; }

        public string Hora { get; set; }

        public string Kilate { get; set; }

        public decimal Peso { get; set; }

        public decimal? Importe { get; set; }

        public decimal Preciok { get; set; }

        public string Codcliente { get; set; }

        public string Codagencia { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
