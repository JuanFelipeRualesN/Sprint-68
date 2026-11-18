using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using EyaInsightsApi;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    o.SerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;   // deja las tildes legibles
    o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Origen de datos: "ejemplo" (por defecto, datos inventados) o "mysql" (gs_data_analysis, SOLO LECTURA).
// La cadena de conexión NUNCA va en archivos: variable de entorno EYA_MYSQL o `dotnet user-secrets`.
if ((builder.Configuration["Datos:Origen"] ?? "ejemplo") == "mysql")
{
    // Conexión: EYA_MYSQL (cadena completa) o por partes EYA_MYSQL_HOST / _PORT / _USER / _PASSWORD / _SSL
    // (por partes no hay que escapar símbolos de la clave). Nunca en archivos ni en el repositorio.
    var cadena = builder.Configuration["EYA_MYSQL"];
    if (string.IsNullOrWhiteSpace(cadena) && !string.IsNullOrWhiteSpace(builder.Configuration["EYA_MYSQL_HOST"]))
    {
        var ssl = Enum.Parse<MySqlSslMode>(builder.Configuration["EYA_MYSQL_SSL"] ?? "Required", ignoreCase: true);
        cadena = new MySqlConnectionStringBuilder
        {
            Server = builder.Configuration["EYA_MYSQL_HOST"],
            Port = uint.Parse(builder.Configuration["EYA_MYSQL_PORT"] ?? "3306", CultureInfo.InvariantCulture),
            UserID = builder.Configuration["EYA_MYSQL_USER"],
            Password = builder.Configuration["EYA_MYSQL_PASSWORD"],
            Database = "gs_data_analysis",
            SslMode = ssl,
            AllowPublicKeyRetrieval = ssl == MySqlSslMode.None,   // solo para MySQL local de pruebas sin TLS
        }.ConnectionString;
    }
    if (string.IsNullOrWhiteSpace(cadena)) throw new InvalidOperationException("Datos:Origen=mysql requiere EYA_MYSQL o EYA_MYSQL_HOST/_USER/_PASSWORD.");
    // Día en hora de Puerto Rico por defecto (-240 min = UTC-4, sin horario de verano); Datos__OffsetUtcMinutos=0 lo cambia a UTC.
    var offsetMin = int.Parse(builder.Configuration["Datos:OffsetUtcMinutos"] ?? "-240", CultureInfo.InvariantCulture);
    // Cuentas internas/de prueba: se excluyen por defecto (Datos__ExcluirCuentasDePrueba=false las incluye, p. ej. para comparar con Power BI).
    string[] Lista(string clave, string defecto) =>
        (builder.Configuration[clave] ?? defecto).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    var exclusion = new ExclusionCuentas(
        !string.Equals(builder.Configuration["Datos:ExcluirCuentasDePrueba"], "false", StringComparison.OrdinalIgnoreCase),
        // Por defecto = el criterio de Power BI: ORE que contiene DEMOSTRACION (vistas vw_*) y «Escuela Facilitadores MSJ» (filtro de su página
        // de conexiones). Los correos @requiretechnology.com NO se excluyen (Power BI los cuenta); se pueden excluir con
        // Datos__DominiosExcluidos=requiretechnology.com (y, si se quiere, Datos__EscuelasExcluidas="Escuela Demo T3 School,Escuela Facilitadores MSJ").
        Lista("Datos:OresExcluidos", "DEMOSTRACION"),
        Lista("Datos:DominiosExcluidos", ""),
        Lista("Datos:EscuelasExcluidas", "Escuela Facilitadores MSJ"));
    builder.Services.AddSingleton<IConsultasUso>(new ConsultasMySql(cadena, offsetMin, exclusion));
}
else builder.Services.AddSingleton<IConsultasUso, ConsultasEjemplo>();

// Asistente (panel junto a Power BI): DESACTIVADO por defecto. Asistente__Habilitado=true lo activa para desarrollo local.
// Desplegado, debe ir detrás del backend de la plataforma (sesión y rol del usuario); nunca expuesto directamente.
var asistenteOn = string.Equals(builder.Configuration["Asistente:Habilitado"], "true", StringComparison.OrdinalIgnoreCase);
if (asistenteOn)
{
    builder.Services.AddSingleton(new AsistenteFoundry(builder.Configuration));
    var origenes = (builder.Configuration["Asistente:Origenes"] ?? "http://localhost:3000")
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    builder.Services.AddCors(o => o.AddPolicy("panel", p => p.WithOrigins(origenes).WithHeaders("content-type").WithMethods("POST")));
}

