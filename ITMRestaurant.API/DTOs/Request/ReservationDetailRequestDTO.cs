namespace ITMRestaurant.API.DTOs.Request
{
    public class ReservationDetailRequestDTO
    {
        public int ReservationId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
