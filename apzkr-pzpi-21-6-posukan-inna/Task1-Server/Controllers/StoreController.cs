using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Services;


namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpPost]
        public async Task<ActionResult<Store>> AddStore(Store store)
        {
            try
            {
                var addedStore = await _storeService.AddStore(store);
                return Ok(addedStore);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding store: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresByUserId(int userId)
        {
            try
            {
                var stores = await _storeService.GetStoresByUserId(userId);
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving stores: {ex.Message}");
            }
        }

        [HttpPut("{storeId}")]
        public async Task<ActionResult<Store>> UpdateStore(int storeId, Store updatedStore)
        {
            try
            {
                var store = await _storeService.UpdateStore(storeId, updatedStore);
                if (store == null)
                {
                    return NotFound("Store not found.");
                }

                return Ok(store);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating store: {ex.Message}");
            }
        }
    }
}
