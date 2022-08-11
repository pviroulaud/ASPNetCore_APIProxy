using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Proxy
{
    public partial class EnumCodigos
    {
        public enum Codigo : ushort
        {
            //100 Respuestas informativas
            DocumentoDadoDeBaja = 101,
            FinalizoProcesoDeFirma = 102,
            NoHayFirmantePendiente = 103,
            FlujoFirmasFinalizado = 104,
            TokenInvalidado = 105,


            //200 Peticiones correctas
            OK = 200,
            SinContenido = 204,
            TokenValido = 205,
            //300 Redirecciones


            //400 Errores del cliente
            ParametrosIncorrectos = 400,
            NecesitaAutorizacion = 401,
            SinPermisos = 403,
            ServicioNoEncontrado = 404,
            EntidadNoEncontrada = 406,  //414,
            LicenciaAgotada = 407,


            // 500 Errores de servidor
            RecursoNoEncontrado = 501,
            BaseDeDatosNoEncontrada = 502,
            OperacionFallida = 503,


            // 510 Errores de servidor - Recuperar Contraseña
            UsuarioBloqueado = 511,
            CodigoIncorrecto = 512,
            CodigoValidacionVencido = 513,
            ContraseniaConfirmacion = 514

        }
    }
    public partial class MensajeResult
    {
        public string errorEnLogicaDeNegocio
        {
            get
            {
                if (codigo < 399)
                {
                    return "NO";
                }
                else
                {
                    return "SI";
                }

            }
            set
            {
                var a = value;
            }
        }
        public int codigo { get; set; }
        public string mensaje
        { //get; set;
            get
            {
                return Enum.GetName(typeof(EnumCodigos.Codigo), codigo);
            }
            set { }
        }
        public string entidad { get; set; }
        public string detalle { get; set; }
    }


    public static class RouteProxy
    {
        public static HttpRequestMessage CreateProxyHttpRequest(this HttpContext context, string uriString, bool isAuthorizeProxied)
        {
            var uri = new Uri(uriString);
            var request = context.Request;
            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;

            //if (!HttpMethods.IsGet(requestMethod) &&
            //    !HttpMethods.IsHead(requestMethod) &&
            //    !HttpMethods.IsDelete(requestMethod) &&
            //    !HttpMethods.IsTrace(requestMethod))
            //{

            //    request.EnableBuffering();
            //    request.Body.Seek(0, SeekOrigin.Begin);
            //    var streamContent = new StreamContent(request.Body);
            //    requestMessage.Content = streamContent;
            //}
            FormUrlEncodedContent formContent=null;
            if (request.ContentType!=null)
            {
                if (request.Form.Count > 0)
                {
                    KeyValuePair<string, string>[] campoValor_FormData = new KeyValuePair<string, string>[request.Form.Count];
                    for (var n = 0; n < campoValor_FormData.Length; n++)
                    {
                        campoValor_FormData[n] = new KeyValuePair<string, string>(request.Form.Keys.ElementAt(n), request.Form[request.Form.Keys.ElementAt(n)]);
                    }
                    formContent = new FormUrlEncodedContent(campoValor_FormData);

                }
            }
            

            foreach (var header in request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
            
            if (formContent!=null)
            {
                requestMessage.Content = formContent;
            }
            
            if (!isAuthorizeProxied)
            {
                requestMessage.Headers.Remove("Authorization");
            }

            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = new HttpMethod(request.Method);

            return requestMessage;
        }

        public static Task<HttpResponseMessage> SendProxyHttpRequest(this HttpContext context, string proxiedAddress, bool isAuthorizeProxied)
        {
            var proxiedRequest = context.CreateProxyHttpRequest(proxiedAddress, isAuthorizeProxied);
            
            return new HttpClient().SendAsync(proxiedRequest);//, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
        }

        public static async Task CopyProxyHttpResponse(this HttpContext context, HttpResponseMessage responseMessage)
        {
            var response = context.Response;

            response.StatusCode = (int)responseMessage.StatusCode;

            foreach (var header in responseMessage.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            response.Headers.Remove("transfer-encoding");

            using (var responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                await responseStream.CopyToAsync(response.Body, 81920, context.RequestAborted).ConfigureAwait(false);
            }
        }

    }
}
