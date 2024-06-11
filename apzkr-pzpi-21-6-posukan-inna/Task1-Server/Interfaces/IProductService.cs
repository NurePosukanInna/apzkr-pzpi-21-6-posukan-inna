using InventoryAPI.DTOs;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Services
{
    public interface IProductService
    {
        Task<ActionResult<Product>> AddProduct(ProductDTO productDTO);
        Task<IEnumerable<Product>> GetProductsByStoreId(int storeId);
        Task<IActionResult> DeleteProduct(int productId);
        Task<IActionResult> UpdateProduct(int productId, ProductDTO productDTO);

    }
}
