using System.Text.Json.Nodes;

/// <summary>
/// Especificación de gráfica (JSON) derivada de forma determinista del resultado de una consulta (tabla D9 del plan de la Fase 4).
/// No calcula nada nuevo: <c>data</c> copia tal cual los números de <c>resultados</c> (los null quedan null). Dibuja el cliente.
/// Formato para Recharts: <c>data</c> = arreglo de objetos planos <c>{categoria, serie1, serie2…}</c>; <c>eje_x.clave</c> y cada <c>series[].clave</c>
/// son las claves de esos objetos; <c>tipo</c> = "line" | "bar" (el front admite también "pie", esta API no lo genera); <c>layout</c> (solo bar) =
/// "horizontal" (columnas) | "vertical" (barras horizontales), igual que el prop <c>layout</c> de BarChart.
/// </summary>
/// <param name="Todas">false = vista previa (los <paramref name="Limite"/> primeros); true = todas las categorías.</param>
/// <param name="Orden">"desc" (mayor a menor, por defecto) o "asc"; solo en barras (las líneas siguen el orden del tiempo).</param>
/// <param name="IncluirCeros">false (por defecto) = sin las categorías cuyos valores son todos 0 o null.</param>
public record OpcionesGrafica(bool Todas = false, string Orden = "desc", bool IncluirCeros = false, int Limite = OpcionesGrafica.LimiteVistaPrevia)
{
    public const int LimiteVistaPrevia = 10;
}

static class Graficas
{
    record Serie(string Clave, string Nombre);
    record Molde(string Tipo, string? Layout, string Unidad, string TituloY, Serie[] Series, string? ClaveOrden = null);

    /// <summary>Clave del eje X dentro de cada objeto de <c>data</c>.</summary>
    const string ClaveX = "categoria";

