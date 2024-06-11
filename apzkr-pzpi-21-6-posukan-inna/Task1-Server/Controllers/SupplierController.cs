using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        public async Task<ActionResult<Supplier>> AddSupplier(Supplier supplier)
        {
            try
            {
                var addedSupplier = await _supplierService.AddSupplier(supplier);
                return Ok(addedSupplier);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding supplier: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliers();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving suppliers: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                var result = await _supplierService.DeleteSupplier(id);
                if (!result)
                {
                    return NotFound($"Supplier with ID {id} not found.");
                }
                return Ok($"Supplier with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting supplier: {ex.Message}");
            }
        }
    }
}
