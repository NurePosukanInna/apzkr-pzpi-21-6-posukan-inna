using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category category)
        {
            try
            {
                var addedCategory = await _categoryService.AddCategory(category);
                return Ok(addedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while adding category: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving categories: {ex.Message}");
            }
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _categoryService.GetProductsByCategory(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving products by category: {ex.Message}");
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(categoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting category: {ex.Message}");
            }
        }

    }
}
