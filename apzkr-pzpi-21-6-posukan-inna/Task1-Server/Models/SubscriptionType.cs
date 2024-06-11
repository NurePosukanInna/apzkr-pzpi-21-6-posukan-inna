using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class SubscriptionType
    {
        public SubscriptionType()
        {
            Subscriptions = new HashSet<Subscription>();
        }

        public int SubscriptionTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
