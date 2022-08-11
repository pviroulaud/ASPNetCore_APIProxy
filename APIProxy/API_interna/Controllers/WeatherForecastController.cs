using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_interna.Controllers
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

        [HttpGet("obtener")]
        public ActionResult<IEnumerable<Proxy.test1DTO>> obtener()
        {
            List<Proxy.test1DTO> ret = new List<Proxy.test1DTO>();
            ret.Add(new Proxy.test1DTO() { id = 1, valor = "v1" });
            ret.Add(new Proxy.test1DTO() { id = 2, valor = "v2" });
            ret.Add(new Proxy.test1DTO() { id = 3, valor = "v3" });

            return ret;
        }

        [HttpGet("obtenerAuth")]
        [Authorize]
        public ActionResult<IEnumerable<Proxy.test1DTO>> obtenerAuth()
        {
            List<Proxy.test1DTO> ret = new List<Proxy.test1DTO>();
            ret.Add(new Proxy.test1DTO() { id = 1, valor = "v1" });
            ret.Add(new Proxy.test1DTO() { id = 2, valor = "v2" });
            ret.Add(new Proxy.test1DTO() { id = 3, valor = "v3" });

            return ret;
        }
        [HttpGet("obtenerAuth2")]
        [Authorize]
        public ActionResult<IEnumerable<Proxy.test1DTO>> obtenerAuth2()
        {
            return BadRequest();
        }

        [HttpGet("obtenerAuth3")]
        [Authorize]
        public ActionResult<IEnumerable<Proxy.test1DTO>> obtenerAuth3()
        {
            Proxy.MensajeResult res = new Proxy.MensajeResult();
            res.codigo = 400;
            res.detalle = "test";

            return Accepted(res);
        }
        [HttpPost("obtenerAuth4")]
        [Authorize]
        public ActionResult<IEnumerable<Proxy.test1DTO>> obtenerAuth4([FromForm] Proxy.test1DTO archivo)
        {
            Proxy.MensajeResult res = new Proxy.MensajeResult();
            res.codigo = 200;
            res.detalle = archivo.valor;
            if (archivo.id==2)
            {
                return BadRequest();
            }
            else
            {
                if (archivo.id == 3)
                {
                    List<Proxy.test1DTO> ret = new List<Proxy.test1DTO>();
                    ret.Add(new Proxy.test1DTO() { id = 1, valor = "v1" });
                    ret.Add(new Proxy.test1DTO() { id = 2, valor = "v2" });
                    ret.Add(new Proxy.test1DTO() { id = 3, valor = "v3" });

                    return ret;
                }
                else
                {
                    return Accepted(res);
                }
            }
            
        }
        [HttpPost("obtenerAuth5")]
        [Authorize]
        public ActionResult<IEnumerable<Proxy.test1DTO>> obtenerAuth5([FromForm] Proxy.test1DTO archivo,[Required] int numero)
        {
            Proxy.MensajeResult res = new Proxy.MensajeResult();
            res.codigo = 200;
            res.detalle = archivo.valor;
            if (archivo.id == 2)
            {
                return BadRequest();
            }
            else
            {
                if (archivo.id == 3)
                {
                    List<Proxy.test1DTO> ret = new List<Proxy.test1DTO>();
                    ret.Add(new Proxy.test1DTO() { id = 1, valor = "v1" });
                    ret.Add(new Proxy.test1DTO() { id = 2, valor = "v2" });
                    ret.Add(new Proxy.test1DTO() { id = 3, valor = "v3" });

                    return ret;
                }
                else
                {
                    return Accepted(res);
                }
            }

        }
    }
}
