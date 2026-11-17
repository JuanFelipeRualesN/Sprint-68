using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace DataAnalysisAPI.Utils
{
    /// <summary>
    /// Utilities for detecting the client IP address in ASP.NET Core.
    /// </summary>
    public static class IPAddressDetector
    {
        /// <summary>
        /// Gets the client IP address, optionally honoring forwarded headers
        /// (X-Forwarded-For / Forwarded) when behind a trusted reverse proxy.
        /// </summary>
        /// <param name="context">The current HttpContext.</param>
        /// <param name="useForwardedHeaders">
        /// If true, tries X-Forwarded-For / Forwarded first. Requires proper proxy config.
        /// </param>
        public static string? GetClientIpAddress(this HttpContext context, bool useForwardedHeaders = true)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            if (useForwardedHeaders)
            {
                // 1) X-Forwarded-For: comma-separated list, client is the first
                var xff = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(xff))
                {
                    var first = xff.Split(',').Select(s => s.Trim()).FirstOrDefault();
                    if (TryParseIp(first, out var ipFromXff))
                        return ipFromXff;
                }

                // 2) RFC 7239 'Forwarded' header: e.g., for=203.0.113.60 or for="[2001:db8::1]"
                var fwd = context.Request.Headers["Forwarded"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(fwd))
                {
                    var match = Regex.Match(fwd, @"for=(?<ip>[^;,\s]+)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        var token = match.Groups["ip"].Value.Trim('"').Trim('[', ']');
                        if (TryParseIp(token, out var ipFromForwarded))
                            return ipFromForwarded;
                    }
                }

                // (Optional) Common CDN/vendor headers
                var cf = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
                if (TryParseIp(cf, out var ipFromCf))
                    return ipFromCf;

                var tci = context.Request.Headers["True-Client-IP"].FirstOrDefault();
                if (TryParseIp(tci, out var ipFromTrueClient))
                    return ipFromTrueClient;
            }

            // Fallback: direct connection IP
            return context.Connection.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// Overload for convenience when you have HttpRequest.
        /// </summary>
        public static string? GetClientIpAddress(this HttpRequest request, bool useForwardedHeaders = true) =>
            request?.HttpContext.GetClientIpAddress(useForwardedHeaders);

        private static bool TryParseIp(string? value, out string ipString)
        {
            ipString = default!;
            if (string.IsNullOrWhiteSpace(value)) return false;

            // Strip port if present (e.g., "203.0.113.60:12345")
            var candidate = value;
            var colonIdx = candidate.LastIndexOf(':');
            if (colonIdx > -1 && candidate.Count(c => c == ':') == 1) // likely IPv4:port
                candidate = candidate.Substring(0, colonIdx);

            // IPv6 may appear like "[2001:db8::1]"
            candidate = candidate.Trim('[', ']');

            if (IPAddress.TryParse(candidate, out var ip))
            {
                ipString = ip.ToString();
                return true;
            }

            return false;
        }
    }
}
