using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Azure.Core;
using Azure.Identity;

namespace EyaInsightsApi;

// ── Contrato del servicio POST /asistente/preguntar (JSON en snake_case) ─────────────────────────────────────────
public record ContextoTablero(string? Reporte, string? Pagina);
public record PreguntaAsistente(string? Pregunta, string? ConversacionId, ContextoTablero? Contexto, string? Idioma = null);

/// <param name="Respuesta">Texto para mostrar (sin el bloque «Referencias:» que redacta el modelo).</param>
/// <param name="Referencias">Una por consulta ejecutada, copiada de la respuesta de la API (no escrita por el modelo).</param>
/// <param name="Avisos">Frases AVISO/EMPATE que la API devolvió (datos hasta, empates en el corte…).</param>
/// <param name="Advertencias">Verificación automática: cifras de la respuesta que no salen de las consultas.</param>
/// <param name="Consultas">Qué consulta y con qué parámetros se ejecutó (transparencia).</param>
/// <param name="Descargas">Fase 4: un archivo .xlsx por consulta exitosa (el servidor lo registra, el modelo no escribe enlaces): {id, nombre, consulta, url, destacar}.</param>
/// <param name="Graficas">Fase 4: especificación de gráfica (formato Recharts) de las consultas graficables, calculada de las mismas cifras de la consulta.</param>
public record RespuestaAsistente(string Respuesta, string ConversacionId, List<JsonNode> Referencias, List<string> Avisos,
    List<string> Advertencias, List<JsonNode> Consultas, string? Error = null,
    List<JsonNode>? Descargas = null, List<JsonNode>? Graficas = null);

/// <summary>Resultado de ejecutar una herramienta por loopback. Ruta/Query = lo que de verdad se pidió (argumentos ya saneados).</summary>
public record ResultadoHerramienta(int Estado, string Cuerpo, string? Ruta = null, string? Query = null);

/// <summary>Una descarga registrada: solo la consulta (no datos), ligada a quien preguntó. Se reconstruye al hacer clic.</summary>
public record EntradaDescarga(string Consulta, string Query, string Usuario, Alcance Alcance, DateTime Expira);

