using EyaInsightsApi;
using MySqlConnector;

/// <summary>
/// Las 12 rutas de consulta como funciones con nombre. Las usan tanto las rutas JSON (GET /{ruta}) como la exportación
/// (GET /exportar/{ruta}), así comparten validación y cálculo. La lógica de cada consulta no cambia respecto a la Fase 1.
/// </summary>
static class Rutas
{
    /// <summary>Ruta (sin «/») → consulta. «buscar-escuela» está aquí pero no es exportable (ver /exportar en Program.cs).</summary>
    public static readonly Dictionary<string, Func<HttpRequest, IConsultasUso, Respuesta>> Consultas = new()
    {
        ["uso-por-area"] = UsoPorArea,
        ["buscar-escuela"] = BuscarEscuela,
        ["conexiones-por-escuela"] = ConexionesPorEscuela,
        ["conexiones-por-maestro"] = ConexionesPorMaestro,
        ["comparar-areas"] = CompararAreas,
        ["comparar-escuelas"] = CompararEscuelas,
        ["promedio-full-quiz"] = PromedioFullQuiz,
        ["tendencia-conexiones"] = TendenciaConexiones,
        ["conexiones-acumuladas"] = ConexionesAcumuladas,
        ["uso-docentes"] = UsoDocentes,
        ["patrones-conexion"] = PatronesConexion,
        ["participacion-escuelas"] = ParticipacionEscuelas,
    };

    /// <summary>Ejecuta una acción y traduce los errores conocidos a respuestas HTTP (400 parámetros, 503 base).</summary>
    public static IResult Intentar(Func<IResult> accion)
    {
        try { return accion(); }
        catch (ErrorConsulta e) { return Results.Json(new { error = "parametros_invalidos", mensaje = e.Message }, statusCode: 400); }
        catch (MySqlException) { return Results.Json(new { error = "base_no_disponible", mensaje = "No se pudo consultar la base de datos; intenta de nuevo más tarde." }, statusCode: 503); }
    }

    public static IResult Ejecutar(Func<Respuesta> consulta) => Intentar(() => Results.Ok(consulta()));

    static Respuesta UsoPorArea(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r); var (desc, limite) = P.OrdenLimite(r);
        var todas = c.UsoPorArea(d, h, desc, 1000);          // todas las áreas, para poder dar totales y participación
        int asig = todas.Sum(f => f.PruebasAsignadas), comp = todas.Sum(f => f.PruebasCompletadas);
        var filas = todas.Take(limite).Select(f => f with { PorcentajeDelTotalCompletadas = P.Pct(f.PruebasCompletadas, comp) }).ToList();
        return new Respuesta("uso_por_area", c.Fuente, c.ColumnaFechaPruebas, P.Periodo(d, h),
            new { orden = P.Texto(desc), limite, escuela = "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaPruebas, filas,
            P.Nota(c.NotaArea, h, c.DatosHastaPruebas),
            new { total_pruebas_asignadas = asig, total_pruebas_completadas = comp, porcentaje_completado_global = P.Pct(comp, asig),
                  areas_consideradas = todas.Count, areas_con_pruebas_asignadas = todas.Count(f => f.PruebasAsignadas > 0),
                  areas_sin_pruebas_asignadas = todas.Count(f => f.PruebasAsignadas == 0) });
    }

    static Respuesta BuscarEscuela(HttpRequest r, IConsultasUso c)
    {
        var texto = r.Query["texto"].FirstOrDefault()?.Trim();
        if (string.IsNullOrEmpty(texto) || texto.Length < 2) throw new ErrorConsulta("Falta el parámetro 'texto' (mínimo 2 caracteres).");
        var (_, limite) = P.OrdenLimite(r);
        return new Respuesta("buscar_escuela", c.Fuente, "no aplica", "no aplica", new { texto, cuentas_de_prueba = P.Cp(c) }, null,
            c.BuscarEscuelas(texto, limite),
            P.Unir("Usa el nombre EXACTO devuelto como filtro 'escuela' en las demás consultas.", c.NotaConexiones));
    }

