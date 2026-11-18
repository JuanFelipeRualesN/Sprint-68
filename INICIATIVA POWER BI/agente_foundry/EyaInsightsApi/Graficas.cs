using System.Text.Json.Nodes;

/// <summary>
/// Especificación de gráfica (JSON) derivada de forma determinista del resultado de una consulta (tabla D9 del plan de la Fase 4).
/// No calcula nada nuevo: <c>valores</c> copia tal cual los números de <c>resultados</c> (los null quedan null). Dibuja el cliente.
/// </summary>
static class Graficas
{
    record Serie(string Clave, string Nombre);
    record Molde(string Tipo, string Unidad, string TituloY, Serie[] Series);

    static readonly Dictionary<string, Molde> Moldes = new()
    {
        ["tendencia_conexiones"] = new("lineas", "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["conexiones_acumuladas"] = new("lineas", "conexiones", "Acumulado", [new("conexiones_acumuladas", "Conexiones acumuladas"), new("usuarios_acumulados", "Usuarios acumulados")]),
        ["patrones_conexion"] = new("barras", "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["conexiones_por_escuela"] = new("barras_horizontales", "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["conexiones_por_maestro"] = new("barras_horizontales", "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["participacion_escuelas"] = new("barras_horizontales", "%", "% de participación", [new("porcentaje_participacion", "% de participación")]),
        ["uso_por_area"] = new("barras_agrupadas", "pruebas", "Pruebas", [new("pruebas_asignadas", "Pruebas asignadas"), new("pruebas_completadas", "Pruebas completadas")]),
        ["comparar_areas"] = new("barras_agrupadas", "pruebas", "Pruebas completadas", [new("pruebas_completadas_a", "Período A"), new("pruebas_completadas_b", "Período B")]),
        ["comparar_escuelas"] = new("barras_agrupadas", "conexiones", "Conexiones", [new("conexiones_a", "Período A"), new("conexiones_b", "Período B")]),
        ["promedio_full_quiz"] = new("barras_horizontales", "%", "Promedio % Full Quiz", [new("promedio_solo_realizadas", "Promedio (solo realizadas)")]),
        ["uso_docentes"] = new("barras_agrupadas", "maestros", "Maestros", [new("maestros", "Maestros"), new("con_conexiones", "Con conexiones"), new("con_asignaciones", "Con asignaciones")]),
    };

    /// <summary>Especificación de la gráfica, o null si la consulta no es graficable (buscar_escuela) o tiene menos de 2 filas.</summary>
    public static JsonObject? Desde(JsonNode respuesta)
    {
        var consulta = respuesta["consulta"]?.GetValue<string>() ?? "";
        if (!Moldes.TryGetValue(consulta, out var molde)) return null;
        var filas = respuesta["resultados"] as JsonArray;
        if (filas is null || filas.Count < 2) return null;
        var filtros = respuesta["filtros"];

        var categorias = new JsonArray();
        var etiquetas = filas.Select(f => Categoria(consulta, f!)).ToList();
        // uso_docentes puede repetir una escuela en varios períodos académicos: se distingue con el período.
        if (consulta == "uso_docentes" && etiquetas.Distinct().Count() != etiquetas.Count)
            etiquetas = filas.Select(f => $"{Columnas.Texto(f!["escuela"])} ({Columnas.Texto(f["periodo_desde"])})").ToList();
        foreach (var e in etiquetas) categorias.Add(e);

        var series = new JsonArray();
        foreach (var s in molde.Series)
        {
            var valores = new JsonArray();
            foreach (var f in filas) valores.Add(f![s.Clave]?.DeepClone());          // null se conserva como null
            series.Add(new JsonObject { ["clave"] = s.Clave, ["nombre"] = NombreSerie(consulta, s, respuesta), ["valores"] = valores });
        }

        var spec = new JsonObject
        {
            ["consulta"] = consulta,
            ["tipo"] = molde.Tipo,
            ["titulo"] = Titulo(consulta, filtros),
            ["eje_x"] = new JsonObject { ["titulo"] = TituloX(consulta, filtros), ["categorias"] = categorias },
            ["eje_y"] = new JsonObject { ["titulo"] = molde.TituloY, ["unidad"] = molde.Unidad },
            ["series"] = series,
            ["periodo"] = respuesta["periodo"]?.DeepClone(),
            ["filtros"] = filtros?.DeepClone(),
            ["datos_hasta"] = respuesta["datos_hasta"]?.DeepClone(),
            ["nota"] = respuesta["nota"]?.DeepClone(),
            ["filas"] = filas.Count,
            ["hay_mas_filas"] = Columnas.HayMasFilas(respuesta),
        };
        return spec;
    }

    static string Categoria(string consulta, JsonNode fila) => consulta switch
    {
        "tendencia_conexiones" or "conexiones_acumuladas" => Columnas.Texto(fila["periodo"]),
        "uso_docentes" => Columnas.Texto(fila["escuela"]),
        "promedio_full_quiz" => string.Join(" · ", new[] { fila["escuela"], fila["materia"] }.Select(Columnas.Texto).Where(t => t.Length > 0)),
        _ => Columnas.Texto(fila["nombre"]),
    };

    static string NombreSerie(string consulta, Serie s, JsonNode respuesta)
    {
        if (consulta is "comparar_areas" or "comparar_escuelas")
        {
            var lado = s.Clave.EndsWith("_a", StringComparison.Ordinal) ? "a" : "b";
            var p = respuesta["periodo"]?[lado];
            return $"Período {lado.ToUpperInvariant()} ({Columnas.Texto(p?["desde"])} a {Columnas.Texto(p?["hasta"])})";
        }
        return s.Nombre;
    }

    static string Titulo(string consulta, JsonNode? filtros) => consulta switch
    {
        "tendencia_conexiones" => "Conexiones por " + Texto(filtros, "granularidad", "mes") switch { "anio" => "año", var g => g },
        "conexiones_acumuladas" => "Conexiones y usuarios acumulados",
        "patrones_conexion" => Texto(filtros, "agrupar", "dia_semana") == "rol" ? "Conexiones por rol" : "Conexiones por día de la semana",
        "conexiones_por_escuela" => "Conexiones por escuela",
        "conexiones_por_maestro" => "Conexiones por maestro",
        "participacion_escuelas" => "Participación por escuela (% de usuarios conectados)",
        "uso_por_area" => "Pruebas asignadas y completadas por área",
        "comparar_areas" => "Pruebas completadas por área: período A vs B",
        "comparar_escuelas" => "Conexiones por escuela: período A vs B",
        "promedio_full_quiz" => "Promedio % Full Quiz",
        "uso_docentes" => "Uso de la plataforma por maestros",
        _ => consulta,
    };

    static string TituloX(string consulta, JsonNode? filtros) => consulta switch
    {
        "tendencia_conexiones" or "conexiones_acumuladas" => "Período",
        "patrones_conexion" => Texto(filtros, "agrupar", "dia_semana") == "rol" ? "Rol" : "Día de la semana",
        "conexiones_por_maestro" => "Maestro",
        "uso_por_area" or "comparar_areas" => "Área",
        "promedio_full_quiz" => Texto(filtros, "agrupar", "escuela_materia") switch { "escuela" => "Escuela", "materia" => "Materia", _ => "Escuela · materia" },
        _ => "Escuela",
    };

    static string Texto(JsonNode? filtros, string clave, string defecto) => filtros?[clave] is { } v ? Columnas.Texto(v) : defecto;
}
