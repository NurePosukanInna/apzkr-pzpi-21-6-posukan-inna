using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI.DTO;
using InventoryAPI.Models;
using InventoryAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Services
{
    public class SupplierRequestService : ISupplierRequestService
    {
        private readonly InventoryContext _context;

        public SupplierRequestService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierRequest>> GetAllSupplierRequests(int userId)
        {
            try
            {
                var userStoreIds = await _context.Stores
                    .Where(s => s.UserId == userId)
                    .Select(s => s.StoreId)
                    .ToListAsync();

                var supplierRequests = await _context.SupplierRequests
                    .Where(req => req.StoreProduct != null && userStoreIds.Contains((int)req.StoreProduct.StoreId))
                    .Include(req => req.StoreProduct)
                        .ThenInclude(sp => sp.Store) 
                    .Include(req => req.StoreProduct)
                        .ThenInclude(sp => sp.Product) 
                    .ToListAsync();

                return supplierRequests;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving supplier requests for user stores: {ex.Message}");
            }
        }

        public async Task<IEnumerable<SupplierRequest>> GetSupplierRequestsForEmployee(int employeeId)
        {
            try
            {
                var userStoreId = await _context.Employees
                    .Where(e => e.EmployeeId == employeeId)
                    .Select(e => e.StoreId)
                    .FirstOrDefaultAsync();

                var supplierRequests = await _context.SupplierRequests
                    .Where(req => req.StoreProduct != null && req.StoreProduct.StoreId == userStoreId)
                    .Include(req => req.StoreProduct)
                        .ThenInclude(sp => sp.Store)
                    .Include(req => req.StoreProduct)
                        .ThenInclude(sp => sp.Product)
                    .ToListAsync();

                return supplierRequests;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving supplier requests for employee: {ex.Message}");
            }
        }


        public async Task<IActionResult> UpdateRequest(int id, UpdateRequestDto updateDto)
        {
            var request = await _context.SupplierRequests.FirstOrDefaultAsync(r => r.RequestId == id);
            if (request == null)
            {
                return new NotFoundResult();
            }

            var oldStatus = request.RequestStatus;

            if (oldStatus == "Completed")
            {
                return new BadRequestObjectResult("The status cannot be updated because the order has already been completed.");
            }

            if (updateDto.Quantity != 0)
            {
                request.Quantity = updateDto.Quantity;
            }

            request.RequestStatus = updateDto.RequestStatus;

            if (updateDto.RequestStatus == "Completed" && oldStatus != "Completed")
            {
                request.DeliveryDate = DateTime.Now;

                var storeProduct = await _context.StoreProducts.FirstOrDefaultAsync(sp => sp.StoreProductId == request.StoreProductId);

                if (storeProduct != null)
                {
                    storeProduct.Quantity += request.Quantity;
                }
            }

            await _context.SaveChangesAsync();

            return new OkObjectResult(request);
        }

        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.SupplierRequests.FindAsync(id);
            if (request == null)
            {
                return new BadRequestObjectResult($"Request with ID {id} not found.");
            }

            _context.SupplierRequests.Remove(request);
            await _context.SaveChangesAsync();

            return new OkObjectResult($"Request with ID {id} deleted successfully.");
        }
    }
}
