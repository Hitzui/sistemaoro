using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using OfficeOpenXml;
using SistemaOro.Data.Entities;

namespace SistemaOro.Forms.Services;

public class ReadExcelTipoCambio
{
    public List<TipoCambio> ReadExcelFile(string filePath)
    {
        var tipoCambioList = new List<TipoCambio>();

        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[0];
        if (worksheet == null)
            return null;

        // Leer filas (asumiendo que la primera fila contiene los encabezados)
        for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
        {
            var fechaCell = worksheet.Cells[rowNum, 1];
            var tipoCambioCell = worksheet.Cells[rowNum, 2];

            if (DateTime.TryParse(fechaCell.Text, out var fecha) &&
                decimal.TryParse(tipoCambioCell.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var tipoCambio))
            {
                var tipoCambioObj = new TipoCambio
                {
                    Fecha = fecha,
                    Tipocambio = tipoCambio,
                    Hora = string.Empty,
                    PrecioOro = decimal.Zero
                };
                tipoCambioList.Add(tipoCambioObj);
            }
        }

        return tipoCambioList;
    }
}