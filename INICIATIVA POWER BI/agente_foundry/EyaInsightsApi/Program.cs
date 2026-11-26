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

    // Consultas «del tablero» (P1–P10) sobre el esquema `agente` (sql/09). Cadena propia: EYA_MYSQL_AGENTE, o la misma
    // conexión con base `agente` y, si existen, EYA_MYSQL_AGENTE_USER/_PASSWORD (el usuario de solo lectura agente_lector).
    var cadenaAgente = builder.Configuration["EYA_MYSQL_AGENTE"];
    if (string.IsNullOrWhiteSpace(cadenaAgente))
    {
        var b = new MySqlConnectionStringBuilder(cadena) { Database = "agente" };
        if (!string.IsNullOrWhiteSpace(builder.Configuration["EYA_MYSQL_AGENTE_USER"]))
        {
            b.UserID = builder.Configuration["EYA_MYSQL_AGENTE_USER"];
            b.Password = builder.Configuration["EYA_MYSQL_AGENTE_PASSWORD"];
        }
        cadenaAgente = b.ConnectionString;
    }
    builder.Services.AddSingleton(new ConsultasTablero(cadenaAgente));
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

// ── Consultas «del tablero» (P1–P10), esquema `agente` ─────────────────────────────────────────────
// Sin fechas = todo el histórico. «definicion»: tablero (la medida de Power BI tal cual) o corregida (lo que su nombre promete).
static ConsultasTablero TB(HttpContext ctx) => ctx.RequestServices.GetService<ConsultasTablero>()
    ?? throw new FuenteNoDisponible("Las consultas del tablero solo funcionan con Datos:Origen=mysql (esquema agente).");

app.MapGet("/resumen-usuarios", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx); var escuela = PT.Escuela(ctx.Request, t);
    var (porRol, total) = t.ResumenUsuarios(escuela);
    return new Respuesta("resumen_usuarios", t.Fuente, "no aplica (foto actual)", "foto actual", new { escuela = escuela ?? "todas" }, null, porRol,
        "Usuarios = usuarios distintos sin el rol client ni la Escuela Facilitadores MSJ (igual que Power BI). Un usuario puede tener a la vez " +
        "suscripciones activas y canceladas: activos + cancelados + sin_suscripcion NO suman usuarios; solo_cancelados son los que no tienen ninguna activa.",
        new { total_usuarios = total.Usuarios, usuarios_activos = total.Activos, usuarios_cancelados = total.Cancelados,
              usuarios_solo_cancelados = total.SoloCancelados, usuarios_sin_suscripcion = total.SinSuscripcion,
              usuarios_activos_y_cancelados = total.ActivosYCancelados, porcentaje_activos = ConsultasTablero.Pct(total.Activos, total.Usuarios, 1) });
}));

app.MapGet("/conexion-por-escuela-pct", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx); var r = ctx.Request;
    var (d, h) = PT.RangoOpcional(r); var (desc, limite) = P.OrdenLimite(r); var def = PT.Definicion(r);
    var todas = t.ConexionPorEscuelaPct(d, h, def == "corregida");
    var ord = (desc ? todas.OrderByDescending(f => f.PorcentajeConectados ?? 0).ThenByDescending(f => f.Escuela, StringComparer.Ordinal)
                    : todas.OrderBy(f => f.PorcentajeConectados ?? 0).ThenBy(f => f.Escuela, StringComparer.Ordinal)).ToList();
    var activos = todas.Sum(f => f.UsuariosActivos);
    return new Respuesta("conexion_por_escuela_pct", t.Fuente, "agente.conexiones.connected_date", PT.Periodo(d, h),
        new { orden = P.Texto(desc), limite, definicion = def }, t.DatosHastaConexiones(), ord.Take(limite).ToList(),
        P.Unir(def == "tablero"
            ? "Definición del tablero: usuarios_conectados = usuarios distintos de la escuela con alguna conexión en el período, tengan o no suscripción activa; usuarios_activos = usuarios con suscripción activa hoy. Por eso el % puede pasar de 100."
            : "Definición corregida: usuarios_conectados = solo usuarios con suscripción activa que se conectaron en el período.",
            P.Empates(ord, limite, f => (int)Math.Round((f.PorcentajeConectados ?? 0) * 100), completa: true)),
        new { escuelas = todas.Count, escuelas_sin_conectados = todas.Count(f => f.UsuariosConectados == 0), total_usuarios_activos = activos });
}));

