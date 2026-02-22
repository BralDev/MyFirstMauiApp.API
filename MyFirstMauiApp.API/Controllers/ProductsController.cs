using Microsoft.AspNetCore.Mvc;
using MyFirstMauiApp.API.Business.DTOs;
using MyFirstMauiApp.API.Business.Interfaces;

namespace MyFirstMauiApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Inyectamos el servicio de negocio
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products); // Retorna 200 OK con la lista
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { message = $"El producto con ID {id} no existe." });

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);

            // Retorna 201 Created y añade la cabecera 'Location' con la URL del nuevo producto
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/products
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
        {
            var result = await _productService.UpdateProductAsync(dto);

            if (result == null)
                return NotFound(new { message = "No se pudo actualizar. El producto no existe." });

            return Ok(result); // Gracias al OUTPUT, devolvemos el objeto actualizado fresco
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);

            if (!deleted)
                return NotFound(new { message = "No se pudo eliminar. El producto no existe." });

            return NoContent(); // 204 No Content: Estándar para eliminaciones exitosas
        }
    }
}