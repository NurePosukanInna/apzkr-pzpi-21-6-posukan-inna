using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAPI.Services
{
    public class StoreService : IStoreService
    {
        private readonly InventoryContext _context;

        public StoreService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<Store> AddStore(Store store)
        {
            try
            {
                _context.Stores.Add(store);
                await _context.SaveChangesAsync();

                return store;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding store: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Store>> GetStoresByUserId(int userId)
        {
            try
            {
                var stores = await _context.Stores
                    .Include(s => s.Sensors)
                    .Where(s => s.UserId == userId)
                    .ToListAsync();

                return stores;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving stores: {ex.Message}");
            }
        }


        public async Task<Store> UpdateStore(int storeId, Store updatedStore)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null)
                {
                    return null; 
                }

                store.StoreName = updatedStore.StoreName;
                store.Address = updatedStore.Address;
                store.UserId = updatedStore.UserId;

                await _context.SaveChangesAsync();

                return store;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating store: {ex.Message}");
            }
        }
    }
}
