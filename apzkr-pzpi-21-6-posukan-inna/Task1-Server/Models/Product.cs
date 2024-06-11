using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            DefectiveProducts = new HashSet<DefectiveProduct>();
            SaleItems = new HashSet<SaleItem>();
            StoreProducts = new HashSet<StoreProduct>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; } 
        public decimal? Volume { get; set; }
        public string? MeasureOfUnits { get; set; } 
        public bool? IsFresh { get; set; }
        public DateTime? ExpiryDate { get; set; } 
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<DefectiveProduct> DefectiveProducts { get; set; }
        public virtual ICollection<SaleItem> SaleItems { get; set; }
        public virtual ICollection<StoreProduct> StoreProducts { get; set; }
    }
}