    static readonly Dictionary<string, Molde> Moldes = new()
    {
        ["tendencia_conexiones"] = new("line", null, "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["conexiones_acumuladas"] = new("line", null, "conexiones", "Acumulado", [new("conexiones_acumuladas", "Conexiones acumuladas"), new("usuarios_acumulados", "Usuarios acumulados")]),
        ["patrones_conexion"] = new("bar", "horizontal", "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["conexiones_por_escuela"] = new("bar", "vertical", "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["conexiones_por_maestro"] = new("bar", "vertical", "conexiones", "Conexiones", [new("conexiones", "Conexiones")]),
        ["participacion_escuelas"] = new("bar", "vertical", "%", "% de participación", [new("porcentaje_participacion", "% de participación")]),
        ["uso_por_area"] = new("bar", "horizontal", "pruebas", "Pruebas", [new("pruebas_asignadas", "Pruebas asignadas"), new("pruebas_completadas", "Pruebas completadas")], "pruebas_completadas"),
        ["comparar_areas"] = new("bar", "horizontal", "pruebas", "Pruebas completadas", [new("pruebas_completadas_a", "Período A"), new("pruebas_completadas_b", "Período B")]),
        ["comparar_escuelas"] = new("bar", "horizontal", "conexiones", "Conexiones", [new("conexiones_a", "Período A"), new("conexiones_b", "Período B")]),
        ["promedio_full_quiz"] = new("bar", "vertical", "%", "Promedio % Full Quiz", [new("promedio_solo_realizadas", "Promedio (solo realizadas)")]),
        ["uso_docentes"] = new("bar", "horizontal", "maestros", "Maestros", [new("maestros", "Maestros"), new("con_conexiones", "Con conexiones"), new("con_asignaciones", "Con asignaciones")]),
    };

    public static bool Graficable(string consulta) => Moldes.ContainsKey(consulta.Replace('-', '_'));

    /// <summary>
    /// Especificación de la gráfica, o null si la consulta no es graficable (buscar_escuela) o quedan menos de 2 categorías.
    /// BARRAS: ordenadas por valor (mayor a menor por defecto), sin las categorías en 0 (por defecto) y limitadas a 10 en la vista previa;
    /// <c>total_categorias</c> = las que hay con esas opciones, <c>categorias_mostradas</c> = las que trae <c>data</c>, <c>categorias_en_cero</c> = las de valor 0.
    /// LÍNEAS y «día de la semana»: orden natural, todas las categorías.
    /// </summary>
    public static JsonObject? Desde(JsonNode respuesta, OpcionesGrafica? opciones = null)
    {
        var consulta = respuesta["consulta"]?.GetValue<string>() ?? "";
        if (!Moldes.TryGetValue(consulta, out var molde)) return null;
        if (respuesta["resultados"] is not JsonArray crudas || crudas.Count < 2) return null;
        var o = opciones ?? new OpcionesGrafica();
        var filtros = respuesta["filtros"];
        List<JsonNode> filas = crudas.Select(f => f!).ToList();

        var ordena = molde.Tipo == "bar" && !(consulta == "patrones_conexion" && Texto(filtros, "agrupar", "dia_semana") == "dia_semana");
        var enCero = 0;
        if (ordena)
        {
            var clave = molde.ClaveOrden ?? molde.Series[0].Clave;
            double Valor(JsonNode f) => Columnas.Numero(f[clave]) ?? 0;
            bool Cero(JsonNode f) => molde.Series.All(s => (Columnas.Numero(f[s.Clave]) ?? 0) == 0);
            enCero = filas.Count(Cero);
            if (!o.IncluirCeros) filas = filas.Where(f => !Cero(f)).ToList();
            filas = (o.Orden == "asc" ? filas.OrderBy(Valor) : filas.OrderByDescending(Valor)).ThenBy(f => Categoria(consulta, f), StringComparer.Ordinal).ToList();
        }
        var total = filas.Count;
        if (ordena && !o.Todas) filas = filas.Take(o.Limite).ToList();
        if (filas.Count < 2) return null;

        var etiquetas = filas.Select(f => Categoria(consulta, f!)).ToList();
        // Etiquetas repetidas (dos maestros con el mismo nombre; una escuela en varios períodos académicos) se distinguen para que Recharts no las confunda.
        if (etiquetas.Distinct().Count() != etiquetas.Count)
        {
            if (consulta == "uso_docentes") etiquetas = filas.Select(f => $"{Columnas.Texto(f!["escuela"])} ({Columnas.Texto(f["periodo_desde"])})").ToList();
            else if (consulta == "conexiones_por_maestro") etiquetas = filas.Select(f => $"{Columnas.Texto(f!["nombre"])} ({Columnas.Texto(f["escuela"])})").ToList();
        }
        if (etiquetas.Distinct().Count() != etiquetas.Count)       // siguen repetidas (mismo nombre y misma escuela): se numeran
        {
            var vistas = new Dictionary<string, int>();
            etiquetas = etiquetas.Select(e => vistas[e] = vistas.GetValueOrDefault(e) + 1).Zip(etiquetas, (n, e) => n == 1 ? e : $"{e} #{n}").ToList();
        }

        var data = new JsonArray();                                   // objetos planos para Recharts; null se conserva como null
        for (var i = 0; i < filas.Count; i++)
        {
            var punto = new JsonObject { [ClaveX] = etiquetas[i] };
            foreach (var s in molde.Series) punto[s.Clave] = filas[i]![s.Clave]?.DeepClone();
            data.Add(punto);
        }
        var series = new JsonArray();
        foreach (var s in molde.Series)
            series.Add(new JsonObject { ["clave"] = s.Clave, ["nombre"] = NombreSerie(consulta, s, respuesta) });

        var spec = new JsonObject
        {
            ["consulta"] = consulta,
            ["tipo"] = molde.Tipo,
            ["titulo"] = Titulo(consulta, filtros),
            ["eje_x"] = new JsonObject { ["clave"] = ClaveX, ["titulo"] = TituloX(consulta, filtros) },
            ["eje_y"] = new JsonObject { ["titulo"] = molde.TituloY, ["unidad"] = molde.Unidad },
            ["series"] = series,
            ["data"] = data,
            ["periodo"] = respuesta["periodo"]?.DeepClone(),
            ["filtros"] = filtros?.DeepClone(),
            ["datos_hasta"] = respuesta["datos_hasta"]?.DeepClone(),
            ["nota"] = respuesta["nota"]?.DeepClone(),
            ["filas"] = filas.Count,
            ["total_categorias"] = total,
            ["categorias_mostradas"] = filas.Count,
            ["hay_mas_filas"] = Columnas.HayMasFilas(respuesta),
        };
        if (ordena)
        {
            spec["categorias_en_cero"] = enCero;
            spec["orden"] = o.Orden;
            spec["incluir_ceros"] = o.IncluirCeros;
            spec["todas"] = o.Todas;
        }
        if (molde.Layout is not null) spec["layout"] = molde.Layout;         // solo en bar (en line se omite)
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
