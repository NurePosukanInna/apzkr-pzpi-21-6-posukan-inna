using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
