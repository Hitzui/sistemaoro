﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 12/8/2024 11:20:24 AM
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
    public partial class Cliente {

        public Cliente()
        {
            this.Ocupacion2 = @"Pequeño Minero";
            OnCreated();
        }

        public string Codcliente { get; set; }

        public string Nombres { get; set; }

        public string? Apellidos { get; set; }

        public string Numcedula { get; set; }

        public DateTime? FEmision { get; set; }

        public DateTime? FVencimiento { get; set; }

        public string Direccion { get; set; }

        public DateTime? FNacimiento { get; set; }

        public string? Estadocivil { get; set; }

        public string? Ciudad { get; set; }

        public string Telefono { get; set; }

        public string? Celular { get; set; }

        public string? Email { get; set; }

        public DateTime? FIngreso { get; set; }

        public string? Ocupacion { get; set; }

        public string? DireccionNegocio { get; set; }

        public string? TiempoNeg { get; set; }

        public string? OtraAe { get; set; }

        public string? DescOtra { get; set; }

        public string? NomCuenta { get; set; }

        public string? NumCuenta { get; set; }

        public string? NomBanco { get; set; }

        public decimal? MontoMensual { get; set; }

        public decimal? TotalOperaciones { get; set; }

        public string? ActuaPor { get; set; }

        public string? NombreTercero { get; set; }

        public string? DireccionTercero { get; set; }

        public int? Pica { get; set; }

        public string? Ocupacion2 { get; set; }

        public int? Idtipodocumento { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
