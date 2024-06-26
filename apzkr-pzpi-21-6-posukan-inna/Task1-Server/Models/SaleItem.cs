﻿using System;
using System.Collections.Generic;

namespace InventoryAPI.Models
{
    public partial class SaleItem
    {
        public int SaleItemId { get; set; }
        public int? SaleId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Sale? Sale { get; set; }
    }
}
