using InventoryAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Services
{
    public interface ICategoryService
    {
        Task<Category> AddCategory(Category category);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);
        Task<bool> DeleteCategory(int categoryId);

    }
}
