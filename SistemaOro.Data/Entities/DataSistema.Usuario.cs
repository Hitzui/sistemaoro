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
    public partial class Usuario {

        public Usuario()
        {
            OnCreated();
        }

        public string Usuario1 { get; set; }

        public string Codoperador { get; set; }

        public string Clave { get; set; }

        public string Nombre { get; set; }

        public string Pregunta { get; set; }

        public string Respuesta { get; set; }

        public DateTime Fcreau { get; set; }

        public int Nivel { get; set; }

        public string? Estado { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
