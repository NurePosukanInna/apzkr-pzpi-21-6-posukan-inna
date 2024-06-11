using InventoryAPI.DTOs;
using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductDTO productDTO)
        {
            try
            {
                var product = await _productService.AddProduct(productDTO);
                return product;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error while adding product: {ex.Message}");
            }
        }

        [HttpGet("{storeId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByStoreId(int storeId)
        {
            try
            {
                var products = await _productService.GetProductsByStoreId(storeId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error while retrieving products: {ex.Message}");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var result = await _productService.DeleteProduct(productId);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error while deleting product: {ex.Message}");
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, ProductDTO productDTO)
        {
            try
            {
                var result = await _productService.UpdateProduct(productId, productDTO);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error while updating product: {ex.Message}");
            }
        }

    }
}
