using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class DefectiveProduct
    {
        public int DefectiveProductId { get; set; }
        public int? ProductId { get; set; }
        public string? Reason { get; set; }
        public DateTime? DateDetected { get; set; }

        public virtual Product? Product { get; set; }
    }
}