var app = builder.Build();
if (asistenteOn) app.UseCors("panel");

if (app.Services.GetRequiredService<IConsultasUso>() is ConsultasMySql mysql)
{
    try { Console.WriteLine($"MySQL conectado en modo SOLO LECTURA como «{mysql.UsuarioConectado}». Conexiones hasta {mysql.DatosHastaConexiones}; pruebas hasta {mysql.DatosHastaPruebas}."); }
    catch (Exception e) { Console.WriteLine($"No se pudo conectar a MySQL ({e.GetType().Name}): revisa VPN, host, usuario y clave."); throw; }
}

// Clave de API: se exige el header x-api-key cuando existe EYA_API_KEY (menos en /salud).
var claveEsperada = app.Configuration["EYA_API_KEY"];
app.Use(async (ctx, next) =>
{
    if (!string.IsNullOrEmpty(claveEsperada) && ctx.Request.Path != "/salud"
        && ctx.Request.Headers["x-api-key"] != claveEsperada)
    {
        ctx.Response.StatusCode = 401;
        await ctx.Response.WriteAsJsonAsync(new { error = "no_autorizado", mensaje = "Falta o es incorrecto x-api-key." });
        return;
    }
    await next();
});

if (asistenteOn)
{
    var local = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
    Console.WriteLine($"Asistente HABILITADO (modelo {app.Services.GetRequiredService<AsistenteFoundry>().Modelo}, {app.Services.GetRequiredService<AsistenteFoundry>().Herramientas} consultas). Solo para desarrollo local.");
    app.MapPost("/asistente/preguntar", async (HttpContext ctx, PreguntaAsistente p, AsistenteFoundry asistente) =>
    {
        if (string.IsNullOrWhiteSpace(p.Pregunta) || p.Pregunta.Trim().Length < 2)
            return Results.Json(new { error = "pregunta_vacia", mensaje = "Escribe una pregunta." }, statusCode: 400);
        var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";
        try
        {
            var r = await asistente.PreguntarAsync(p, (op, a) => asistente.EjecutarAsync(local, baseUrl, claveEsperada, op, a, ctx.RequestAborted), ctx.RequestAborted);
            return Results.Json(r);
        }
        catch (ErrorFoundry e) when (e.FiltroContenido)    // p. ej. el filtro anti-jailbreak de Azure
        {
            return Results.Json(new RespuestaAsistente("No puedo responder a ese mensaje. Reformula tu pregunta sobre el uso de la plataforma.",
                p.ConversacionId ?? "", new(), new(), new(), new(), "contenido_bloqueado"));
        }
        catch (Exception e) when (e is Azure.Identity.AuthenticationFailedException or Azure.Identity.CredentialUnavailableException
                                  || (e is ErrorFoundry ef && ef.Estado is 401 or 403))
        {
            Console.WriteLine($"Asistente: sin acceso al modelo ({e.GetType().Name}).");
            return Results.Json(new { error = "sin_acceso_al_modelo", mensaje = "El servicio no pudo autenticarse con Azure AI Foundry (¿az login vigente?)." }, statusCode: 503);
        }
        catch (Exception e) when (e is ErrorFoundry or HttpRequestException or TaskCanceledException)
        {
            Console.WriteLine($"Asistente: modelo no disponible ({e.GetType().Name}: {e.Message}).");
            return Results.Json(new { error = "modelo_no_disponible", mensaje = "El asistente no está disponible en este momento. Intenta de nuevo." }, statusCode: 502);
        }
    });
}

app.MapGet("/salud", (IConsultasUso c) => new { estado = "ok", fuente = c.Fuente, datos_hasta_pruebas = c.DatosHastaPruebas, datos_hasta_conexiones = c.DatosHastaConexiones });

app.MapGet("/uso-por-area", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
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
}));

app.MapGet("/buscar-escuela", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
{
    var texto = r.Query["texto"].FirstOrDefault()?.Trim();
    if (string.IsNullOrEmpty(texto) || texto.Length < 2) throw new ErrorConsulta("Falta el parámetro 'texto' (mínimo 2 caracteres).");
    var (_, limite) = P.OrdenLimite(r);
    return new Respuesta("buscar_escuela", c.Fuente, "no aplica", "no aplica", new { texto, cuentas_de_prueba = P.Cp(c) }, null,
        c.BuscarEscuelas(texto, limite),
        P.Unir("Usa el nombre EXACTO devuelto como filtro 'escuela' en las demás consultas.", c.NotaConexiones));
}));

