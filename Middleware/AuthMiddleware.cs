using System.Net;

namespace UHC_API.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;
        private readonly byte[][] _safeList;
        private readonly string _password;

        public AuthMiddleware(
            RequestDelegate next,
            ILogger<AuthMiddleware> logger, string password, string safeList)
        {
            var ips = safeList.Split(';');
            _safeList = new byte[ips.Length][];
            for (var i = 0; i < ips.Length; i++)
            {
                _safeList[i] = IPAddress.Parse(ips[i]).GetAddressBytes();
            }

            _password = password;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethod.Get.Method)
            {
                string? givenPassword = context.Request.Headers["AUTH"];
                Console.WriteLine(_password);
                

                var remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;
                foreach (var address in _safeList)
                {
                    if (address.SequenceEqual(bytes) || bytes.ToString().Contains(address.ToString()))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning(
                        "Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    return;
                }
                if (givenPassword == null || givenPassword != _password)
                {
                    _logger.LogWarning("Request was denied. Reason: Wrong password. Password given: {password}" , givenPassword);
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    
}