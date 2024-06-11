using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class Store
    {
        public Store()
        {
            Employees = new HashSet<Employee>();
            Sales = new HashSet<Sale>();
            Sensors = new HashSet<Sensor>();
            StoreProducts = new HashSet<StoreProduct>();
        }

        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Address { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Sensor> Sensors { get; set; }
        public virtual ICollection<StoreProduct> StoreProducts { get; set; }
    }
}
