using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Data;
using System;
using System.Linq;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefectiveProductsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public DefectiveProductsController(InventoryContext context)
        {
            _context = context;
        }
        [HttpGet("{storeId}")]
        public IActionResult GetAllDefectiveProductsForStore([FromRoute] int storeId)
        {
            try
            {
                var defectiveProductsForStore = _context.StoreProducts
                    .Where(sp => sp.StoreId == storeId)
                    .Join(_context.DefectiveProducts,
                        sp => sp.ProductId,
                        dp => dp.ProductId,
                        (sp, dp) => new
                        {
                            ProductId = sp.ProductId,
                            ProductName = sp.Product.ProductName,
                            Volume = sp.Product.Volume,
                            MeasureOfUnits = sp.Product.MeasureOfUnits,
                            DateDetected = dp.DateDetected,
                            Reason = dp.Reason

                        })
                    .ToList();

                return Ok(defectiveProductsForStore);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error while retrieving defective products for store: {ex.Message}");
            }
        }


    }
}

