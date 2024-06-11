using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class User
    {
        public User()
        {
            Stores = new HashSet<Store>();
            Subscriptions = new HashSet<Subscription>();
        }

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
