using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CustomersApi
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ApiKeyMiddleware> _logger;
        private const string API_KEY = "ApiKey";

        public ApiKeyMiddleware(RequestDelegate next, Microsoft.Extensions.Logging.ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;

            _logger = logger;

            


        }
        //este intercepta la llamadas
        public async Task Invoke(HttpContext httpContext)
        {
            
            _logger.LogInformation("Middleware en ejecucion");

            //aqui obtengo el emvabezado para revisar
            // si no se incluye la llave para este api (ou), no debo dejar
            if (!httpContext.Request.Headers.TryGetValue("API_KEY",out var key))
            {

                _logger.LogInformation("APi key no encontrada");

                //la llave se comparte y se actualiza y se manda otra vez a los clientes
                // le envio el mensaje al cliente de que no esta autorizado via codigo http
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                //uso objeto aninimos porque no tengo que esta=r creando clases por ahora
                string responseMessage = JsonConvert.SerializeObject(new { message = "Api key Invalid" });
                await httpContext.Response.WriteAsync(responseMessage);

                return;
            }

            //TODO :
            //1. generar una llave en incluirla en el fcomnfiguration
            //2.Validar que la llave recibida coincida con la llave  en el Configuration
            //3. Desde el cliente, incluir la llave correcta para procesar las perticiones
            // De lo contrario todos los llamados que se tengan sera 401 (unautorized)

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
