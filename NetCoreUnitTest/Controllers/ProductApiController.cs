using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreUnitTest.Models;
using NetCoreUnitTest.Repository;

namespace NetCoreUnitTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductApiController(IRepository<Product> repository)
        {
           
            _repository = repository;
        }       

        // GET: api/ProductApi
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var product = await _repository.GetAll();
            return Ok(product);
        }

        // GET: api/ProductApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/ProductApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _repository.Update(product);
                      
            return NoContent();//api den hiç bir şey dömediğini haber verir.
        }

        // POST: api/ProductApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            await _repository.Create(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product); //dönülen response un headerında bulunucak bilgi.CreatedAction ile 201 durum kodu.
        }

        // DELETE: api/ProductApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _repository.Delete(product);

            return NoContent();
        }

        private async Task <bool> ProductExists(int id)
        {
            Product product = await _repository.GetById(id);

            if (product == null)
            {
                return false;
            }
            else
            {
               return true;
            }
          
        }
    }
}
