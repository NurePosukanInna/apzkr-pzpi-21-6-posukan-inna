using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> UserLogin(User user);
        Task<IActionResult> UserRegistration(User user);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
