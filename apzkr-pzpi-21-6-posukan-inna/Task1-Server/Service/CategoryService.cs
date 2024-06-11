using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly InventoryContext _context;

        public CategoryService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding category: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving categories: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            try
            {
                return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving products by category: {ex.Message}");
            }
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return false;
                }

                var productsInCategory = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();

                foreach (var product in productsInCategory)
                {
                    product.CategoryId = null;
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting category: {ex.Message}");
            }
        }
    }
}
