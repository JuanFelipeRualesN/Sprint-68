using System.Collections.Concurrent;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EyaInsightsApi;

// REQ-3741 · Fase 4 · A1 — Autenticación y autorización (DISENO_AUTH.md, opción B′).
// La API no emite ni valida tokens propios: lee el header `Token` de la plataforma y lo valida contra UsersAPI
// (POST {UsersApiUrl}/usersapi/accounts/get_user_by_token con {Token, IpAddress, PermissionName}), igual que DataAnalysisAPI.
// De la respuesta salen UserId / RoleId / OreId, y de ahí el ALCANCE de datos. Falla cerrada: ante la duda, no se entrega nada.

/// <summary>
/// Qué datos puede ver quien llama. <see cref="Todo"/> = todas las escuelas. <paramref name="OreId"/> = solo las escuelas de ese ORE
/// (admin-ore). <paramref name="EscuelaId"/> = solo esa escuela (director). Ampliación de A1 al contrato de DISENO_AUTH.md §3.2:
/// el segundo campo es opcional, así que <c>new Alcance(ore)</c> y <c>Alcance.Todo</c> siguen valiendo.
/// </summary>
public sealed record Alcance(int? OreId, int? EscuelaId = null)
{
    public static readonly Alcance Todo = new((int?)null);
    /// <summary>Alcance que no coincide con ninguna escuela (para rutas sin datos: /salud, preflight).</summary>
    public static readonly Alcance Nada = new(-1, -1);
    public bool EsTodo => OreId is null && EscuelaId is null;
}

/// <param name="Origen">"plataforma" (Token validado en UsersAPI) | "servicio" (x-api-key) | "local" (sin validación, solo desarrollo).</param>
public sealed record Usuario(string Id, string Rol, Alcance Alcance, string Origen);

public enum EstadoAuth { Ok, NoAutorizado, SinPermiso, NoDisponible }

public sealed class AuthOpciones
{
    public string Modo { get; init; } = "local";
    public string UsersApiUrl { get; init; } = "";
    public string Permiso { get; init; } = "embed-powerbi-info";
    public int[] RolesTodo { get; init; } = [2];       // admin
    public int[] RolesOre { get; init; } = [8];        // admin-ore
    public int[] RolesEscuela { get; init; } = [6];    // director
    public int CacheSegundos { get; init; } = 60;
    public int CacheNegativaSegundos { get; init; } = 10;
    public int TimeoutSegundos { get; init; } = 5;
    public string ApiKey { get; init; } = "";          // EYA_API_KEY: identidad de servicio (opcional)

    public bool EsPlataforma => Modo == "plataforma";

    public static AuthOpciones Leer(IConfiguration c)
    {
        int[] Roles(string clave, string defecto) =>
            (c[clave] ?? defecto).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
        int Num(string clave, int defecto) => int.TryParse(c[clave], NumberStyles.None, CultureInfo.InvariantCulture, out var v) ? v : defecto;
        return new AuthOpciones
        {
            Modo = (c["Auth:Modo"] ?? "local").Trim().ToLowerInvariant(),
            UsersApiUrl = (c["Auth:UsersApiUrl"] ?? "").Trim().TrimEnd('/'),
            Permiso = string.IsNullOrWhiteSpace(c["Auth:Permiso"]) ? "embed-powerbi-info" : c["Auth:Permiso"]!.Trim(),
            RolesTodo = Roles("Auth:RolesTodo", "2"),
            RolesOre = Roles("Auth:RolesOre", "8"),
            RolesEscuela = Roles("Auth:RolesEscuela", "6"),
            CacheSegundos = Num("Auth:CacheSegundos", 60),
            ApiKey = c["EYA_API_KEY"] ?? "",
        };
    }