app.MapGet("/conexiones-por-escuela", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
{
    var (d, h) = P.Rango(r); var (desc, limite) = P.OrdenLimite(r);
    var escuela = P.Escuela(r, c);
    var todasLas = c.ConexionesPorEscuela(d, h, desc, 1000);     // todas las escuelas: permite totales, participación y empates exactos
    int total = todasLas.Sum(f => f.Conexiones);
    var todas = escuela is null ? todasLas : todasLas.Where(f => f.Nombre == escuela).ToList();   // con filtro, solo esa escuela (el % sigue siendo sobre el total)
    var filas = todas.Take(limite).Select(f => f with
    {
        PorcentajeDelTotal = P.Pct(f.Conexiones, total),
        ConexionesPorUsuario = f.UsuariosConectados == 0 ? null : Math.Round((double)f.Conexiones / f.UsuariosConectados, 1)
    }).ToList();
    return new Respuesta("conexiones_por_escuela", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
        new { orden = P.Texto(desc), limite, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, filas,
        P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones), P.Empates(todas, limite, f => f.Conexiones, completa: true)),
        new { total_conexiones = total, escuelas_consideradas = todasLas.Count, escuelas_con_conexiones = todasLas.Count(f => f.Conexiones > 0),
              escuelas_sin_conexiones = todasLas.Count(f => f.Conexiones == 0) });
}));

app.MapGet("/conexiones-por-maestro", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
{
    var (d, h) = P.Rango(r); var (desc, limite) = P.OrdenLimite(r);
    var escuela = P.Escuela(r, c);
    var todos = c.ConexionesPorMaestro(d, h, desc, 5000, escuela);   // todos los maestros: totales, participación y empates exactos
    int total = todos.Sum(f => f.Conexiones);
    var filas = todos.Take(limite).Select(f => f with { PorcentajeDelTotal = P.Pct(f.Conexiones, total) }).ToList();
    return new Respuesta("conexiones_por_maestro", c.Fuente, c.ColumnaFechaConexiones, P.Periodo(d, h),
        new { orden = P.Texto(desc), limite, escuela = escuela ?? "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaConexiones, filas,
        P.Unir(P.Nota(c.NotaConexiones, h, c.DatosHastaConexiones), P.Empates(todos, limite, f => f.Conexiones, completa: true)),
        new { total_conexiones_maestros = total, maestros_considerados = todos.Count, maestros_con_conexiones = todos.Count(f => f.Conexiones > 0),
              maestros_sin_conexiones = todos.Count(f => f.Conexiones == 0) });
}));

app.MapGet("/comparar-areas", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
{
    var (da, ha) = P.Rango(r, "desde_a", "hasta_a"); var (db, hb) = P.Rango(r, "desde_b", "hasta_b");
    return new Respuesta("comparar_areas", c.Fuente, c.ColumnaFechaPruebas, new { a = P.Periodo(da, ha), b = P.Periodo(db, hb) },
        new { escuela = "todas", cuentas_de_prueba = P.Cp(c) }, c.DatosHastaPruebas, c.CompararAreas(da, ha, db, hb),
        P.Nota(c.NotaArea is null ? "Si los dos periodos tienen distinta duración, la comparación no está normalizada." : c.NotaArea + " Si los dos periodos tienen distinta duración, la comparación no está normalizada.", P.Max(ha, hb), c.DatosHastaPruebas));
}));

app.MapGet("/comparar-escuelas", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
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
}));

app.MapGet("/promedio-full-quiz", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
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
}));

app.MapGet("/tendencia-conexiones", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
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
}));

app.MapGet("/conexiones-acumuladas", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
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
}));

app.MapGet("/uso-docentes", (HttpRequest r, IConsultasUso c) => Ejecutar(() =>
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
}));

app.Run();

static IResult Ejecutar(Func<Respuesta> consulta)
{
    try { return Results.Ok(consulta()); }
    catch (ErrorConsulta e) { return Results.Json(new { error = "parametros_invalidos", mensaje = e.Message }, statusCode: 400); }
    catch (MySqlException) { return Results.Json(new { error = "base_no_disponible", mensaje = "No se pudo consultar la base de datos; intenta de nuevo más tarde." }, statusCode: 503); }
}

