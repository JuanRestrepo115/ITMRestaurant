using ITMRestaurant.Domain.Enums;

namespace ITMRestaurant.API.DTOs.Response
{
    public class ReservationDetailResponseDTO
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
