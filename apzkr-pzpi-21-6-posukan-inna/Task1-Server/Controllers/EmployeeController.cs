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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            try
            {
                var addedEmployee = await _employeeService.AddEmployee(employee);
                return Ok(addedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding employee: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var isDeleted = await _employeeService.DeleteEmployee(id);
                if (!isDeleted)
                {
                    return NotFound("Employee not found.");
                }

                return Ok("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting employee: {ex.Message}");
            }
        }

        [HttpGet("{id}/stores")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresByEmployeeId(int id)
        {
            try
            {
                var stores = await _employeeService.GetStoresByEmployeeId(id);
                if (stores == null || !stores.Any())
                {
                    return NotFound("No stores found for the employee.");
                }

                return Ok(stores);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving stores: {ex.Message}");
            }
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int userId)
        {
            try
            {
                var employees = await _employeeService.GetAllEmployees(userId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employeeData)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployee(id, employeeData);
                if (updatedEmployee == null)
                {
                    return NotFound("Employee not found.");
                }

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating employee: {ex.Message}");
            }
        }
    }
}
