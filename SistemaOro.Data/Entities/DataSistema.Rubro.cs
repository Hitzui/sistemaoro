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
    public partial class Rubro {

        public Rubro()
        {
            this.Movcajas = new List<Movcaja>();
            OnCreated();
        }

        public int Codrubro { get; set; }

        public string Descrubro { get; set; }

        public int? Naturaleza { get; set; }

        public virtual IList<Movcaja> Movcajas { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
