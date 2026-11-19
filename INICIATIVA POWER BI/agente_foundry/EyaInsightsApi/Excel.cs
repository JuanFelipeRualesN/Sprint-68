using System.Text.Json;
using System.Text.Json.Nodes;
using ClosedXML.Excel;

/// <summary>
/// Libro .xlsx de una consulta: hoja «Datos» (una fila por resultado) y hoja «Referencias» (de dónde salen las cifras).
/// Se calcula sobre el JSON de la respuesta (el mismo que ve el asistente). Todo texto se escribe como texto, nunca como fórmula.
/// </summary>
static class Excel
{
    static readonly XLColor Cabecera = XLColor.FromHtml("#1F3864");

    public static byte[] Libro(JsonNode respuesta)
    {
        using var wb = new XLWorkbook();
        Datos(wb.Worksheets.Add("Datos"), respuesta);
        Referencias(wb.Worksheets.Add("Referencias"), respuesta);
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    static void Texto(IXLCell c, string valor) { c.Value = valor; c.Style.NumberFormat.Format = "@"; }

    static void Datos(IXLWorksheet ws, JsonNode respuesta)
    {
        var filas = respuesta["resultados"]?.AsArray() ?? [];
        var claves = new List<string>();                      // orden de aparición (las filas omiten los nulos)
        foreach (var f in filas)
            foreach (var (k, _) in Columnas.Aplanar(f))
                if (!claves.Contains(k)) claves.Add(k);

        for (var j = 0; j < claves.Count; j++)
        {
            var c = ws.Cell(1, j + 1);
            Texto(c, Columnas.Titulo(claves[j]));
            c.Style.Font.Bold = true; c.Style.Font.FontColor = XLColor.White; c.Style.Fill.BackgroundColor = Cabecera;
            c.Style.Alignment.WrapText = true; c.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }
        for (var i = 0; i < filas.Count; i++)
        {
            var plano = Columnas.Aplanar(filas[i]).ToDictionary(p => p.Clave, p => p.Valor);
            for (var j = 0; j < claves.Count; j++)
            {
                if (!plano.TryGetValue(claves[j], out var v)) continue;        // celda vacía = sin dato (null)
                var c = ws.Cell(i + 2, j + 1);
                if (Columnas.Numero(v) is { } n)
                {
                    c.Value = n;
                    c.Style.NumberFormat.Format = Columnas.EsPorcentaje(claves[j]) || claves[j].Contains("variacion") ? "0.0" : n % 1 == 0 ? "#,##0" : "0.0##";
                }
                else Texto(c, Columnas.Texto(v));
            }
        }
        if (claves.Count == 0)
        {
            Texto(ws.Cell(1, 1), "Sin resultados para este filtro (ver la hoja Referencias).");
            ws.Column(1).Width = 60;
        }
        else
        {
            ws.SheetView.FreezeRows(1);
            if (filas.Count > 0) ws.Range(1, 1, filas.Count + 1, claves.Count).SetAutoFilter();
            ws.Columns(1, claves.Count).AdjustToContents(1, Math.Min(filas.Count + 1, 500));
            foreach (var col in ws.ColumnsUsed()) if (col.Width > 60) col.Width = 60; else if (col.Width < 12) col.Width = 12;
        }
    }

    static void Referencias(IXLWorksheet ws, JsonNode r)
    {
        var fila = 1;
        void Linea(string concepto, string valor)
        {
            Texto(ws.Cell(fila, 1), concepto); ws.Cell(fila, 1).Style.Font.Bold = true;
            Texto(ws.Cell(fila, 2), valor); ws.Cell(fila, 2).Style.Alignment.WrapText = true;
            ws.Cell(fila, 1).Style.Alignment.Vertical = ws.Cell(fila, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            fila++;
        }
        void Titulo(string t)
        {
            Texto(ws.Cell(fila, 1), t);
            var rango = ws.Range(fila, 1, fila, 2);
            rango.Style.Font.Bold = true; rango.Style.Font.FontColor = XLColor.White; rango.Style.Fill.BackgroundColor = Cabecera;
            fila++;
        }

        var nFilas = r["resultados"]?.AsArray().Count ?? 0;
        var cortado = Columnas.HayMasFilas(r);
        Titulo("Referencias de esta exportación");
        Linea("Consulta", Columnas.Texto(r["consulta"]));
        Linea("Fuente", Columnas.Texto(r["fuente"]));
        Linea("Columna de fecha", Columnas.Texto(r["columna_fecha"]));
        foreach (var (k, v) in Columnas.Aplanar(r["periodo"], "")) Linea(k.Length == 0 ? "Período" : "Período · " + Rotulo(k), Columnas.Texto(v));
        foreach (var (k, v) in Columnas.Aplanar(r["filtros"]))
            Linea(k == "cuentas_de_prueba" ? "Cuentas de prueba" : "Filtro · " + Rotulo(k), Columnas.Texto(v));
        Linea("Datos hasta", r["datos_hasta"] is { } dh ? Columnas.Texto(dh) : "no aplica");
        Linea("Filas exportadas", nFilas.ToString(System.Globalization.CultureInfo.InvariantCulture));
        Linea("Lista completa", cortado
            ? "NO: hay más filas que las exportadas (ver la nota). No presentes la lista como completa."
            : "Sí: están todas las filas que cumplen el filtro.");
        Linea("Generado en (UTC)", Columnas.Texto(r["generado_en"]));
        Linea("Nota", Columnas.Texto(r["nota"]) is { Length: > 0 } nota ? nota : "(sin nota)");
        if (r["resumen"] is JsonObject)
        {
            Titulo("Resumen de la consulta");
            foreach (var (k, v) in Columnas.Aplanar(r["resumen"])) Linea(Rotulo(k), Columnas.Texto(v));
        }
        ws.Column(1).Width = 34; ws.Column(2).Width = 100;
    }

    /// <summary>`clave_con_guiones` → «clave con guiones» (el resumen y los filtros no tienen diccionario propio).</summary>
    static string Rotulo(string clave) => clave.Replace('_', ' ');
}
