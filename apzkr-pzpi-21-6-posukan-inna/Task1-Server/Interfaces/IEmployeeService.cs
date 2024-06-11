using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryAPI.Models;

namespace InventoryAPI.Services
{
    public interface IEmployeeService
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<bool> DeleteEmployee(int id);
        Task<IEnumerable<Store>> GetStoresByEmployeeId(int id);
        Task<Employee> UpdateEmployee(int id, Employee employeeData);
        Task<IEnumerable<Employee>> GetAllEmployees(int userId);
    }
}
