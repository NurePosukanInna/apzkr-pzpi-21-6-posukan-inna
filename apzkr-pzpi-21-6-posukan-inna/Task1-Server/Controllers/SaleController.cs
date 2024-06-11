using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }


        [HttpPost]
        public async Task<ActionResult<Sale>> AddSale(Sale sale)
        {
            try
            {
                var createdSale = await _saleService.AddSale(sale);
                return Ok(createdSale);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while creating sale: {ex.Message}");
            }
        }


        [HttpGet("{storeId}")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByStoreId(int storeId)
        {
            try
            {
                var sales = await _saleService.GetSalesByStoreId(storeId);
                if (sales == null || !sales.Any())
                {
                    return NotFound("No sales found for the store.");
                }
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving sales: {ex.Message}");
            }
        }
    }
}
