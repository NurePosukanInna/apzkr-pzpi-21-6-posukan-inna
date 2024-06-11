using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAPI.Services
{
    public interface ISaleService
    {
        Task<Sale> AddSale(Sale sale);

        Task<IEnumerable<Sale>> GetSalesByStoreId(int storeId);
    }
}
