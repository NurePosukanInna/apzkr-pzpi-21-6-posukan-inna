using InventoryAPI.Models;


namespace InventoryAPI.Services
{
    public interface IStoreService
    {
        Task<Store> AddStore(Store store);
        Task<IEnumerable<Store>> GetStoresByUserId(int userId);
        Task<Store> UpdateStore(int storeId, Store updatedStore);
    }
}