    /// <summary>
    /// Comprobaciones de arranque (A1 + A6). Lanza <see cref="InvalidOperationException"/> con un mensaje claro si la configuración es insegura:
    ///  · Production exige Auth:Modo=plataforma y Auth:UsersApiUrl (URL absoluta); no hay valor por defecto a propósito;
    ///  · Auth:Modo=local solo se permite si la API escucha en loopback (no se puede dejar abierta a la red).
    /// </summary>
    public void ValidarArranque(IConfiguration cfg, bool produccion)
    {
        if (Modo is not ("local" or "plataforma"))
            throw new InvalidOperationException($"Auth:Modo = «{Modo}» no es válido: usa «local» (solo desarrollo) o «plataforma».");
        if (produccion && !EsPlataforma)
            throw new InvalidOperationException("En Production Auth:Modo debe ser «plataforma» (variable Auth__Modo). El modo «local» no valida a nadie y no se permite desplegado.");
        if (EsPlataforma)
        {
            if (string.IsNullOrWhiteSpace(UsersApiUrl))
                throw new InvalidOperationException("Auth:Modo=plataforma requiere Auth:UsersApiUrl (variable Auth__UsersApiUrl), la URL base de UsersAPI del ambiente, p. ej. https://users-api.ejemplo.invalid .");
            if (!Uri.TryCreate(UsersApiUrl, UriKind.Absolute, out var u) || u.Scheme is not ("http" or "https"))
                throw new InvalidOperationException("Auth:UsersApiUrl debe ser una URL absoluta http(s), p. ej. https://users-api.ejemplo.invalid .");
            if (produccion && u.Scheme != "https")
                Console.WriteLine("ADVERTENCIA: Auth:UsersApiUrl no usa https; el header Token viajaría sin cifrar.");
        }
        else
        {
            var urls = cfg["urls"] ?? cfg["ASPNETCORE_URLS"] ?? cfg["DOTNET_URLS"] ?? "";
            foreach (var t in urls.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var sinEsquema = t.Contains("://") ? t[(t.IndexOf("://", StringComparison.Ordinal) + 3)..] : t;
                var host = sinEsquema.StartsWith('[') ? sinEsquema[..(sinEsquema.IndexOf(']') + 1)] : sinEsquema.Split(':', '/')[0];
                if (host is not ("localhost" or "127.0.0.1" or "[::1]"))
                    throw new InvalidOperationException($"Auth:Modo=local no valida a nadie, así que la API solo puede escuchar en loopback (localhost/127.0.0.1); recibió «{t}». Usa Auth__Modo=plataforma.");
            }
        }
    }
}

/// <summary>Valida un Token de la plataforma contra UsersAPI, con caché corta y una sola llamada por token en vuelo.</summary>
public sealed class ValidadorSesion
{
    record Entrada(Task<(EstadoAuth Estado, Usuario? Usuario)> Tarea, DateTime Expira);

    readonly AuthOpciones _o;
    readonly HttpClient _http;
    readonly Func<string, int?> _escuelaDeUsuario;
    readonly ConcurrentDictionary<string, Entrada> _cache = new();

    public ValidadorSesion(AuthOpciones opciones, HttpClient http, Func<string, int?> escuelaDeUsuario)
    {
        _o = opciones; _http = http; _escuelaDeUsuario = escuelaDeUsuario;
        _http.Timeout = TimeSpan.FromSeconds(opciones.TimeoutSegundos);
    }

