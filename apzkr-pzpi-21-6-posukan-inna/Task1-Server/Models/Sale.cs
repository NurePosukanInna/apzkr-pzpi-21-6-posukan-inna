using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class Sale
    {
        public Sale()
        {
            SaleItems = new HashSet<SaleItem>();
        }

        public int SaleId { get; set; }
        public DateTime? SaleDate { get; set; }
        public int? EmployeeId { get; set; }
        public int? StoreId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
}
