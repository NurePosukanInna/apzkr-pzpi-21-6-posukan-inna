using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class Sensor
    {
        public int SensorId { get; set; }
        public int? StoreId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; } 

        public DateTime? Timestamp { get; set; }

        public virtual Store? Store { get; set; }
    }
}
