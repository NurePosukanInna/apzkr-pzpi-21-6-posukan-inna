using InventoryAPI.Data;
using InventoryAPI.DTOs;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace InventoryAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;

        public ProductService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Product>> AddProduct(ProductDTO productDTO)
        {
            try
            {
                var product = new Product
                {
                    ProductName = productDTO.ProductName,
                    Price = productDTO.Price,
                    CategoryId = productDTO.CategoryId,
                    SupplierId = productDTO.SupplierId,
                    Currency = productDTO.Currency,
                    Volume = productDTO.Volume,
                    MeasureOfUnits = productDTO.MeasureOfUnits,
                    IsFresh = productDTO.IsFresh
                };

                if (productDTO.IsFresh == true)
                {
                    product.ExpiryDate = productDTO.ExpiryDate;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var productsInStore = new StoreProduct
                {
                    ProductId = product.ProductId,
                    StoreId = productDTO.StoreId,
                    Quantity = productDTO.Quantity,
                    MinQuantity = productDTO.MinQuantity
                };
                _context.StoreProducts.Add(productsInStore);
                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error while adding product: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByStoreId(int storeId)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.StoreProducts)
                    .Include(p => p.Supplier)
                    .Include(p => p.Category) 
                    .Where(p => p.StoreProducts.Any(ps => ps.StoreId == storeId))
                    .Where(p => !_context.DefectiveProducts.Any(dp => dp.ProductId == p.ProductId))
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving products: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    return new BadRequestObjectResult($"Product with ID {productId} not found.");
                }

                var relatedSaleItems = _context.SaleItems.Where(si => si.ProductId == productId);
                var relatedStoreProducts = _context.StoreProducts.Where(sp => sp.ProductId == productId);

                _context.SaleItems.RemoveRange(relatedSaleItems);
                _context.StoreProducts.RemoveRange(relatedStoreProducts);


                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                return new OkObjectResult($"Product with ID {productId} deleted successfully.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error while deleting product: {ex.Message}");
            }
        }
      public async Task<IActionResult> UpdateProduct(int productId, ProductDTO productDTO)
{
    try
    {
        var product = await _context.Products.Include(p => p.StoreProducts).FirstOrDefaultAsync(p => p.ProductId == productId);

        if (product == null)
        {
            return new BadRequestObjectResult($"Product with ID {productId} not found.");
        }

        product.ProductName = productDTO.ProductName;
        product.Price = productDTO.Price;
        product.CategoryId = productDTO.CategoryId;
        product.SupplierId = productDTO.SupplierId;
        product.Currency = productDTO.Currency;
        product.Volume = productDTO.Volume;
        product.MeasureOfUnits = productDTO.MeasureOfUnits;
        product.IsFresh = productDTO.IsFresh;

        if (productDTO.IsFresh)
        {
            product.ExpiryDate = productDTO.ExpiryDate;
        }
        else
        {
            product.ExpiryDate = null;
        }

        if (product.StoreProducts.Any())
        {
            var storeProduct = product.StoreProducts.First();
            storeProduct.Quantity = productDTO.Quantity;
            storeProduct.MinQuantity = productDTO.MinQuantity;
        }

        await _context.SaveChangesAsync();

        return new OkObjectResult($"Product with ID {productId} updated successfully.");
    }
    catch (Exception ex)
    {
        return new BadRequestObjectResult($"Error while updating product: {ex.Message}");
    }
}

    }
}
