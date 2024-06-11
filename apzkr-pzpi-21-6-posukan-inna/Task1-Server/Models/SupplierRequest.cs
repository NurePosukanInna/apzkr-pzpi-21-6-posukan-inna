using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class SupplierRequest
    {
        public int RequestId { get; set; }
        public int? StoreProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? RequestStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public virtual StoreProduct? StoreProduct { get; set; }
    }
}