/// <summary>Lectura y validación de parámetros (mismos mensajes que la API de ejemplo en Python).</summary>
static class P
{
    public static double? Pct(double parte, double total) => total == 0 ? null : Math.Round(100.0 * parte / total, 1);

    public static string Texto(bool desc) => desc ? "desc" : "asc";

    public static DateOnly Max(DateOnly a, DateOnly b) => a > b ? a : b;

    public static string Cp(IConsultasUso c) => c.ExcluyeCuentasDePrueba ? "excluidas" : "incluidas";

    public static string? Unir(string? a, string? b) =>
        string.IsNullOrEmpty(a) ? b : string.IsNullOrEmpty(b) ? a : a + " " + b;

    /// <summary>
    /// Si el valor de la última fila mostrada se repite en filas que quedaron fuera del límite, la lista no es exhaustiva
    /// (p. ej. muchas escuelas con 0 conexiones): se avisa para que el agente no presente 10 al azar como «las únicas».
    /// <paramref name="todas"/> trae hasta 50 filas ya ordenadas.
    /// </summary>
    public static string? Empates<T>(List<T> todas, int limite, Func<T, int> valor, bool completa = false)
    {
        if (todas.Count <= limite) return null;
        var v = valor(todas[limite - 1]);
        var mas = todas.Skip(limite).TakeWhile(f => valor(f) == v).Count();
        if (mas == 0) return null;
        var alMenos = !completa && todas.Count >= 50 && mas == todas.Count - limite ? "al menos " : "";
        return $"EMPATE: hay {alMenos}{mas} fila(s) más con el mismo valor ({v}) que la última mostrada; el orden entre empates es " +
               "alfabético, así que esta lista no es exhaustiva.";
    }

    /// <summary>Si el período pedido termina después del último día con datos, lo avisa para que el agente no lo oculte.</summary>
    public static string? Nota(string? nota, DateOnly hasta, DateOnly? datosHasta)
    {
        if (datosHasta is { } limite && hasta > limite)
        {
            var aviso = $"AVISO: los datos cargados llegan solo hasta {limite:yyyy-MM-dd}; el período pedido termina el {hasta:yyyy-MM-dd}, " +
                        "así que los días posteriores no están incluidos.";
            return nota is null ? aviso : $"{nota} {aviso}";
        }
        return nota;
    }
    public static object Periodo(DateOnly d, DateOnly h) => new { desde = d.ToString("yyyy-MM-dd"), hasta = h.ToString("yyyy-MM-dd") };

    public static DateOnly? FechaOpcional(HttpRequest r, string nombre) =>
        string.IsNullOrEmpty(r.Query[nombre].FirstOrDefault()) ? null : Fecha(r, nombre);

    static DateOnly Fecha(HttpRequest r, string nombre)
    {
        var v = r.Query[nombre].FirstOrDefault();
        if (string.IsNullOrEmpty(v)) throw new ErrorConsulta($"Falta el parámetro '{nombre}' (formato AAAA-MM-DD).");
        if (!DateOnly.TryParseExact(v, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var f))
            throw new ErrorConsulta($"'{nombre}' debe tener el formato AAAA-MM-DD.");
        return f;
    }

    public static (DateOnly, DateOnly) Rango(HttpRequest r, string desde = "desde", string hasta = "hasta")
    {
        var (d, h) = (Fecha(r, desde), Fecha(r, hasta));
        if (d > h) throw new ErrorConsulta($"'{desde}' no puede ser posterior a '{hasta}'.");
        return (d, h);
    }

    public static (bool desc, int limite) OrdenLimite(HttpRequest r)
    {
        var orden = r.Query["orden"].FirstOrDefault() ?? "desc";
        if (orden is not ("asc" or "desc")) throw new ErrorConsulta("'orden' debe ser 'asc' o 'desc'.");
        var txt = r.Query["limite"].FirstOrDefault();
        var limite = 10;
        if (txt is not null && !int.TryParse(txt, out limite)) throw new ErrorConsulta("'limite' debe ser un número entero.");
        if (limite is < 1 or > 50) throw new ErrorConsulta("'limite' debe estar entre 1 y 50.");
        return (orden == "desc", limite);
    }

    public static string? Escuela(HttpRequest r, IConsultasUso c)
    {
        var e = r.Query["escuela"].FirstOrDefault();
        if (!string.IsNullOrEmpty(e) && !c.EscuelaExiste(e)) throw new ErrorConsulta($"La escuela '{e}' no existe.");
        return string.IsNullOrEmpty(e) ? null : e;
    }
}