public class ErrorFoundry(int estado, string cuerpo) : Exception($"Foundry respondió {estado}")
{
    public int Estado { get; } = estado;
    public string Cuerpo { get; } = cuerpo;
    public bool FiltroContenido => Cuerpo.Contains("content_filter", StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// El agente: recibe la pregunta, la envía a un modelo de Foundry con las consultas como herramientas y ejecuta las
/// que el modelo pida contra ESTA misma API (mismas validaciones, notas y referencias que ve cualquier cliente).
/// La base nunca queda expuesta: el modelo solo ve el JSON agregado que devuelven las consultas.
/// Opción B del plan (el bucle corre en nuestro servicio, no como herramienta OpenAPI de Foundry).
/// </summary>
public sealed class AsistenteFoundry
{
    const int MaxPasos = 6;
    static readonly TimeSpan VidaConversacion = TimeSpan.FromHours(2);
    static readonly Regex FechaIso = new(@"(\d{4})-(\d{2})-(\d{2})(T[\d:.]+Z?)?", RegexOptions.Compiled);
    static readonly Regex Aviso = new(@"(AVISO:|EMPATE:)[^\n]*?\.(?=\s|$)", RegexOptions.Compiled);

    readonly HttpClient _http;
    readonly TokenCredential _credencial;
    readonly string _endpoint, _modelo, _plantilla;
    readonly JsonArray _herramientas = new();
    readonly Dictionary<string, string> _rutas = new();
    readonly ConcurrentDictionary<string, (string Id, DateTime Cuando, string Usuario)> _conversaciones = new();
    readonly ConcurrentDictionary<string, EntradaDescarga> _descargas = new();
    const int MaxDescargas = 1000;
    static readonly Regex PideExcel = new(@"excel|xlsx|descarg|hoja de c[aá]lculo|spreadsheet|download", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex PideGrafica = new(@"gr[aá]fic|chart|graph|plot", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    AccessToken _token;

    public AsistenteFoundry(IConfiguration cfg, bool produccion)
    {
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(90) };
        _endpoint = (cfg["Foundry:Endpoint"] ?? "https://chatbotaipowerbi.services.ai.azure.com/api/projects/proj-default").TrimEnd('/');
        _modelo = cfg["Foundry:Modelo"] ?? "gpt-4.1-mini";
        // Asistente:Credencial = azcli (desarrollo: la sesión de `az login`) | managed (Azure: identidad administrada del App Service, sin claves;
        // necesita el rol «Azure AI User» en el proyecto de Foundry; Asistente__ClientId elige una identidad asignada por el usuario) |
        // prueba (SOLO fuera de Production: token ficticio, para probar contra foundry_falso.py). Por defecto = managed.
        var cred = (cfg["Asistente:Credencial"] ?? "managed").Trim().ToLowerInvariant();
        _credencial = cred switch
        {
            "azcli" => new AzureCliCredential(),
            "prueba" when !produccion => new CredencialDePrueba(),      // produccion = IHostEnvironment.IsProduction() (también cuando no hay variable de entorno)
            _ => new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ExcludeInteractiveBrowserCredential = true,
                ManagedIdentityClientId = string.IsNullOrWhiteSpace(cfg["Asistente:ClientId"]) ? null : cfg["Asistente:ClientId"],
            }),
        };
        _plantilla = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "instrucciones.txt"), Encoding.UTF8).Trim();
        CargarHerramientas(Path.Combine(AppContext.BaseDirectory, "openapi.json"));
    }

    public string Modelo => _modelo;
    public int Herramientas => _herramientas.Count;

    // Las herramientas salen de openapi.json (el mismo contrato que usa el agente de Foundry): una sola fuente.
    void CargarHerramientas(string ruta)
    {
        var spec = JsonNode.Parse(File.ReadAllText(ruta, Encoding.UTF8))!;
        foreach (var (rutaApi, metodos) in spec["paths"]!.AsObject())
        {
            var op = metodos!["get"]!;
            var props = new JsonObject();
            var requeridos = new JsonArray();
            foreach (var par in op["parameters"]!.AsArray())
            {
                var p = par!;
                if (p["$ref"] is JsonNode r) p = spec["components"]!["parameters"]![r.GetValue<string>().Split('/').Last()]!;
                var nombre = p["name"]!.GetValue<string>();
                var esquema = new JsonObject();
                foreach (var k in new[] { "type", "enum" })
                    if (p["schema"]![k] is JsonNode v) esquema[k] = v.DeepClone();
                esquema["description"] = p["description"]!.GetValue<string>();
                props[nombre] = esquema;
                if (p["required"]?.GetValue<bool>() == true) requeridos.Add(nombre);
            }
            var id = op["operationId"]!.GetValue<string>();
            _herramientas.Add(new JsonObject
            {
                ["type"] = "function",
                ["name"] = id,
                ["description"] = $"{op["summary"]!.GetValue<string>()}. {op["description"]!.GetValue<string>()}",
                ["parameters"] = new JsonObject { ["type"] = "object", ["properties"] = props, ["required"] = requeridos },
            });
            _rutas[id] = rutaApi;
        }
    }

    // ── Ejecución de una herramienta contra la propia API (bucle local) ─────────────────────────────────────────
    /// <summary>
    /// Llama por loopback a la ruta de la herramienta REENVIANDO la identidad de quien pregunta (<paramref name="headers"/>: su Token, o x-api-key si es
    /// un servicio): la API vuelve a autenticar y a aplicar el alcance (ORE/escuela), así el modelo no puede salirse del alcance de la persona.
    /// </summary>
    public async Task<ResultadoHerramienta> EjecutarAsync(HttpClient http, string baseUrl, IEnumerable<(string Nombre, string Valor)> headers,
        string operacion, JsonObject args, CancellationToken ct)
    {
        if (!_rutas.TryGetValue(operacion, out var ruta))
            return new(400, JsonSerializer.Serialize(new { error = "consulta_desconocida", mensaje = $"No existe la consulta '{operacion}'." }));
        // El modelo a veces inventa argumentos que la consulta no tiene: se descartan para que no cambien el resultado sin que se note.
        var declarados = _herramientas.FirstOrDefault(h => h!["name"]!.GetValue<string>() == operacion)?["parameters"]?["properties"]?.AsObject();
        if (declarados is not null)
            foreach (var k in args.Select(kv => kv.Key).Where(k => !declarados.ContainsKey(k)).ToList()) args.Remove(k);
        static string Valor(JsonNode? n) => n switch
        {
            JsonValue v when v.TryGetValue<string>(out var s) => s,
            null => "",
            _ => n.ToJsonString().Trim('"'),
        };
        var q = string.Join("&", args.Where(kv => kv.Value is not null)
            .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(Valor(kv.Value))}"));
        using var req = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{ruta}?{q}");
        foreach (var (nombre, valor) in headers) req.Headers.TryAddWithoutValidation(nombre, valor);
        using var resp = await http.SendAsync(req, ct);
        return new((int)resp.StatusCode, await resp.Content.ReadAsStringAsync(ct), ruta, q);
    }

    // ── Descargas (Fase 4 · T6/A3): id opaco de 128 bits ↔ consulta, ligado a quien preguntó ────────────────────────────
    static string ClaveUsuario(Usuario u) => $"{u.Origen}:{u.Id}";

    string RegistrarDescarga(string consulta, string query, Usuario u)
    {
        var ahora = DateTime.UtcNow;
        foreach (var k in _descargas.Where(kv => kv.Value.Expira <= ahora).Select(kv => kv.Key).ToList()) _descargas.TryRemove(k, out _);
        while (_descargas.Count >= MaxDescargas && _descargas.OrderBy(kv => kv.Value.Expira).Select(kv => kv.Key).FirstOrDefault() is { } viejo)
            _descargas.TryRemove(viejo, out _);
        var id = Guid.NewGuid().ToString("N");
        _descargas[id] = new EntradaDescarga(consulta, query, ClaveUsuario(u), u.Alcance, ahora + VidaConversacion);
        return id;
    }

    /// <summary>null si no existe, venció o es de OTRA persona (mismo resultado: no se revela que el id existe).</summary>
    public EntradaDescarga? BuscarDescarga(string id, Usuario u) =>
        _descargas.TryGetValue(id, out var e) && e.Expira > DateTime.UtcNow && e.Usuario == ClaveUsuario(u) ? e : null;

    static readonly string[] ParamsDeGrafica = ["formato", "limite", "todas", "incluir_ceros"];

    /// <summary>
    /// Query de la gráfica de una consulta del chat: los mismos filtros, sin 'limite' (la consulta del chat trae pocas filas) y con formato=grafica.
    /// Si el modelo pidió un 'limite' de 1 a 10 se respeta como nº de categorías de la vista previa; 'orden' se conserva (si no, mayor a menor).
    /// </summary>
    public static string QueryDeGrafica(string query)
    {
        var partes = query.Split('&', StringSplitOptions.RemoveEmptyEntries).ToList();
        var lim = partes.FirstOrDefault(x => x.StartsWith("limite=", StringComparison.Ordinal));
        var salida = partes.Where(x => !ParamsDeGrafica.Any(k => x.StartsWith(k + "=", StringComparison.Ordinal))).ToList();
        if (lim is not null && int.TryParse(lim[7..], out var n) && n is >= 1 and <= OpcionesGrafica.LimiteVistaPrevia) salida.Add("limite=" + n);
        salida.Add("formato=grafica");
        return string.Join("&", salida);
    }

    /// <summary>Query de la gráfica de una descarga registrada, con las opciones de la persona (todas, orden, incluir_ceros, limite) por encima de las del chat.</summary>
    public static string QueryDeGraficaAmpliada(string queryDescarga, IQueryCollection pedido)
    {
        var sustituye = new[] { "todas", "orden", "incluir_ceros", "limite" }.Where(k => pedido.ContainsKey(k)).ToHashSet();
        var partes = queryDescarga.Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Where(x => !ParamsDeGrafica.Any(k => x.StartsWith(k + "=", StringComparison.Ordinal)) && !sustituye.Any(k => x.StartsWith(k + "=", StringComparison.Ordinal))).ToList();
        foreach (var k in sustituye) partes.Add($"{k}={Uri.EscapeDataString(pedido[k].ToString())}");
        partes.Add("formato=grafica");
        return string.Join("&", partes);
    }

    /// <summary>Query de la descarga: la de la consulta del chat SIN 'limite' (el Excel trae la lista completa, hasta 5000 filas) y con formato=xlsx.</summary>
    public static string QueryDeExcel(string query)
    {
        var partes = query.Split('&', StringSplitOptions.RemoveEmptyEntries).Where(x => !x.StartsWith("limite=", StringComparison.Ordinal) && !x.StartsWith("formato=", StringComparison.Ordinal));
        return string.Join("&", partes.Append("formato=xlsx"));
    }

    // ── Llamada a Foundry (Responses API) ──────────────────────────────────────────────────────────────────────
    async Task<string> TokenAsync(CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(_token.Token) && _token.ExpiresOn > DateTimeOffset.UtcNow.AddMinutes(3)) return _token.Token;
        _token = await _credencial.GetTokenAsync(new TokenRequestContext(new[] { "https://ai.azure.com/.default" }), ct);
        return _token.Token;
    }

    async Task<JsonDocument> LlamarAsync(JsonObject cuerpo, CancellationToken ct)
    {
        for (var intento = 0; ; intento++)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, _endpoint + "/openai/v1/responses")
            { Content = new StringContent(cuerpo.ToJsonString(), Encoding.UTF8, "application/json") };
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await TokenAsync(ct));
            using var resp = await _http.SendAsync(req, ct);
            var texto = await resp.Content.ReadAsStringAsync(ct);
            if (resp.StatusCode == HttpStatusCode.TooManyRequests && intento == 0) { await Task.Delay(2500, ct); continue; }
            if (!resp.IsSuccessStatusCode) throw new ErrorFoundry((int)resp.StatusCode, texto);
            return JsonDocument.Parse(texto);
        }
    }

    // ── La pregunta ────────────────────────────────────────────────────────────────────────────────────────────
    /// <param name="pedir">GET por loopback (con la identidad de quien pregunta) a una ruta relativa de esta API: se usa para pedir la gráfica ya ordenada, sin ceros y con su total.</param>
    public async Task<RespuestaAsistente> PreguntarAsync(PreguntaAsistente p, Usuario usuario, Func<string, JsonObject, Task<ResultadoHerramienta>> ejecutar,
        Func<string, Task<(int Estado, string Cuerpo)>> pedir, CancellationToken ct)
    {
        var pregunta = (p.Pregunta ?? "").Trim();
        if (pregunta.Length > 1000) pregunta = pregunta[..1000];
        // La conversación pertenece a quien la empezó: con el id de otra persona se empieza una nueva (mismo resultado que un id desconocido).
        var conversacion = !string.IsNullOrEmpty(p.ConversacionId) && _conversaciones.TryGetValue(p.ConversacionId, out var duena) && duena.Usuario == ClaveUsuario(usuario)
            ? p.ConversacionId : Guid.NewGuid().ToString("N");
        string? ultimoId = _conversaciones.TryGetValue(conversacion, out var previa) ? previa.Id : null;

        var contexto = ContextoTexto(p.Contexto, p.Idioma);
        // En una conversación que continúa, cada mensaje se marca como pregunta NUEVA: el historial es solo contexto (regla 5b), no pedidos pendientes.
        if (ultimoId is not null) contexto = (contexto.Length > 0 ? contexto + "\n" : "") + "[Mensaje nuevo: responde SOLO a esta pregunta; no retomes pedidos ni aclaraciones de mensajes anteriores]";
        JsonNode entrada = JsonValue.Create(contexto.Length > 0 ? contexto + "\n" + pregunta : pregunta)!;
        var hoy = DateTime.UtcNow.AddHours(-4).ToString("yyyy-MM-dd");                                         // día en hora de Puerto Rico
        // Las fechas relativas las calcula el código (el modelo se equivoca con «últimos 3 meses completos»).
        var h = DateOnly.ParseExact(hoy, "yyyy-MM-dd");
        var mesActual = new DateOnly(h.Year, h.Month, 1);
        var instrucciones = _plantilla.Replace("{hoy}", hoy)
            .Replace("{trimestre_desde}", mesActual.AddMonths(-3).ToString("yyyy-MM-dd")).Replace("{trimestre_hasta}", mesActual.AddDays(-1).ToString("yyyy-MM-dd"))
            .Replace("{mes_pasado_desde}", mesActual.AddMonths(-1).ToString("yyyy-MM-dd")).Replace("{mes_pasado_hasta}", mesActual.AddDays(-1).ToString("yyyy-MM-dd"))
            .Replace("{inicio_anio}", new DateOnly(h.Year, 1, 1).ToString("yyyy-MM-dd"))
            .Replace("{anio_academico_desde}", AnioAcademico(h).Desde.ToString("yyyy-MM-dd")).Replace("{anio_academico_hasta}", AnioAcademico(h).Hasta.ToString("yyyy-MM-dd"))
            .Replace("{anio_academico_actual_desde}", AnioAcademico(h).ActualDesde.ToString("yyyy-MM-dd"));

        var referencias = new List<JsonNode>(); var consultas = new List<JsonNode>();
        var descargas = new List<JsonNode>(); var graficas = new List<JsonNode>(); var vistas = new HashSet<string>();
        bool pideExcel = PideExcel.IsMatch(pregunta), pideGrafica = PideGrafica.IsMatch(pregunta);
        var salidas = new List<string>(); var argumentos = new List<string>(); var avisos = new List<string>();
        _cifrasPrevias.TryGetValue(conversacion, out var previas);     // cifras de los turnos anteriores de esta conversación (preguntas de seguimiento)
        var texto = "";

        for (var paso = 0; paso < MaxPasos; paso++)
        {
            var cuerpo = new JsonObject
            {
                ["model"] = _modelo, ["instructions"] = instrucciones, ["input"] = entrada,
                ["tools"] = _herramientas.DeepClone(), ["store"] = true,
            };
            if (ultimoId is not null) cuerpo["previous_response_id"] = ultimoId;

            using var doc = await LlamarAsync(cuerpo, ct);
            ultimoId = doc.RootElement.GetProperty("id").GetString();
            var pendientes = new List<(string Nombre, string Args, string CallId)>();
            var mensajes = new List<string>();            // un texto por cada mensaje del modelo (puede haber varios en la misma respuesta)
            foreach (var item in doc.RootElement.GetProperty("output").EnumerateArray())
            {
                var tipo = item.GetProperty("type").GetString();
                if (tipo == "function_call")
                    pendientes.Add((item.GetProperty("name").GetString()!, item.GetProperty("arguments").GetString() ?? "{}", item.GetProperty("call_id").GetString()!));
                else if (tipo == "message")
                {
                    var sb = new StringBuilder();
                    foreach (var c in item.GetProperty("content").EnumerateArray())
                        if (c.GetProperty("type").GetString() == "output_text") sb.Append(c.GetProperty("text").GetString());
                    if (sb.Length > 0) mensajes.Add(sb.ToString());
                }
            }
            if (pendientes.Count == 0) { texto = UnirMensajes(mensajes); break; }

            var resultados = new JsonArray();
            foreach (var (nombre, args, callId) in pendientes)
            {
                var objArgs = JsonNode.Parse(args)?.AsObject() ?? new JsonObject();
                var (estado, salida, rutaApi, query) = await ejecutar(nombre, objArgs);
                salidas.Add(salida); argumentos.Add(args);
                consultas.Add(new JsonObject { ["herramienta"] = nombre, ["argumentos"] = objArgs.DeepClone(), ["estado"] = estado });
                if (estado == 200)
                {
                    if (Referencia(salida) is JsonNode r) referencias.Add(r);
                    foreach (var a in AvisosDe(salida)) if (!avisos.Contains(a)) avisos.Add(a);
                    // Descarga y gráfica: las arma el servidor con las mismas cifras de la consulta (el modelo no escribe enlaces ni cifras).
                    if (rutaApi is not null && rutaApi != "/buscar-escuela" && query is not null && vistas.Add(rutaApi + "?" + query)
                        && JsonNode.Parse(salida) is JsonObject nodo)
                    {
                        var consulta = rutaApi.TrimStart('/');
                        var id = RegistrarDescarga(consulta, QueryDeExcel(query), usuario);
                        descargas.Add(new JsonObject { ["id"] = id, ["nombre"] = Columnas.NombreArchivo(nodo), ["consulta"] = consulta,
                            ["url"] = "/asistente/descargas/" + id, ["destacar"] = pideExcel });
                        // La gráfica sale de /exportar/{consulta}?formato=grafica (todas las filas: ordenada, sin ceros, vista previa de 10 y con su total),
                        // no de las filas del chat. Con el id de la descarga el panel pide después la versión ampliada (GET /asistente/graficas/{id}).
                        if (Graficas.Graficable(consulta))
                        {
                            var (eg, cg) = await pedir($"/exportar/{Uri.EscapeDataString(consulta)}?{QueryDeGrafica(query)}");
                            if (eg == 200 && JsonNode.Parse(cg) is JsonObject spec)
                            {
                                spec["id"] = id; spec["url"] = "/asistente/graficas/" + id; spec["destacar"] = pideGrafica;
                                graficas.Add(spec);
                            }
                        }
                    }
                }
                resultados.Add(new JsonObject { ["type"] = "function_call_output", ["call_id"] = callId, ["output"] = salida });
            }
            entrada = resultados;
        }

        if (string.IsNullOrWhiteSpace(texto)) texto = "No pude completar la consulta. Intenta de nuevo en un momento.";
        _conversaciones[conversacion] = (ultimoId!, DateTime.UtcNow, ClaveUsuario(usuario));
        foreach (var k in _conversaciones.Where(kv => DateTime.UtcNow - kv.Value.Cuando > VidaConversacion).Select(kv => kv.Key).ToList())
            _conversaciones.TryRemove(k, out _);

        _cifrasPrevias[conversacion] = Acumular(previas, pregunta, salidas, argumentos);
        foreach (var k in _cifrasPrevias.Keys.Where(k => !_conversaciones.ContainsKey(k)).ToList()) _cifrasPrevias.TryRemove(k, out _);
        var visible = QuitarBloqueReferencias(texto);
        // Los AVISO/EMPATE de la API se muestran aparte (recuadro amarillo): si el modelo los repitió como línea suelta, se quitan.
        if (avisos.Count > 0) visible = Regex.Replace(visible, @"(?im)^\s*(AVISO|EMPATE):[^\n]*\n?", "").Trim();
        return new RespuestaAsistente(visible, conversacion, referencias, avisos,
            Verificar(pregunta, contexto, visible, salidas, argumentos, hoy, previas), consultas,
            Descargas: descargas.Count > 0 ? descargas : null, Graficas: graficas.Count > 0 ? graficas : null);
    }

    /// <summary>
    /// Año académico por defecto (SUPUESTO documentado: agosto a julio). «El último año académico» = el año académico COMPLETO más reciente;
    /// «este año académico» = desde el 1 de agosto vigente hasta hoy. El asistente lo dice en la respuesta y ofrece cambiarlo.
    /// </summary>
    public static (DateOnly Desde, DateOnly Hasta, DateOnly ActualDesde) AnioAcademico(DateOnly hoy)
    {
        var inicio = hoy.Month >= 8 ? hoy.Year : hoy.Year - 1;          // el año académico en curso empezó el 1-ago de este año
        return (new DateOnly(inicio - 1, 8, 1), new DateOnly(inicio, 7, 31), new DateOnly(inicio, 8, 1));
    }

    // ── Utilidades ─────────────────────────────────────────────────────────────────────────────────────────────
    static string Corto(string? s) { var t = (s ?? "").Replace('\n', ' ').Replace('«', '"').Replace('»', '"').Trim(); return t.Length > 80 ? t[..80] : t; }

    static string ContextoTexto(ContextoTablero? c, string? idioma)
    {
        var partes = new List<string>();
        if (c is not null && !(string.IsNullOrWhiteSpace(c.Reporte) && string.IsNullOrWhiteSpace(c.Pagina)))
            partes.Add($"[Contexto del tablero (solo orientación, no es un dato ni un período): informe = «{Corto(c.Reporte)}», página = «{Corto(c.Pagina)}»]");
        // Idioma del PANEL (el de la plataforma): solo desempata cuando la pregunta no deja claro el suyo (regla 5a). Va SIEMPRE, también en español.
        var en = idioma is not null && idioma.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        partes.Add($"[Idioma del panel: {(en ? "inglés" : "español")}]");
        return string.Join("\n", partes);
    }

    /// <summary>Referencia = lo que devolvió la API (consulta, fecha filtrada, período, filtros, datos hasta, filas, generado).</summary>
    static JsonNode? Referencia(string salida)
    {
        if (JsonNode.Parse(salida) is not JsonObject n) return null;
        var r = new JsonObject();
        foreach (var k in new[] { "consulta", "fuente", "columna_fecha", "periodo", "filtros", "alcance", "datos_hasta", "total_filas", "generado_en" })
            if (n[k] is JsonNode v) r[k] = v.DeepClone();
        return r;
    }

    static IEnumerable<string> AvisosDe(string salida) =>
        JsonNode.Parse(salida)?["nota"]?.GetValue<string>() is { } nota ? Aviso.Matches(nota).Select(m => m.Value.Trim()) : Enumerable.Empty<string>();

    /// <summary>
    /// El modelo cierra con «Referencias: …» (útil en el arnés de pruebas); en el panel se muestran las de la API, así que NINGÚN bloque de
    /// referencias puede quedar en la respuesta (ni al final, ni en medio, ni en línea con el texto) y el texto debe salir UNA sola vez.
    /// </summary>
    static readonly Regex BloqueReferencias = new(@"Referencias:\s*consulta=.*?(?:generado=\S+|(?=\n\s*\n)|\z)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex LineaReferencias = new(@"(?im)^[ \t]*Referencias:.*(?:\n|\z)", RegexOptions.Compiled);
    static string QuitarBloqueReferencias(string texto)
    {
        var t = BloqueReferencias.Replace(texto, " ");
        t = LineaReferencias.Replace(t, "");
        t = Regex.Replace(t, @"[ \t]+\n", "\n");
        t = Regex.Replace(t, @"\n{3,}", "\n\n").Trim();
        return QuitarRepeticion(t);
    }

    static string Plano(string s) => Regex.Replace(s, @"\s+", " ").Trim();

    /// <summary>Varios mensajes del modelo en una respuesta: se unen sin repetir los que ya están contenidos en otro (el más largo gana).</summary>
    static string UnirMensajes(List<string> mensajes)
    {
        var res = new List<string>();
        foreach (var m in mensajes.Select(QuitarBloqueReferencias).Where(m => m.Length > 0))
        {
            var pm = Plano(m);
            if (res.Any(r => Plano(r).Contains(pm, StringComparison.Ordinal))) continue;
            res.RemoveAll(r => pm.Contains(Plano(r), StringComparison.Ordinal));
            res.Add(m);
        }
        return string.Join("\n\n", res);
    }

    /// <summary>«Respuesta + respuesta»: si el texto (sin espacios repetidos) es el mismo bloque dos veces seguidas, se queda con uno.</summary>
    static string QuitarRepeticion(string texto)
    {
        var p = Plano(texto);
        if (p.Length < 20) return texto;
        for (var corte = p.Length / 2 - 2; corte <= p.Length / 2 + 2; corte++)
            if (corte > 0 && corte < p.Length && p[..corte].Trim() == p[corte..].Trim())
            {
                // Se corta el original por el punto equivalente: el primer bloque ocupa aproximadamente la mitad del texto.
                var mitad = texto.Length / 2;
                for (var d = 0; d < texto.Length / 2; d++)
                    foreach (var i in new[] { mitad - d, mitad + d })
                        if (i > 0 && i < texto.Length && Plano(texto[..i]) == p[..corte].Trim()) return texto[..i].Trim();
                return p[..corte].Trim();
            }
        return texto;
    }

    // Verificador anti-invención (mismo criterio que probar_agente_local.py): toda cifra debe salir de lo que devolvió la API.
    static string Norm(string t) { var d = new string(t.Where(char.IsDigit).ToArray()).TrimStart('0'); return d.Length == 0 ? "0" : d; }
    static HashSet<string> Cifras(string t) =>
        Regex.Matches(FechaIso.Replace(t, " "), @"\d+(?:[.,]\d+)*").Select(m => Norm(m.Value)).ToHashSet();
    static HashSet<string> ComponentesFecha(string t) =>
        FechaIso.Matches(t).SelectMany(m => new[] { m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value }).Select(Norm).ToHashSet();

    static readonly Regex DiaDeMes = new(@"\b\d{1,2}\s+de\s+(enero|febrero|marzo|abril|mayo|junio|julio|agosto|septiembre|setiembre|octubre|noviembre|diciembre)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    readonly ConcurrentDictionary<string, HashSet<string>> _cifrasPrevias = new();

    static HashSet<string> Acumular(HashSet<string>? previas, string pregunta, List<string> salidas, List<string> argumentos)
    {
        var s = previas is null ? new HashSet<string>() : new HashSet<string>(previas);
        s.UnionWith(Cifras(pregunta));
        foreach (var x in salidas.Concat(argumentos)) { s.UnionWith(Cifras(x)); s.UnionWith(ComponentesFecha(x)); }
        return s;
    }

    static List<string> Verificar(string pregunta, string contexto, string respuesta, List<string> salidas, List<string> argumentos, string hoy, HashSet<string>? previas = null)
    {
        var cuerpo = Regex.Replace(respuesta, @"(?m)^\s*\d+[.)]\s+", "");     // numeración de listas
        // Sin consultas, el modelo solo interpreta fechas («lo corrido de 2026» = 1 de enero a hoy): esos «N de mes» no son datos.
        if (salidas.Count == 0) cuerpo = DiaDeMes.Replace(cuerpo, " ");
        var baseCifras = Cifras(pregunta + " " + contexto + " " + hoy);
        baseCifras.UnionWith(ComponentesFecha(pregunta + " " + contexto + " " + hoy));
        if (previas is not null) baseCifras.UnionWith(previas);
        foreach (var s in salidas.Concat(argumentos)) { baseCifras.UnionWith(Cifras(s)); baseCifras.UnionWith(ComponentesFecha(s)); }
        var extra = Cifras(cuerpo).Except(baseCifras).OrderBy(x => x.Length).ThenBy(x => x, StringComparer.Ordinal).Take(8).ToList();
        if (extra.Count == 0) return new();
        return new() { salidas.Count == 0
            ? "La respuesta trae cifras pero no se ejecutó ninguna consulta: " + string.Join(", ", extra)
            : "Hay cifras en la respuesta que no salen de las consultas (posible cálculo del asistente): " + string.Join(", ", extra) };
    }
}

/// <summary>Credencial ficticia SOLO para pruebas locales contra foundry_falso.py (Asistente:Credencial=prueba; no existe en Production).</summary>
sealed class CredencialDePrueba : TokenCredential
{
    public override AccessToken GetToken(TokenRequestContext c, CancellationToken ct) => new("token-de-prueba", DateTimeOffset.UtcNow.AddHours(1));
    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext c, CancellationToken ct) => new(GetToken(c, ct));
}
