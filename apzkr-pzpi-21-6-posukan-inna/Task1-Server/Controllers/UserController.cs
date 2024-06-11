using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Services;
using System.Collections.Generic;
using InventoryAPI.Interfaces;
using System;
using System.Threading.Tasks;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] User user)
        {
            try
            {
                return await _userService.UserLogin(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during user login: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegistration([FromBody] User user)
        {
            try
            {
                return await _userService.UserRegistration(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during user registration: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                IEnumerable<User> users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting all users: {ex.Message}");
            }
        }
    }
}
