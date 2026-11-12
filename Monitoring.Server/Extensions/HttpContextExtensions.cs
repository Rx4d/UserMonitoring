using System.Net;

namespace Monitoring.Server;

public static class HttpContextExtensions
{
    extension(HttpContext context)
    {
        public string? GetRemoteOrLocalIp()
        {
            var ip = context.Connection.RemoteIpAddress ?? context.Connection.LocalIpAddress;
            if (ip is null) return null;

            if (IPAddress.IsLoopback(ip))
                return IPAddress.Loopback.ToString();

            if (ip.IsIPv4MappedToIPv6)
                return ip.MapToIPv4().ToString();

            return ip.ToString();
        }
    }
}