    static Respuesta ConexionesPorEscuela(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r); var (desc, limite) = P.OrdenLimite(r);
        var escuela = P.Escuela(r, c); var (minC, maxC) = P.RangoConexiones(r);
        var todasLas = c.ConexionesPorEscuela(d, h, desc, 1000);     // todas las escuelas: permite totales, participación y empates exactos
        int total = todasLas.Sum(f => f.Conexiones);
        var todas = escuela is null ? todasLas : todasLas.Where(f => f.Nombre == escuela).ToList();   // con filtro, solo esa escuela (el % sigue siendo sobre el total)
        var conFiltroN = minC > 0 || maxC < int.MaxValue;
        if (conFiltroN) todas = todas.Where(f => f.Conexiones >= minC && f.Conexiones <= maxC).ToList();
        var filas = todas.Take(limite).Select(f => f with
        {
            PorcentajeDelTotal = P.Pct(f.Conexiones, total),
            ConexionesPorUsuario = f.UsuariosConectados == 0 ? null : Math.Round((double)f.Conexiones / f.UsuariosConectados, 1)
        }).ToList();
        return new Respuesta("conexiones_por_escuela", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
            new { orden = P.Texto(desc), limite, escuela = escuela ?? "todas", min_conexiones = conFiltroN ? minC : (int?)null, max_conexiones = conFiltroN && maxC < int.MaxValue ? maxC : (int?)null, cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, filas,
            P.Unir(P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones), conFiltroN ? "Filtro por número de conexiones: una conexión = un día con ingreso (no ingresos dentro del mismo día); «una sola vez» = min_conexiones 1 y max_conexiones 1 dentro del período pedido." : null),
                P.Empates(todas, limite, f => f.Conexiones, completa: true)),
            new { total_conexiones = total, escuelas_consideradas = todasLas.Count, escuelas_con_conexiones = todasLas.Count(f => f.Conexiones > 0),
                  escuelas_sin_conexiones = todasLas.Count(f => f.Conexiones == 0), escuelas_que_cumplen_el_filtro = conFiltroN ? todas.Count : (int?)null });
    }

    static Respuesta ConexionesPorMaestro(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r); var (desc, limite) = P.OrdenLimite(r);
        var escuela = P.Escuela(r, c); var (minC, maxC) = P.RangoConexiones(r);
        var todosLos = c.ConexionesPorMaestro(d, h, desc, 5000, escuela);   // todos los maestros: totales, participación y empates exactos
        int total = todosLos.Sum(f => f.Conexiones);
        var conFiltroN = minC > 0 || maxC < int.MaxValue;
        var todos = conFiltroN ? todosLos.Where(f => f.Conexiones >= minC && f.Conexiones <= maxC).ToList() : todosLos;
        var filas = todos.Take(limite).Select(f => f with { PorcentajeDelTotal = P.Pct(f.Conexiones, total) }).ToList();
        return new Respuesta("conexiones_por_maestro", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
            new { orden = P.Texto(desc), limite, escuela = escuela ?? "todas", min_conexiones = conFiltroN ? minC : (int?)null, max_conexiones = conFiltroN && maxC < int.MaxValue ? maxC : (int?)null, cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, filas,
            P.Unir(P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones), conFiltroN ? "Filtro por número de conexiones: una conexión = un día con ingreso; «una sola vez» = min_conexiones 1 y max_conexiones 1 dentro del período pedido." : null),
                P.Empates(todos, limite, f => f.Conexiones, completa: true)),
            new { total_conexiones_maestros = total, maestros_considerados = todosLos.Count, maestros_con_conexiones = todosLos.Count(f => f.Conexiones > 0),
                  maestros_sin_conexiones = todosLos.Count(f => f.Conexiones == 0), maestros_que_cumplen_el_filtro = conFiltroN ? todos.Count : (int?)null });
    }

    static Respuesta CompararAreas(HttpRequest r, IConsultasUso c)
    {
        var (da, ha) = P.Rango(r, "desde_a", "hasta_a"); var (db, hb) = P.Rango(r, "desde_b", "hasta_b");
        return new Respuesta("comparar_areas", c.Fuente, c.ColumnaFechaPruebas, new { a = P.Periodo(da, ha), b = P.Periodo(db, hb) },
            new { escuela = "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaPruebas, c.CompararAreas(da, ha, db, hb),
            P.Nota(c.NotaArea is null ? "Si los dos periodos tienen distinta duración, la comparación no está normalizada." : c.NotaArea + " Si los dos periodos tienen distinta duración, la comparación no está normalizada.", P.Max(ha, hb), c.DatosHastaPruebas));
    }

    static Respuesta CompararEscuelas(HttpRequest r, IConsultasUso c)
    {
        var (da, ha) = P.Rango(r, "desde_a", "hasta_a"); var (db, hb) = P.Rango(r, "desde_b", "hasta_b");
        var (desc, limite) = P.OrdenLimite(r);
        var a = c.ConexionesPorEscuela(da, ha, true, 1000).ToDictionary(f => f.Nombre, f => f.Conexiones);
        var b = c.ConexionesPorEscuela(db, hb, true, 1000).ToDictionary(f => f.Nombre, f => f.Conexiones);
        var todas = a.Keys.Union(b.Keys).Select(n =>
        {
            int va = a.GetValueOrDefault(n), vb = b.GetValueOrDefault(n), dif = vb - va;
            return new FilaComparacionEscuela(n, va, vb, dif, va == 0 ? null : Math.Round(100.0 * dif / va, 1));
        }).Where(f => f.ConexionesA > 0 || f.ConexionesB > 0).ToList();
        var ord = (desc ? todas.OrderByDescending(f => f.Diferencia).ThenByDescending(f => f.Nombre, StringComparer.Ordinal)
                        : todas.OrderBy(f => f.Diferencia).ThenBy(f => f.Nombre, StringComparer.Ordinal)).ToList();
        int ta = a.Values.Sum(), tb = b.Values.Sum();
        return new Respuesta("comparar_escuelas", c.Fuente, c.ColumnaFechaConexiones, new { a = P.Periodo(da, ha), b = P.Periodo(db, hb) },
            new { orden = P.Texto(desc), limite, escuela = "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, ord.Take(limite).ToList(),
            P.Unir(P.Nota(c.NotaConexiones, P.Max(ha, hb), c.DatosHastaConexiones),
                "Diferencia = conexiones del período B menos las del A; con orden desc primero las escuelas que más crecieron, con asc las que más cayeron. Solo escuelas con conexiones en A o B. Si los períodos tienen distinta duración, la comparación no está normalizada."),
            new { total_conexiones_a = ta, total_conexiones_b = tb, diferencia_total = tb - ta, variacion_total_pct = ta == 0 ? (double?)null : Math.Round(100.0 * (tb - ta) / ta, 1),
                  escuelas_comparadas = ord.Count });
    }

    static Respuesta PromedioFullQuiz(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r); var (desc, limite) = P.OrdenLimite(r);
        var agrupar = r.Query["agrupar"].FirstOrDefault() ?? "escuela_materia";
        if (agrupar is not ("escuela" or "materia" or "escuela_materia")) throw new ErrorConsulta("'agrupar' debe ser 'escuela', 'materia' o 'escuela_materia'.");
        var escuela = P.Escuela(r, c);
        var todas = c.PromedioFullQuiz(d, h, agrupar, desc, limite + 1, escuela);   // una fila de más para saber si se cortó la lista
        var hayMas = todas.Count > limite; var filas = todas.Take(limite).ToList();
        return new Respuesta("promedio_full_quiz", c.Fuente, c.ColumnaFechaPruebas, P.Periodo(d, h),
            new { agrupar, orden = P.Texto(desc), limite, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaPruebas,
            filas, P.Unir(P.Nota(c.NotaPromedioQuiz, h, c.DatosHastaPruebas),
                hayMas ? $"HAY MÁS FILAS: se muestran solo las primeras {limite} (límite máximo 50); existen más combinaciones con el mismo filtro. No presentes la lista como completa." : null));
    }

    static Respuesta TendenciaConexiones(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r);
        var gran = r.Query["granularidad"].FirstOrDefault() ?? "mes";
        if (gran is not ("mes" or "trimestre" or "anio")) throw new ErrorConsulta("'granularidad' debe ser 'mes', 'trimestre' o 'anio'.");
        var escuela = P.Escuela(r, c);
        var serie = c.Tendencia(d, h, gran, escuela);
        var conVar = serie.Select((f, i) => i == 0 ? f : f with
        {
            DiferenciaConPeriodoAnterior = f.Conexiones - serie[i - 1].Conexiones,
            VariacionPct = serie[i - 1].Conexiones == 0 ? null : Math.Round(100.0 * (f.Conexiones - serie[i - 1].Conexiones) / serie[i - 1].Conexiones, 1)
        }).ToList();
        var mayor = serie.OrderByDescending(f => f.Conexiones).ThenBy(f => f.Periodo, StringComparer.Ordinal).FirstOrDefault();
        var menor = serie.OrderBy(f => f.Conexiones).ThenBy(f => f.Periodo, StringComparer.Ordinal).FirstOrDefault();
        return new Respuesta("tendencia_conexiones", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
            new { granularidad = gran, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, conVar,
            P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones), "El primer período no trae variación (no hay anterior); el último puede estar incompleto si el período pedido no lo cubre entero."),
            new { total_conexiones = serie.Sum(f => f.Conexiones), periodo_con_mas_conexiones = mayor?.Periodo, conexiones_del_mayor = mayor?.Conexiones,
                  periodo_con_menos_conexiones = menor?.Periodo, conexiones_del_menor = menor?.Conexiones });
    }

    static Respuesta ConexionesAcumuladas(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r);
        var gran = r.Query["granularidad"].FirstOrDefault() ?? "mes";
        if (gran is not ("dia" or "mes" or "trimestre" or "anio")) throw new ErrorConsulta("'granularidad' debe ser 'dia', 'mes', 'trimestre' o 'anio'.");
        if (gran == "dia" && h.DayNumber - d.DayNumber > 92) throw new ErrorConsulta("Con granularidad 'dia' el período puede tener como máximo 93 días; usa 'mes' para períodos más largos.");
        var escuela = P.Escuela(r, c);
        string Clave(DateOnly x) => gran == "dia" ? x.ToString("yyyy-MM-dd") : ConsultasEjemplo.Clave(x, gran);
        var acc = new SortedDictionary<string, (int Con, int Nue)>(StringComparer.Ordinal);
        for (var x = d; x <= h; x = x.AddDays(1)) acc.TryAdd(Clave(x), (0, 0));            // incluye períodos en cero
        foreach (var (dia, con, nue) in c.ConexionesYNuevosPorDia(d, h, escuela)) { var k = Clave(dia); acc[k] = (acc[k].Con + con, acc[k].Nue + nue); }
        int sc = 0, su = 0;
        var filas = acc.Select(kv => { sc += kv.Value.Con; su += kv.Value.Nue; return new FilaAcumulada(kv.Key, kv.Value.Con, sc, kv.Value.Nue, su); }).ToList();
        var act = escuela is null ? c.UsuariosActivos() : null;
        static double? Pct2(int parte, int total) => total == 0 ? null : Math.Round(100.0 * parte / total, 2);
        return new Respuesta("conexiones_acumuladas", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
            new { granularidad = gran, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, filas,
            P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones),
                P.Unir("conexiones_acumuladas y usuarios_acumulados son la suma corrida desde el inicio del período pedido; usuarios_nuevos = usuarios cuya primera conexión del período cae en esa fila.",
                    escuela is null ? c.NotaActivos : "Con filtro de escuela no se informan usuarios_activos ni porcentajes (la tarjeta de Power BI es de toda la plataforma).")),
            new { total_conexiones = sc, usuarios_conectados = su,
                  usuarios_activos = act?.UsuariosActivos, porcentaje_conexion_usuarios = act is null ? null : Pct2(su, act.UsuariosActivos),
                  usuarios_conectados_alguna_vez = act?.ConectadosAlgunaVez, porcentaje_conectados_alguna_vez = act is null ? null : Pct2(act.ConectadosAlgunaVez, act.UsuariosActivos) });
    }

    static Respuesta UsoDocentes(HttpRequest r, IConsultasUso c)
    {
        var pd = P.FechaOpcional(r, "periodo_desde");
        var fecha = pd is null ? P.FechaOpcional(r, "fecha") ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMinutes(-240)) : (DateOnly?)null;   // hoy en Puerto Rico
        var (_, limite) = P.OrdenLimite(r);
        var escuela = P.Escuela(r, c);
        var (todas, total) = c.UsoDocentes(fecha, pd, escuela);
        var filas = todas.Take(limite).ToList();
        var seleccion = pd is { } p0 ? (object)new { periodo_desde = p0.ToString("yyyy-MM-dd") } : new { fecha = fecha!.Value.ToString("yyyy-MM-dd") };
        return new Respuesta("uso_docentes", c.Fuente, "vw_teacher_activities.from_period / to_period (período académico de cada escuela)", seleccion,
            new { limite, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, null, filas,
            P.Unir(c.NotaDocentes, todas.Count == 0
                ? "NO HAY DATOS: ninguna escuela tiene un período académico que cumpla esa condición."
                : todas.Count > limite ? $"HAY MÁS FILAS: se muestran {limite} de {todas.Count} combinaciones escuela-período; el 'resumen' trae el total de todas." : null),
            new { escuelas = todas.Select(f => f.Escuela).Distinct().Count(), combinaciones_escuela_periodo = todas.Count,
                  maestros = total.Maestros, con_conexiones = total.ConConexiones, con_planes = total.ConPlanes, planes_con_datos = total.PlanesConDatos,
                  con_grupos = total.ConGrupos, con_asignaciones = total.ConAsignaciones,
                  maestros_sin_conexiones = total.Maestros - total.ConConexiones });
    }

    static Respuesta PatronesConexion(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r);
        var agrupar = r.Query["agrupar"].FirstOrDefault() ?? "dia_semana";
        if (agrupar is not ("dia_semana" or "rol")) throw new ErrorConsulta("'agrupar' debe ser 'dia_semana' o 'rol'.");
        var escuela = P.Escuela(r, c);
        var todas = c.PatronConexiones(d, h, agrupar, escuela);
        int total = todas.Sum(f => f.Conexiones);
        var filas = todas.Select(f => f with { PorcentajeDelTotal = P.Pct(f.Conexiones, total) }).ToList();
        var mayor = todas.OrderByDescending(f => f.Conexiones).FirstOrDefault(); var menor = todas.OrderBy(f => f.Conexiones).FirstOrDefault();
        var empateMayor = mayor is not null && todas.Count(f => f.Conexiones == mayor.Conexiones) > 1;
        return new Respuesta("patrones_conexion", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
            new { agrupar, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, filas,
            P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones),
                agrupar == "dia_semana" ? "Siempre salen los 7 días (lunes a domingo, en ese orden); el día se lee en la hora local indicada. El total por día depende de cuántos lunes, martes… tenga el período pedido: con períodos cortos la comparación no es justa."
                                        : "Rol de la cuenta de cada usuario (sin el rol client); ordenado de más a menos conexiones."),
            new { total_conexiones = total, mas_conexiones = mayor?.Nombre, conexiones_del_mayor = mayor?.Conexiones, hay_empate_en_el_mayor = empateMayor,
                  menos_conexiones = menor?.Nombre, conexiones_del_menor = menor?.Conexiones });
    }

    static Respuesta ParticipacionEscuelas(HttpRequest r, IConsultasUso c)
    {
        var (d, h) = P.Rango(r); var (desc, limite) = P.OrdenLimite(r);
        var escuela = P.Escuela(r, c);
        var todasLas = c.ParticipacionEscuelas(d, h);
        var todas = (desc ? todasLas.OrderBy(f => f.PorcentajeParticipacion is null).ThenByDescending(f => f.PorcentajeParticipacion)
                          : todasLas.OrderBy(f => f.PorcentajeParticipacion is null).ThenBy(f => f.PorcentajeParticipacion)).ToList();   // estable: empates por nombre; sin activos al final
        if (escuela is not null) todas = todas.Where(f => f.Nombre == escuela).ToList();
        int act = todasLas.Sum(f => f.UsuariosActivos), con = todasLas.Sum(f => f.UsuariosConectados);
        return new Respuesta("participacion_escuelas", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
            new { orden = P.Texto(desc), limite, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, todas.Take(limite).ToList(),
            P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones), P.Unir(c.NotaParticipacion,
                todas.Count > limite ? $"HAY MÁS FILAS: se muestran {limite} de {todas.Count} escuelas; el 'resumen' trae el total de todas. Los empates se ordenan alfabéticamente, así que la lista no es exhaustiva si el último valor se repite." : null)),
            new { escuelas_consideradas = todasLas.Count, total_usuarios_activos = act, total_usuarios_conectados = con, porcentaje_participacion_global = P.Pct(con, act),
                  escuelas_sin_conectados = todasLas.Count(f => f.UsuariosConectados == 0), escuelas_con_100_o_mas = todasLas.Count(f => f.PorcentajeParticipacion >= 100) });
    }
}
