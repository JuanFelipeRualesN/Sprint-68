using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SubscriptionsAPI.CustomClasses
{
    /// <summary>
    /// ASP.NET Core middleware that protects Swagger endpoints behind a query/cookie key (?docKey=...).
    /// - Configure the expected key in "AppSettings:doc_key" (or "Swagger:DocKey" fallback).
    /// - First access with ?docKey=KEY sets a secure cookie for subsequent swagger asset requests.
    /// </summary>
    public sealed class SwaggerAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        private const string QueryKeyName = "docKey";
        private const string CookieName = "docKey";

        public SwaggerAccessMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsSwaggerRequest(context))
            {
                await _next(context);
                return;
            }

            var expectedKey =
                _config["AppSettings:doc_key"] ??
                _config["Swagger:DocKey"];

            if (string.IsNullOrWhiteSpace(expectedKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized (swagger key not configured).");
                return;
            }

            // 1) Query ?docKey=...
            if (context.Request.Query.TryGetValue(QueryKeyName, out var queryVals) &&
                string.Equals(queryVals.ToString(), expectedKey, StringComparison.Ordinal))
            {
                // Persist to cookie for further swagger asset requests
                context.Response.Cookies.Append(CookieName, expectedKey, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = context.Request.IsHttps,
                    SameSite = SameSiteMode.Strict,
                    Path = "/"
                });

                await _next(context);
                return;
            }

            // 2) Cookie
            if (context.Request.Cookies.TryGetValue(CookieName, out var cookieVal) &&
                string.Equals(cookieVal, expectedKey, StringComparison.Ordinal))
            {
                await _next(context);
                return;
            }

            // 3) Deny
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized.");
        }

        private static bool IsSwaggerRequest(HttpContext ctx)
        {
            var path = ctx.Request.Path.Value ?? string.Empty;

            // Standard swagger endpoints
            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
                return true;

            // Back-compat with original logic that checked for substring "swagg"
            return path.Contains("swagg", StringComparison.OrdinalIgnoreCase);
        }
    }

    public static class SwaggerAccessMiddlewareExtensions
    {
        /// <summary>
        /// Registers the Swagger access gate. Place BEFORE app.UseSwagger()/UseSwaggerUI().
        /// </summary>
        public static IApplicationBuilder UseSwaggerAccessGate(this IApplicationBuilder app)
            => app.UseMiddleware<SwaggerAccessMiddleware>();
    }
}

/* Program.cs (minimal hosting, .NET 8)
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Protect swagger first
app.UseSwaggerAccessGate();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();
*/