    /// <summary>SHA-256 truncado: para la caché y los logs. El token nunca se guarda ni se escribe.</summary>
    public static string Huella(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)))[..16];

    public async Task<(EstadoAuth Estado, Usuario? Usuario)> ValidarAsync(string token, string ip, CancellationToken ct)
    {
        var clave = Huella(token) + "|" + _o.Permiso;
        var ahora = DateTime.UtcNow;
        if (_cache.TryGetValue(clave, out var e) && e.Expira > ahora) return await e.Tarea;
        if (_cache.Count > 2000) foreach (var k in _cache.Where(kv => kv.Value.Expira <= ahora).Select(kv => kv.Key).ToList()) _cache.TryRemove(k, out _);
        // La llamada a UsersAPI no se cancela con la petición que la inició: otras peticiones con el mismo token esperan el mismo resultado.
        var tarea = ConsultarAsync(token, ip);
        var nueva = new Entrada(tarea, ahora.AddYears(1));          // mientras está en vuelo, otras peticiones la comparten
        _cache[clave] = nueva;
        var r = await tarea;
        if (r.Estado == EstadoAuth.NoDisponible) _cache.TryRemove(new KeyValuePair<string, Entrada>(clave, nueva));   // los fallos de red no se cachean
        else _cache[clave] = new Entrada(tarea, DateTime.UtcNow.AddSeconds(r.Estado == EstadoAuth.Ok ? _o.CacheSegundos : _o.CacheNegativaSegundos));
        return r;
    }

    static string? Prop(JsonElement raiz, string nombre)
    {
        foreach (var p in raiz.EnumerateObject())
            if (string.Equals(p.Name, nombre, StringComparison.OrdinalIgnoreCase))
                return p.Value.ValueKind switch { JsonValueKind.String => p.Value.GetString(), JsonValueKind.Number => p.Value.GetRawText(), JsonValueKind.True => "true", JsonValueKind.False => "false", _ => null };
        return null;
    }

    async Task<(EstadoAuth Estado, Usuario? Usuario)> ConsultarAsync(string token, string ip)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, _o.UsersApiUrl + "/usersapi/accounts/get_user_by_token")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { Token = token, IpAddress = ip, PermissionName = _o.Permiso }), Encoding.UTF8, "application/json"),
            };
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            req.Headers.Add("Token", token);
            using var resp = await _http.SendAsync(req);
            if (resp.StatusCode is System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden) return (EstadoAuth.NoAutorizado, null);
            if (!resp.IsSuccessStatusCode) return (EstadoAuth.NoDisponible, null);     // 400/5xx de UsersAPI: no se pudo validar → falla cerrada
            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            if (doc.RootElement.ValueKind != JsonValueKind.Object) return (EstadoAuth.NoDisponible, null);
            var r = doc.RootElement;
            if (!string.Equals(Prop(r, "SessionValid"), "true", StringComparison.OrdinalIgnoreCase)) return (EstadoAuth.NoAutorizado, null);
            var id = Prop(r, "UserId");
            if (string.IsNullOrWhiteSpace(id) || !int.TryParse(Prop(r, "RoleId"), NumberStyles.None, CultureInfo.InvariantCulture, out var rol))
                return (EstadoAuth.SinPermiso, null);
            return Autorizar(id, rol, Prop(r, "OreId"));
        }
        catch (Exception e)      // cualquier fallo = no se pudo validar (falla cerrada); no debe quedar una tarea fallida en la caché
        {
            Console.WriteLine($"Auth: no se pudo validar la sesión ({e.GetType().Name}).");
            return (EstadoAuth.NoDisponible, null);
        }
    }

    /// <summary>Rol → alcance (DISENO_AUTH.md §3.1). Cualquier rol no listado → sin permiso. Un rol de alcance parcial SIN su ORE/escuela → sin permiso (nunca «todo»).</summary>
    (EstadoAuth, Usuario?) Autorizar(string id, int rol, string? ore)
    {
        if (_o.RolesTodo.Contains(rol)) return (EstadoAuth.Ok, new Usuario(id, "admin", Alcance.Todo, "plataforma"));
        if (_o.RolesOre.Contains(rol))
            return int.TryParse(ore, NumberStyles.None, CultureInfo.InvariantCulture, out var o)
                ? (EstadoAuth.Ok, new Usuario(id, "admin-ore", new Alcance(o), "plataforma"))
                : (EstadoAuth.SinPermiso, null);
        if (_o.RolesEscuela.Contains(rol))
        {
            // get_user_by_token no devuelve SchoolId: la escuela del director se lee de gs_data_analysis.user (SUPUESTO: UserId de UsersAPI = user.user_id).
            var escuela = _escuelaDeUsuario(id);
            return escuela is { } s ? (EstadoAuth.Ok, new Usuario(id, "director", new Alcance(null, s), "plataforma")) : (EstadoAuth.SinPermiso, null);
        }
        return (EstadoAuth.SinPermiso, null);
    }
}

public static class Auth
{
    public const string ClaveItem = "eya.usuario";

    /// <summary>El usuario autenticado de esta petición (lo deja el middleware; nunca null tras él).</summary>
    public static Usuario Usuario(HttpContext ctx) => (Usuario)ctx.Items[ClaveItem]!;

