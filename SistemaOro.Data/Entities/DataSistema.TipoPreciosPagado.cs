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
    public partial class TipoPreciosPagado {

        public TipoPreciosPagado()
        {
            OnCreated();
        }

        public string Codprecio { get; set; }

        public string Descripcion { get; set; }

        public string Pesoinicial { get; set; }

        public string Pesofinal { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
