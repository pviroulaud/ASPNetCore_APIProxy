using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace API_externa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("obtenerBadReq")]
        [Authorize]
        public IActionResult obtenerBadReq()
        {

            return BadRequest();
        }

        [HttpGet("obtener")]
        public async Task obtener()
        {

            var proxiedResponse = await Proxy.RouteProxy.SendProxyHttpRequest(this.HttpContext,"https://localhost:44391/WeatherForecast/obtener", false);

            

            await Proxy.RouteProxy.CopyProxyHttpResponse(this.HttpContext, proxiedResponse); 
        }

        [HttpGet("obtenerAuth")]
        [Authorize]
        public async Task obtenerAuth()
        {

            var proxiedResponse = await Proxy.RouteProxy.SendProxyHttpRequest(this.HttpContext, "https://localhost:44391/WeatherForecast/obtenerAuth", true);



            await Proxy.RouteProxy.CopyProxyHttpResponse(this.HttpContext, proxiedResponse);
        }
        [HttpGet("obtenerAuth2")]
        [Authorize]
        public async Task obtenerAuth2()
        {

            var proxiedResponse = await Proxy.RouteProxy.SendProxyHttpRequest(this.HttpContext, "https://localhost:44391/WeatherForecast/obtenerAuth2", true);



            await Proxy.RouteProxy.CopyProxyHttpResponse(this.HttpContext, proxiedResponse);
        }
        [HttpGet("obtenerAuth3")]
        [Authorize]
        public async Task obtenerAuth3()
        {

            var proxiedResponse = await Proxy.RouteProxy.SendProxyHttpRequest(this.HttpContext, "https://localhost:44391/WeatherForecast/obtenerAuth3", true);



            await Proxy.RouteProxy.CopyProxyHttpResponse(this.HttpContext, proxiedResponse);
        }
        [HttpPost("obtenerAuth4")]
        [Authorize]
        public async Task obtenerAuth4([FromForm] Proxy.test1DTO archivo,int id)
        {

            var proxiedResponse = await Proxy.RouteProxy.SendProxyHttpRequest(this.HttpContext, "https://localhost:44391/WeatherForecast/obtenerAuth4", true);

            if (id==1)
            {
                await Proxy.RouteProxy.CopyProxyHttpResponse(this.HttpContext, proxiedResponse);
                
            }

            else
            {
               
                ActionContext action = new ActionContext(this.HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
                
                BadRequest().ExecuteResult(action);
            }
            
        }
        [HttpPost("obtenerAuth5")]
        [Authorize]
        public async Task obtenerAuth5([FromForm] Proxy.test1DTO archivo,[Required] int id)
        {

            var proxiedResponse = await Proxy.RouteProxy.SendProxyHttpRequest(this.HttpContext, "https://localhost:44391/WeatherForecast/obtenerAuth5", true);

            if (id == 1)
            {
                 await Proxy.RouteProxy.CopyProxyHttpResponse(this.HttpContext, proxiedResponse);
                

            }

            else
            {

                ActionContext action =  new ActionContext(this.HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

               
                 await BadRequest().ExecuteResultAsync(action);
            }

        }
    }
}