app.MapGet("/no-conectados-por-rol", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx); var r = ctx.Request;
    var (d, h) = PT.RangoOpcional(r); var def = PT.Definicion(r);
    var (porRol, total) = t.NoConectadosPorRol(d, h, def == "corregida");
    return new Respuesta("no_conectados_por_rol", t.Fuente, "agente.conexiones.connected_date", PT.Periodo(d, h),
        new { definicion = def }, t.DatosHastaConexiones(), porRol,
        def == "tablero"
            ? "Definición del tablero (medida «Usuarios NO Conectados (Activos por Rol)»): no_conectados = activos − conectados, pero «conectados» incluye usuarios SIN suscripción activa, así que subestima a los no conectados. Pide definicion=corregida para la cifra real."
            : "Definición corregida: conectados = solo usuarios con suscripción activa que se conectaron en el período; no_conectados = activos que no se conectaron.",
        new { total_activos = total.Activos, total_conectados = total.Conectados, total_no_conectados = total.NoConectados,
              porcentaje_no_conectados = ConsultasTablero.Pct(total.NoConectados, total.Activos, 1),
              aviso_total = "El total usa usuarios distintos; no es la suma de los roles." });
}));

app.MapGet("/escuelas-visitadas", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx);
    var estado = ctx.Request.Query["estado"].FirstOrDefault() ?? "todas";
    if (estado is not ("todas" or "visitada" or "no_visitada")) throw new ErrorConsulta("'estado' debe ser 'todas', 'visitada' o 'no_visitada'.");
    var (todas, cargado) = t.EscuelasVisitadas();
    var filas = todas.Where(f => estado == "todas" || f.Visitada == (estado == "visitada")).ToList();
    return new Respuesta("escuelas_visitadas", "excel:escuelas visitadas (cargado en agente.escuelas_visitadas)", "no aplica (foto actual)", "foto actual",
        new { estado }, cargado is { } c ? DateOnly.FromDateTime(c) : null, filas,
        "Fuente: Excel de visitas a escuelas que mantiene el equipo; 'datos_hasta' es el día en que se cargó. Si una fila trae 'alerta', menciónala.",
        new { escuelas_visitadas = todas.Count(f => f.Visitada), escuelas_no_visitadas = todas.Count(f => !f.Visitada), total_escuelas = todas.Count,
              filas_con_alerta = todas.Count(f => f.Alerta is not null) });
}));

app.MapGet("/quizzes-por-periodo", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx); var r = ctx.Request;
    var anio = PT.AnioAcademico(r);
    var periodo = r.Query["periodo"].FirstOrDefault()?.ToUpperInvariant();
    if (periodo is not (null or "P1" or "P2" or "P3" or "P4")) throw new ErrorConsulta("'periodo' debe ser P1, P2, P3 o P4.");
    var excluir = !string.Equals(r.Query["excluir_posprueba"].FirstOrDefault(), "false", StringComparison.OrdinalIgnoreCase);
    var filas = t.QuizzesPorPeriodo(anio, periodo, excluir);
    int tom = filas.Sum(f => f.Tomados), asig = filas.Sum(f => f.Asignados);
    return new Respuesta("quizzes_por_periodo", t.Fuente, "agente.pruebas.academic_year_start (año académico ago–jul) y period_no", "por año académico y período",
        new { anio_academico = anio ?? "todos", periodo = periodo ?? "todos", excluir_posprueba = excluir }, null, filas,
        "Asignados = tomados + no tomados (filas de vw_academics_details, como «Pruebas asignadas» de Power BI). " +
        (excluir ? "Sin la Posprueba, como la página «Reporte Académico»." : "Incluye la Posprueba."),
        new { total_asignados = asig, total_tomados = tom, total_no_tomados = asig - tom, porcentaje_tomados = ConsultasTablero.Pct(tom, asig) });
}));

app.MapGet("/conexiones-por-ubicacion", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx); var r = ctx.Request;
    var (d, h) = PT.RangoOpcional(r);
    var agrupar = r.Query["agrupar"].FirstOrDefault() ?? "ciudad";
    if (agrupar is not ("ciudad" or "ore")) throw new ErrorConsulta("'agrupar' debe ser 'ciudad' u 'ore'.");
    var fac = PT.IncluirFacilitadores(r);
    var filas = t.ConexionesPorUbicacion(d, h, agrupar, fac);
    var (u, n) = t.TotalConexiones(d, h, fac);
    return new Respuesta("conexiones_por_ubicacion", t.Fuente, "agente.conexiones.connected_date", PT.Periodo(d, h),
        new { agrupar, incluir_facilitadores = fac }, t.DatosHastaConexiones(), filas,
        "usuarios_conectados = usuarios distintos; conexiones = un usuario en un día. " +
        (fac ? "Incluye la Escuela Facilitadores MSJ." : "Sin la Escuela Facilitadores MSJ, como la página de conexiones de Power BI."),
        new { total_usuarios_conectados = u, total_conexiones = n, grupos = filas.Count });
}));

