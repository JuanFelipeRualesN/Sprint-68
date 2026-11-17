using System.Net.Http;

namespace MessagesAPI.CustomClasses
{
    /// <summary>
    /// To handle the IP address detection.
    /// </summary>
    public static class IPAddressDetector
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        /// <summary>
        /// Get the client IP Address. This class is from 
        /// https://www.strathweb.com/2013/05/retrieving-the-clients-ip-address-in-asp-net-web-/
        /// It basically takes the IP address of hosted or self hosted, depending on what applies 
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return null;
        }
    }
}