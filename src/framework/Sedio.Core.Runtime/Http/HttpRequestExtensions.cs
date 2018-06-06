using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Sedio.Core.Runtime.Http
{
    public static class HttpRequestExtensions
    {
        public static IPAddress GetClientIpAddress(this HttpRequest request,bool tryUseXForwardHeader = true)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            var forwardedFor = request.GetHeaderValue("X-Forwarded-For");
            
            if (tryUseXForwardHeader && forwardedFor != null)
            {
                ip = forwardedFor.TrimEnd(',')
                    .Split(',')
                    .AsEnumerable<string>()
                    .Select(s => s.Trim()).FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(ip) && request.HttpContext.Connection.RemoteIpAddress != null)
            {
                ip = request.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                var remoteAdd = request.GetHeaderValue("REMOTE_ADD");
                
                if (remoteAdd == null)
                {
                    throw new HttpRequestException("Unable to determine request ip");
                }
                else
                {
                    ip = remoteAdd;
                }
            }

            var result = IPAddress.Parse(ip);

            if (result.AddressFamily == AddressFamily.InterNetworkV6)
            {
                if (IPAddress.IsLoopback(result))
                {
                    return IPAddress.Loopback;
                }
            }

            return result;
        }

        public static string GetHeaderValue(this HttpRequest request, string headerName)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(headerName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(headerName));
            
            if (request.Headers.TryGetValue(headerName, out var result))
            {
                return result.ToString();
            }

            return null;
        }
    }
}