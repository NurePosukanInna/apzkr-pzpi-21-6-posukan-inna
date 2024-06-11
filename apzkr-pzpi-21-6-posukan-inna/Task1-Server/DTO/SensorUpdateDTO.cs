namespace InventoryAPI.DTO
{
    public class SensorUpdateDto
    {
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public DateTime? Timestamp { get; set; }

    }

}
