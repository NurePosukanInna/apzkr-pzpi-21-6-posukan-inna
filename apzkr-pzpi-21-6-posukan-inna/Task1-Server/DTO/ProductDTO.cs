namespace InventoryAPI.DTOs
{
    public class ProductDTO
    {
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }
        public int MinQuantity { get; set; }
        public string Currency { get; set; } 
        public decimal? Volume { get; set; } 
        public string MeasureOfUnits { get; set; } 
        public bool IsFresh { get; set; }
        public DateTime? ExpiryDate { get; set; } 
    }
}
