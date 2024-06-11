using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI.DTO;
using InventoryAPI.Models;

namespace InventoryAPI.Services
{
    public interface ISupplierRequestService
    {
        Task<IEnumerable<SupplierRequest>> GetAllSupplierRequests(int userId);
        Task<IActionResult> UpdateRequest(int id, UpdateRequestDto updateDto);
        Task<IActionResult> DeleteRequest(int id);
        Task<IEnumerable<SupplierRequest>> GetSupplierRequestsForEmployee(int employeeId);


    }
}
