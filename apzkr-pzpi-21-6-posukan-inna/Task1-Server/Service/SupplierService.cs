using InventoryAPI.Data;
using InventoryAPI.Interfaces;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly InventoryContext _context;

        public SupplierService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            try
            {
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();
                return supplier;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding supplier: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _context.Suppliers.ToListAsync();
                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving suppliers: {ex.Message}");
            }
        }

        public async Task<bool> DeleteSupplier(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier == null)
                {
                    return false;
                }

                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting supplier: {ex.Message}");
            }
        }
    }
}
