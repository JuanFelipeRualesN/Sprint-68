using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace SubscriptionsAPI.CustomClasses
{
    /// <summary>
    /// To handle the IP Address detection.
    /// </summary>
    public static class IPAddressDetector
    {
        // <summary>
        /// Gets the best-effort client IP address, honoring common proxy headers.
        /// Prefer configuring ForwardedHeadersMiddleware; this method is a fallback.
        /// </summary>
        public static string? GetClientIpAddress(HttpContext httpContext)
        {
            if (httpContext == null) return null;

            var headers = httpContext.Request.Headers;

            // Check common proxy headers
            foreach (var headerName in new[] { "CF-Connecting-IP", "X-Forwarded-For", "X-Real-IP", "X-Client-IP", "Forwarded" })
            {
                if (!headers.TryGetValue(headerName, out var values)) continue;

                var raw = values.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(raw)) continue;

                if (headerName.Equals("Forwarded", StringComparison.OrdinalIgnoreCase))
                {
                    // RFC 7239: Forwarded: for=192.0.2.60;proto=http;by=203.0.113.43
                    var m = Regex.Match(raw, @"for=\s*(?<ip>[^;,\s]+)", RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        var ipStr = m.Groups["ip"].Value.Trim('"').Trim('[', ']');
                        if (IPAddress.TryParse(ipStr, out var parsed))
                            return Normalize(parsed);
                    }
                }
                else
                {
                    // X-Forwarded-For may contain a list: client, proxy1, proxy2
                    var first = raw.Split(',').Select(s => s.Trim()).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
                    if (!string.IsNullOrWhiteSpace(first) && IPAddress.TryParse(first, out var parsed))
                        return Normalize(parsed);
                }
            }

            // Fallback to connection remote IP
            var remote = httpContext.Connection.RemoteIpAddress;
            return remote != null ? Normalize(remote) : null;

            static string Normalize(IPAddress ip)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    ip = ip.MapToIPv4();
                return ip.ToString();
            }
        }
    }
}