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
    builder.Services.AddCors(o => o.AddPolicy("panel", p => p.WithOrigins(origenes).WithHeaders("content-type").WithMethods("GET", "POST").WithExposedHeaders("content-disposition")));   // GET + content-disposition: descarga directa de /exportar desde el panel
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

foreach (var (ruta, f) in Rutas.Consultas)
    app.MapGet("/" + ruta, (HttpRequest r, IConsultasUso c) => Rutas.Ejecutar(() => f(r, c)));

// Exportación: GET /exportar/{consulta}?formato=xlsx|grafica&<mismos parámetros que la consulta>.
// NO está en openapi.json a propósito (D6): el asistente convierte cada path de openapi.json en una herramienta del modelo,
// y el modelo no debe poder pedir archivos. Solo la usa el panel, vía /asistente/descargas/{id}.
app.MapGet("/exportar/{consulta}", (string consulta, HttpRequest r, IConsultasUso c) =>
{
    if (consulta == "buscar-escuela" || !Rutas.Consultas.TryGetValue(consulta, out var f))
        return Results.Json(new { error = "consulta_desconocida", mensaje = $"No se puede exportar '{consulta}'." }, statusCode: 404);
    var formato = r.Query["formato"].FirstOrDefault() ?? "xlsx";
    if (formato is not ("xlsx" or "grafica"))
        return Results.Json(new { error = "parametros_invalidos", mensaje = "'formato' debe ser 'xlsx' o 'grafica'." }, statusCode: 400);
    return Rutas.Intentar(() =>
    {
        if (formato == "xlsx") r.HttpContext.Items["exportar"] = true;           // límite hasta 5000 (P.OrdenLimite)
        var resp = Columnas.ANodo(f(r, c));
        if (formato == "xlsx")
            return Results.File(Excel.Libro(resp), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Columnas.NombreArchivo(resp));
        return Graficas.Desde(resp) is { } spec
            ? Results.Json(spec, Columnas.JsonApi)
            : Results.Json(new { error = "sin_grafica", mensaje = "Esta consulta no tiene gráfica: hacen falta al menos 2 filas y una consulta graficable." }, statusCode: 422);
    });
});

app.Run();

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

    /// <summary>min_conexiones / max_conexiones (opcionales): filtran por el número de conexiones (días con ingreso) de cada fila. «Una sola vez» = 1 y 1.</summary>
    public static (int min, int max) RangoConexiones(HttpRequest r)
    {
        int Entero(string nombre, int defecto)
        {
            var t = r.Query[nombre].FirstOrDefault();
            if (string.IsNullOrEmpty(t)) return defecto;
            if (!int.TryParse(t, out var v) || v < 0) throw new ErrorConsulta($"'{nombre}' debe ser un número entero de 0 en adelante.");
            return v;
        }
        var (mn, mx) = (Entero("min_conexiones", 0), Entero("max_conexiones", int.MaxValue));
        if (mn > mx) throw new ErrorConsulta("'min_conexiones' no puede ser mayor que 'max_conexiones'.");
        return (mn, mx);
    }

    public static (bool desc, int limite) OrdenLimite(HttpRequest r)
    {
        var orden = r.Query["orden"].FirstOrDefault() ?? "desc";
        if (orden is not ("asc" or "desc")) throw new ErrorConsulta("'orden' debe ser 'asc' o 'desc'.");
        // Exportación (GET /exportar/...): el límite sube a 5000 y, sin 'limite', se devuelven todas las filas (hasta 5000).
        var exportar = r.HttpContext.Items["exportar"] is true;
        var max = exportar ? 5000 : 50;
        var txt = r.Query["limite"].FirstOrDefault();
        var limite = exportar ? max : 10;
        if (txt is not null && !int.TryParse(txt, out limite)) throw new ErrorConsulta("'limite' debe ser un número entero.");
        if (limite < 1 || limite > max) throw new ErrorConsulta($"'limite' debe estar entre 1 y {max}.");
        return (orden == "desc", limite);
    }

    public static string? Escuela(HttpRequest r, IConsultasUso c)
    {
        var e = r.Query["escuela"].FirstOrDefault();
        if (!string.IsNullOrEmpty(e) && !c.EscuelaExiste(e)) throw new ErrorConsulta($"La escuela '{e}' no existe.");
        return string.IsNullOrEmpty(e) ? null : e;
    }
}
