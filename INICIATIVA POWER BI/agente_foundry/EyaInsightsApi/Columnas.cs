using EyaInsightsApi;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

/// <summary>Utilidades compartidas por la exportación a Excel (Excel.cs) y las gráficas (Graficas.cs).</summary>
static class Columnas
{
    /// <summary>Mismas opciones JSON que la API (snake_case, sin nulos): así el libro y la gráfica se calculan sobre el JSON que ve el asistente.</summary>
    public static readonly JsonSerializerOptions JsonApi = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static JsonNode ANodo(Respuesta r) => JsonSerializer.SerializeToNode(r, JsonApi)!;

    static readonly Dictionary<string, string> Titulos = new()
    {
        ["nombre"] = "Nombre", ["escuela"] = "Escuela", ["materia"] = "Materia", ["periodo"] = "Período",
        ["pruebas_asignadas"] = "Pruebas asignadas", ["pruebas_completadas"] = "Pruebas completadas",
        ["porcentaje_completado"] = "% completado", ["porcentaje_del_total_completadas"] = "% del total de completadas",
        ["conexiones"] = "Conexiones", ["usuarios_conectados"] = "Usuarios conectados",
        ["porcentaje_del_total"] = "% del total", ["conexiones_por_usuario"] = "Conexiones por usuario", ["ultima_conexion"] = "Última conexión",
        ["pruebas_completadas_a"] = "Pruebas completadas (A)", ["pruebas_completadas_b"] = "Pruebas completadas (B)",
        ["conexiones_a"] = "Conexiones (A)", ["conexiones_b"] = "Conexiones (B)",
        ["diferencia"] = "Diferencia (B − A)", ["variacion_pct"] = "Variación %",
        ["diferencia_con_periodo_anterior"] = "Diferencia con el período anterior",
        ["conexiones_acumuladas"] = "Conexiones acumuladas", ["usuarios_nuevos"] = "Usuarios nuevos", ["usuarios_acumulados"] = "Usuarios acumulados",
        ["periodo_desde"] = "Período académico desde", ["periodo_hasta"] = "Período académico hasta",
        ["maestros"] = "Maestros", ["con_conexiones"] = "Con conexiones", ["con_planes"] = "Con planes",
        ["planes_con_datos"] = "Planes con datos", ["con_grupos"] = "Con grupos", ["con_asignaciones"] = "Con asignaciones",
        ["promedio_solo_realizadas"] = "Promedio % Full Quiz (solo realizadas)", ["promedio_todas_las_asignadas"] = "Promedio % Full Quiz (todas las asignadas)",
        ["usuarios_activos"] = "Usuarios activos", ["porcentaje_participacion"] = "% de participación",
    };

    /// <summary>Encabezado legible de una columna; si no está en el diccionario: `_`→espacio y mayúscula inicial.</summary>
    public static string Titulo(string clave)
    {
        if (Titulos.TryGetValue(clave, out var t)) return t;
        var s = clave.Replace('_', ' ');
        return s.Length == 0 ? s : char.ToUpper(s[0], CultureInfo.InvariantCulture) + s[1..];
    }

    /// <summary>¿La columna es un porcentaje (se escribe con formato 0.0)?</summary>
    public static bool EsPorcentaje(string clave) =>
        clave.StartsWith("porcentaje", StringComparison.Ordinal) || clave.EndsWith("_pct", StringComparison.Ordinal) || clave.StartsWith("promedio", StringComparison.Ordinal);

    /// <summary>Aplana un objeto anidado a pares `clave.subclave` (valores escalares como texto invariante; null se omite).</summary>
    public static IEnumerable<(string Clave, JsonNode Valor)> Aplanar(JsonNode? nodo, string prefijo = "")
    {
        if (nodo is JsonObject o)
        {
            foreach (var (k, v) in o)
                foreach (var par in Aplanar(v, prefijo.Length == 0 ? k : prefijo + "." + k)) yield return par;
        }
        else if (nodo is JsonArray a)
        {
            for (var i = 0; i < a.Count; i++)
                foreach (var par in Aplanar(a[i], $"{prefijo}[{i + 1}]")) yield return par;
        }
        else if (nodo is not null) yield return (prefijo, nodo);
    }

    /// <summary>Texto de un valor escalar JSON (números con punto decimal, sin notación regional).</summary>
    public static string Texto(JsonNode? n) => n switch
    {
        null => "",
        JsonValue v when v.GetValueKind() == JsonValueKind.String => v.GetValue<string>(),
        JsonValue v when v.GetValueKind() == JsonValueKind.True => "sí",
        JsonValue v when v.GetValueKind() == JsonValueKind.False => "no",
        _ => n.ToJsonString(JsonApi).Trim('"'),
    };

