using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private ServicesConfiguration Services { get; }

    //constructor
    public OrdersController(IConfiguration configuration)
        {
            
            Services = new ServicesConfiguration();
            configuration.GetSection("Services").Bind(Services);

        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {

            var httpClient = new HttpClient();
            //voy a tomar esa ruta de la propiedad services
            httpClient.BaseAddress = new Uri(Services.ProductsSvc);

            //llamamos al API de productos desde orders
            //recibimos un json
           // var json = await httpClient.GetStringAsync("/Products"); // me traigo to todos los products
            var json = await httpClient.GetStringAsync(Services.ProductsSvc + "/products");
            var products = JsonConvert.DeserializeObject<JArray>(json);

            //traemos los productos y lo pnemos en un arreglo
            object[] lines = products.Select(line => (object)line).ToArray();

            object order1 = new { Lines = lines, customer = new object(),DateTime.Now };


            return Ok( new
                object[] { order1}
                
                );
        }

   


        private class ServicesConfiguration
        {

            public string CustomersSvc { get; set; }
            public string ProductsSvc { get; set; }
  
        }
    }
}
