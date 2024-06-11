using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Services
{
    public class SaleService : ISaleService
    {
        private readonly InventoryContext _context;

        public SaleService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<Sale> AddSale(Sale sale)
        {
            var currentTime = DateTime.UtcNow;
            sale.SaleDate = currentTime;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            foreach (var salesItem in sale.SaleItems)
            {
                var storeProduct = await _context.StoreProducts
                    .Include(sp => sp.Product)
                    .Where(p => p.StoreId == sale.StoreId && p.ProductId == salesItem.ProductId)
                    .FirstOrDefaultAsync();

                if (storeProduct != null)
                {
                    storeProduct.Quantity -= salesItem.Quantity;

                    if (storeProduct.Quantity < storeProduct.MinQuantity)
                    {
                        var orderQuantity = await CalculateOrderQuantity((int)storeProduct.ProductId);

                        decimal amount = (decimal)(storeProduct.Product.Price * orderQuantity);

                        decimal discount = amount * 0.2m;

                        decimal totalAmount = amount - discount;

                        var request = new SupplierRequest
                        {
                            StoreProductId = storeProduct.StoreProductId,
                            Quantity = orderQuantity,
                            TotalAmount = totalAmount, 
                            RequestDate = currentTime,
                            RequestStatus = "Pending"
                        };

                        _context.SupplierRequests.Add(request);
                    }
                    salesItem.Price = storeProduct.Product.Price;
                }
            }

            await _context.SaveChangesAsync();

            return sale;
        }



        public async Task<int?> CalculateOrderQuantity(int productId)
        {
            var lastWeekStartDate = DateTime.UtcNow.Date.AddDays(-7);

            var completedRequests = await _context.SupplierRequests
                .Where(r => r.StoreProduct != null &&
                            r.StoreProduct.ProductId == productId &&
                            r.RequestStatus == "Completed" &&
                            r.RequestDate >= lastWeekStartDate &&
                            r.RequestDate != null)
                .OrderByDescending(r => r.RequestDate)
                .Take(2)
                .ToListAsync();

            if (completedRequests.Count >= 2)
            {
                var averageQuantity = completedRequests.Average(r => r.Quantity);
                var orderQuantity = (int)Math.Ceiling((double)(averageQuantity * 1.2));
                return orderQuantity;
            }
            else
            {
                var storeProduct = await _context.StoreProducts
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (storeProduct != null)
                {
                    var orderQuantity = storeProduct.MinQuantity - storeProduct.Quantity + 2;
                    return (int)orderQuantity;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<IEnumerable<Sale>> GetSalesByStoreId(int storeId)
        {
            var sales = await _context.Sales
                .Include(s => s.SaleItems)
                 .ThenInclude(s => s.Product)
                 .Include(s => s.Employee) 
                .Where(s => s.StoreId == storeId)
                .ToListAsync();

            return sales;
        }
    }
}
