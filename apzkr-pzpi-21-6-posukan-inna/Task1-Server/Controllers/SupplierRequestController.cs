using Microsoft.AspNetCore.Mvc;
using InventoryAPI.DTO;
using InventoryAPI.Models;
using InventoryAPI.Services;


namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierRequestController : ControllerBase
    {
        private readonly ISupplierRequestService _supplierRequestService;

        public SupplierRequestController(ISupplierRequestService supplierRequestService)
        {
            _supplierRequestService = supplierRequestService;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllSupplierRequests(int userId)
        {
            try
            {
                var supplierRequests = await _supplierRequestService.GetAllSupplierRequests(userId);
                return Ok(supplierRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, UpdateRequestDto updateDto)
        {
            try
            {
                var result = await _supplierRequestService.UpdateRequest(id, updateDto);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating supplier request: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            try
            {
                var result = await _supplierRequestService.DeleteRequest(id);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting supplier request: {ex.Message}");
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetSupplierRequestsForEmployee(int employeeId)
        {
            try
            {
                var supplierRequests = await _supplierRequestService.GetSupplierRequestsForEmployee(employeeId);
                return Ok(supplierRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
