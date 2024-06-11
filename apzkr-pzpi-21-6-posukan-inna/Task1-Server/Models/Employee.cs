using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Sales = new HashSet<Sale>();
        }

        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? StoreId { get; set; }

        public virtual Store? Store { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