app.MapGet("/conexiones-por-mes", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx); var r = ctx.Request;
    var anio = PT.AnioAcademico(r);
    var (d0, h0) = PT.RangoOpcional(r);
    DateOnly d, h;
    if (anio is not null) { var a = int.Parse(anio[..4], CultureInfo.InvariantCulture); (d, h) = (new DateOnly(a, 8, 1), new DateOnly(a + 1, 7, 31)); }
    else if (d0 is { } x && h0 is { } y) (d, h) = (x, y);
    else throw new ErrorConsulta("Indica 'anio_academico' (AAAA-AAAA, de agosto a julio) o 'desde' y 'hasta'.");
    var fac = PT.IncluirFacilitadores(r);
    var serie = t.ConexionesPorMes(d, h, fac);
    var (u, n) = t.TotalConexiones(d, h, fac);
    var hasta = t.DatosHastaConexiones();
    var mayor = serie.OrderByDescending(f => f.Conexiones).ThenBy(f => f.Mes, StringComparer.Ordinal).FirstOrDefault();
    return new Respuesta("conexiones_por_mes", t.Fuente, "agente.conexiones.connected_date", P.Periodo(d, h),
        new { anio_academico = anio, incluir_facilitadores = fac }, hasta, serie,
        P.Nota("Conexiones = un usuario en un día; usuarios_conectados = usuarios distintos del mes (no se suman entre meses: el total de usuarios está en 'resumen'). " +
               "Año académico = 1 de agosto a 31 de julio (Power BI usa el año calendario en su columna «Año académico»).", h, hasta),
        new { total_conexiones = n, total_usuarios_conectados = u, mes_con_mas_conexiones = mayor?.Mes, conexiones_del_mayor = mayor?.Conexiones });
}));

app.MapGet("/suscripciones-por-anio", (HttpContext ctx) => Ejecutar(() =>
{
    var t = TB(ctx); var r = ctx.Request;
    var criterio = r.Query["criterio"].FirstOrDefault() ?? "fin";
    if (criterio is not ("fin" or "activacion")) throw new ErrorConsulta("'criterio' debe ser 'fin' o 'activacion'.");
    var anio = PT.AnioAcademico(r);
    var filas = t.SuscripcionesPorAnio(criterio, anio);
    return new Respuesta("suscripciones_por_anio", t.Fuente,
        criterio == "fin" ? "agente.suscripciones.cancellation_date (fecha de fin programada)" : "agente.suscripciones.activated_date",
        "por año académico", new { criterio, anio_academico = anio ?? "todos" }, null, filas,
        criterio == "fin"
            ? "Criterio «fin» = «Academic Year» del tablero: año en que termina la suscripción (cancellation_date es la fecha de fin programada, no un evento de cancelación). Los usuarios no se suman entre años."
            : "Criterio «activacion»: año académico (ago–jul) en que se activó la suscripción. Los usuarios no se suman entre años.");
}));

app.Run();

static IResult Ejecutar(Func<Respuesta> consulta)
{
    try { return Results.Ok(consulta()); }
    catch (ErrorConsulta e) { return Results.Json(new { error = "parametros_invalidos", mensaje = e.Message }, statusCode: 400); }
    catch (FuenteNoDisponible e) { return Results.Json(new { error = "fuente_no_disponible", mensaje = e.Message }, statusCode: 503); }
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

/// <summary>Parámetros de las consultas del tablero (fechas opcionales, definición, año académico).</summary>
static class PT
{
    public static (DateOnly?, DateOnly?) RangoOpcional(HttpRequest r)
    {
        bool hayD = !string.IsNullOrEmpty(r.Query["desde"].FirstOrDefault()), hayH = !string.IsNullOrEmpty(r.Query["hasta"].FirstOrDefault());
        if (!hayD && !hayH) return (null, null);
        if (hayD != hayH) throw new ErrorConsulta("Indica 'desde' y 'hasta' juntos, o ninguno (todo el histórico).");
        var (d, h) = P.Rango(r);
        return (d, h);
    }

    public static object Periodo(DateOnly? d, DateOnly? h) =>
        d is { } x && h is { } y ? P.Periodo(x, y) : "todo el histórico";

    public static string Definicion(HttpRequest r)
    {
        var d = r.Query["definicion"].FirstOrDefault() ?? "tablero";
        if (d is not ("tablero" or "corregida")) throw new ErrorConsulta("'definicion' debe ser 'tablero' o 'corregida'.");
        return d;
    }

    public static string? AnioAcademico(HttpRequest r)
    {
        var a = r.Query["anio_academico"].FirstOrDefault();
        if (string.IsNullOrEmpty(a)) return null;
        if (a.Length != 9 || a[4] != '-' || !int.TryParse(a[..4], out var i) || !int.TryParse(a[5..], out var f) || f != i + 1)
            throw new ErrorConsulta("'anio_academico' debe tener el formato AAAA-AAAA con años seguidos, p. ej. 2025-2026.");
        return a;
    }

    public static bool IncluirFacilitadores(HttpRequest r) =>
        string.Equals(r.Query["incluir_facilitadores"].FirstOrDefault(), "true", StringComparison.OrdinalIgnoreCase);

    public static string? Escuela(HttpRequest r, ConsultasTablero t)
    {
        var e = r.Query["escuela"].FirstOrDefault();
        if (string.IsNullOrEmpty(e)) return null;
        if (!t.EscuelaExiste(e)) throw new ErrorConsulta($"La escuela '{e}' no existe.");
        return e;
    }
}
