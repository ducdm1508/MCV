using Microsoft.AspNetCore.Mvc;
using UnitOfWork.DTOs;
using UnitOfWork.Services;

namespace UnitOfWork.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            await _productService.AddProduct(dto);
            return Ok("Product created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            await _productService.UpdateProduct(dto);
            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok("Product deleted successfully.");
        }
    }
}
