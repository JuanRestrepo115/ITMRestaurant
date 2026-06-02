using ITMRestaurant.Domain.Enums;

namespace ITMRestaurant.API.DTOs.Response
{
    public class ReservationResponseDTO
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFullName { get; set; } = string.Empty; 
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public DateTime ReservationTime { get; set; }
        public int NumberOfGuests { get; set; }
        public ReservationState State { get; set; }
        public string Observations { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
