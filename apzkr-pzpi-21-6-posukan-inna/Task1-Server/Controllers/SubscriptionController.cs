using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Services;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionType>>> GetSubscriptionTypes()
        {
            try
            {
                var subscriptionTypes = await _subscriptionService.GetSubscriptionTypes();
                return Ok(subscriptionTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving subscription types: {ex.Message}");
            }
        }

        [HttpPost("addType")]
        public async Task<ActionResult<SubscriptionType>> AddSubscriptionType(SubscriptionType subscriptionType)
        {
            try
            {
                var addedSubscriptionType = await _subscriptionService.AddSubscriptionType(subscriptionType);
                return Ok(addedSubscriptionType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while adding subscription type: {ex.Message}");
            }
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<Subscription>> AddSubscriptionToUser(int userId, Subscription subscription)
        {
            try
            {
                var addedSubscription = await _subscriptionService.AddSubscriptionToUser(userId, subscription);
                return Ok(addedSubscription);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while adding subscription to user: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetActiveSubscriptionsForOwner(int userId)
        {
            try
            {
                var activeSubscriptions = await _subscriptionService.GetActiveSubscriptionsForUser(userId);
                return Ok(activeSubscriptions);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving active subscriptions for user: {ex.Message}");
            }
        }
    }
}
