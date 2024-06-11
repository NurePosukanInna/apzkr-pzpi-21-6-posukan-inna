using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class StoreProduct
    {
        public StoreProduct()
        {
            SupplierRequests = new HashSet<SupplierRequest>();
        }

        public int StoreProductId { get; set; }
        public int? StoreId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public int? MinQuantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<SupplierRequest> SupplierRequests { get; set; }
    }
}
