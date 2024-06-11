using InventoryAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Interfaces
{
    public interface ISupplierService
    {
        Task<Supplier> AddSupplier(Supplier supplier);
        Task<IEnumerable<Supplier>> GetAllSuppliers();
        Task<bool> DeleteSupplier(int id);
    }
}