    /// <summary>Número de un nodo JSON (null si no es número).</summary>
    public static double? Numero(JsonNode? n)
    {
        if (n is not JsonValue v || v.GetValueKind() != JsonValueKind.Number) return null;
        return double.Parse(v.ToJsonString(), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Texto de la hoja «Datos» cuando no hay filas. Si el período pedido empieza DESPUÉS del último día con datos, dice esa causa
    /// (consultas con «datos_hasta»: pruebas o conexiones); si no, el mensaje genérico.
    /// </summary>
    public static string MensajeSinResultados(JsonNode respuesta)
    {
        const string generico = "Sin resultados para este filtro (ver la hoja Referencias).";
        if (respuesta["datos_hasta"]?.GetValue<string>() is not { } hasta
            || !DateOnly.TryParseExact(hasta, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var limite)) return generico;
        // Un período (desde/hasta) o dos (a y b, comparaciones): la causa solo vale si TODOS empiezan después de los datos.
        var inicios = new List<DateOnly>();
        void Inicio(JsonNode? p)
        {
            if (p?["desde"]?.GetValue<string>() is { } s && DateOnly.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) inicios.Add(d);
        }
        var periodo = respuesta["periodo"];
        if (periodo?["desde"] is not null) Inicio(periodo); else { Inicio(periodo?["a"]); Inicio(periodo?["b"]); }
        if (inicios.Count == 0 || inicios.Any(d => d <= limite)) return generico;
        var consulta = respuesta["consulta"]?.GetValue<string>() ?? "";
        var tipo = consulta is "uso_por_area" or "comparar_areas" or "promedio_full_quiz" ? "pruebas" : "conexiones";
        var empieza = inicios.Count == 1 ? $"el período pedido empieza el {inicios[0]:yyyy-MM-dd}"
                                         : $"los períodos pedidos empiezan el {inicios[0]:yyyy-MM-dd} y el {inicios[1]:yyyy-MM-dd}";
        return $"No hay datos de {tipo} después del {limite:yyyy-MM-dd}; {empieza} (ver la hoja Referencias).";
    }

    /// <summary>¿El resultado quedó cortado (hay más filas que las devueltas)? Lo dice la nota o el resumen de la consulta.</summary>
    public static bool HayMasFilas(JsonNode respuesta)
    {
        var nota = respuesta["nota"]?.GetValue<string>() ?? "";
        if (nota.Contains("HAY MÁS FILAS", StringComparison.Ordinal)) return true;
        var filas = respuesta["resultados"]?.AsArray().Count ?? 0;
        var resumen = respuesta["resumen"] as JsonObject;
        var todas = respuesta["filtros"]?["escuela"]?.GetValue<string>() is null or "todas";
        foreach (var (clave, soloSinEscuela) in new[] { ("maestros_que_cumplen_el_filtro", false), ("escuelas_que_cumplen_el_filtro", false),
                                                       ("maestros_considerados", false), ("escuelas_consideradas", true),
                                                       ("combinaciones_escuela_periodo", false), ("areas_consideradas", false), ("escuelas_comparadas", false) })
        {
            if (resumen?[clave] is { } n && (!soloSinEscuela || todas)) return Numero(n) > filas;
        }
        return false;
    }

    static string Limpio(string s)
    {
        var sb = new StringBuilder();
        foreach (var ch in s.ToLowerInvariant()) sb.Append(ch is (>= 'a' and <= 'z') or (>= '0' and <= '9') or '_' or '-' ? ch : '_');
        return sb.ToString();
    }

    /// <summary>`{consulta}_{desde}_{hasta}.xlsx` (o `_{fecha}` / `_a_{…}_b_{…}`), solo [a-z0-9_-].</summary>
    public static string NombreArchivo(JsonNode respuesta, string extension = "xlsx")
    {
        var partes = new List<string> { respuesta["consulta"]?.GetValue<string>() ?? "consulta" };
        var p = respuesta["periodo"];
        if (p is JsonObject po)
        {
            if (po["a"] is not null && po["b"] is not null)
            {
                partes.Add("a"); partes.Add(Texto(po["a"]!["desde"])); partes.Add(Texto(po["a"]!["hasta"]));
                partes.Add("b"); partes.Add(Texto(po["b"]!["desde"])); partes.Add(Texto(po["b"]!["hasta"]));
            }
            else foreach (var k in new[] { "desde", "hasta", "fecha", "periodo_desde" })
                if (po[k] is { } v) partes.Add(Texto(v));
        }
        return Limpio(string.Join('_', partes.Where(x => x.Length > 0))) + "." + extension;
    }
}
