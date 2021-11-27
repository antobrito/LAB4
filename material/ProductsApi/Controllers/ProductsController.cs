using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductsApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private AdventureWorksDbContext _dbContext;
        private ILogger<ProductsController> _logger;

        public ProductsController(
            ILogger<ProductsController> logger,
            AdventureWorksDbContext dbContext
        )
        {
            _dbContext = dbContext;
            _logger = logger;

            _logger.LogInformation("DI correcto");
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogInformation("Llamado del GET ejecutandose");

            // lambda
            return Ok(
                _dbContext.Products.Select(product => new {
                    Id = product.ProductId,
                    Name = product.Name,
                    ListPrice = product.ListPrice,
                })
            );
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
