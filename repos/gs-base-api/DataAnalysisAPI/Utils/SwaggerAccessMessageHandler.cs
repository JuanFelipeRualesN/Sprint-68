using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DataAnalysisAPI.Utils
{
    /// <summary>
    /// Middleware para controlar acceso a Swagger mediante un docKey.
    /// Acepta ?docKey=... y, si es correcto, emite una cookie temporal.
    /// </summary>
    public sealed class SwaggerAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _docKey;
        private const string CookieName = "swagger_key_ok";

        public SwaggerAccessMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _docKey = config["AppSettings:doc_key"] ?? string.Empty;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!IsSwagger(context))
            {
                await _next(context);
                return;
            }

            var authorized = false;

            // 1) ¿Ya tiene cookie válida?
            if (context.Request.Cookies.TryGetValue(CookieName, out var cookie) && cookie == "1")
            {
                authorized = true;
            }

            // 2) ¿Trae docKey correcto en el query?
            if (!authorized)
            {
                var providedKey = context.Request.Query["docKey"].FirstOrDefault();
                if (!string.IsNullOrEmpty(providedKey) && TimeConstantEquals(providedKey, _docKey))
                {
                    authorized = true;

                    // Opción: recordar por 2 horas
                    context.Response.Cookies.Append(
                        CookieName, "1",
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,      // true si usas HTTPS (recomendado)
                            SameSite = SameSiteMode.Lax,
                            IsEssential = true,
                            Expires = DateTimeOffset.UtcNow.AddHours(2)
                        });
                }
            }

            if (!authorized)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }

        private static bool IsSwagger(HttpContext context)
        {
            // Mantiene la lógica del original: buscar "swagg" en la URL
            var path = context.Request.Path.Value ?? string.Empty;
            return path.IndexOf("swagg", StringComparison.OrdinalIgnoreCase) >= 0;
            // Alternativa más estricta: return context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase);
        }

        // Comparación en tiempo constante para evitar timing attacks
        private static bool TimeConstantEquals(string a, string b)
        {
            if (a is null || b is null) return false;
            uint diff = (uint)a.Length ^ (uint)b.Length;
            var len = Math.Min(a.Length, b.Length);
            for (int i = 0; i < len; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0 && a.Length == b.Length;
        }
    }

    public static class SwaggerAccessMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerAccessGate(this IApplicationBuilder app)
            => app.UseMiddleware<SwaggerAccessMiddleware>();
    }
}
