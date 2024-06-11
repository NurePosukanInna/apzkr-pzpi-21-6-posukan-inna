using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class Subscription
    {
        public int SubscriptionId { get; set; }
        public int? SubscriptionTypeId { get; set; }
        public string? SubscriptionStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }

        public virtual SubscriptionType? SubscriptionType { get; set; }
        public virtual User? User { get; set; }
    }
}