    static string Ip(HttpContext c)
    {
        var xff = c.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(xff)) return xff.Split(',')[0].Trim();
        return c.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
    }

    static bool ClaveIgual(string recibida, string esperada) =>
        CryptographicOperations.FixedTimeEquals(SHA256.HashData(Encoding.UTF8.GetBytes(recibida)), SHA256.HashData(Encoding.UTF8.GetBytes(esperada)));

    static Task Rechazar(HttpContext ctx, int estado, string error, string mensaje)
    {
        ctx.Response.StatusCode = estado;
        return ctx.Response.WriteAsJsonAsync(new { error, mensaje });
    }

    /// <summary>
    /// Middleware de autenticación. Va DESPUÉS de CORS y antes de las rutas. Sin auth: OPTIONS (preflight) y /salud.
    ///  · local: sin validación (usuario «local», todo). Si existe EYA_API_KEY se sigue exigiendo x-api-key, como antes.
    ///  · plataforma: header Token → UsersAPI; sin Token, x-api-key = EYA_API_KEY → identidad de servicio (todo); si no, 401.
    /// </summary>
    public static void UsarAutenticacion(this WebApplication app, AuthOpciones o, ValidadorSesion? validador)
    {
        var anonimo = new Usuario("anonimo", "ninguno", Alcance.Nada, "ninguno");
        app.Use(async (ctx, next) =>
        {
            if (HttpMethods.IsOptions(ctx.Request.Method) || ctx.Request.Path == "/salud")
            {
                ctx.Items[ClaveItem] = anonimo;
                await next();
                return;
            }
            var apiKey = ctx.Request.Headers["x-api-key"].FirstOrDefault();
            if (!o.EsPlataforma)
            {
                if (!string.IsNullOrEmpty(o.ApiKey) && (apiKey is null || !ClaveIgual(apiKey, o.ApiKey)))
                {
                    await Rechazar(ctx, 401, "no_autorizado", "Falta o es incorrecto x-api-key.");
                    return;
                }
                ctx.Items[ClaveItem] = new Usuario("local", "admin", Alcance.Todo, "local");
                await next();
                return;
            }

            var token = ctx.Request.Headers["Token"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var (estado, usuario) = await validador!.ValidarAsync(token.Trim(), Ip(ctx), ctx.RequestAborted);
                switch (estado)
                {
                    case EstadoAuth.Ok: ctx.Items[ClaveItem] = usuario!; await next(); return;
                    case EstadoAuth.NoAutorizado: await Rechazar(ctx, 401, "no_autorizado", "Sesión no válida o vencida."); return;
                    case EstadoAuth.SinPermiso: await Rechazar(ctx, 403, "sin_permiso", "Tu rol no tiene acceso a estos datos."); return;
                    default: await Rechazar(ctx, 503, "autenticacion_no_disponible", "No se pudo validar la sesión; intenta de nuevo."); return;
                }
            }
            if (!string.IsNullOrEmpty(o.ApiKey) && apiKey is not null && ClaveIgual(apiKey, o.ApiKey))
            {
                ctx.Items[ClaveItem] = new Usuario("servicio", "servicio", Alcance.Todo, "servicio");
                await next();
                return;
            }
            await Rechazar(ctx, 401, "no_autorizado", "Falta el header Token (sesión de la plataforma).");
        });
    }

    /// <summary>Headers que el asistente reenvía en las llamadas internas (loopback) para conservar la identidad y el alcance de quien pregunta.</summary>
    public static List<(string Nombre, string Valor)> HeadersParaReenviar(HttpContext ctx)
    {
        var lista = new List<(string, string)>();
        if (Usuario(ctx).Origen == "servicio" && ctx.Request.Headers["x-api-key"].FirstOrDefault() is { Length: > 0 } k) lista.Add(("x-api-key", k));
        else if (ctx.Request.Headers["Token"].FirstOrDefault() is { Length: > 0 } t) lista.Add(("Token", t));
        else if (ctx.Request.Headers["x-api-key"].FirstOrDefault() is { Length: > 0 } k2 && Usuario(ctx).Origen == "local") lista.Add(("x-api-key", k2));
        if (ctx.Request.Headers["Accept-Language"].FirstOrDefault() is { Length: > 0 } l) lista.Add(("Accept-Language", l));
        return lista;
    }
}
