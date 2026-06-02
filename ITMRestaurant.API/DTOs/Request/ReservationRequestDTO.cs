
using ITMRestaurant.API.DTOs.Response;

namespace ITMRestaurant.API.DTOs.Request
{
    public class ReservationRequestDTO
    {
        public int RestaurantId { get; set; }
        public int CustomerId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string Observations { get; set; } = string.Empty;

        public ICollection<ReservationDetailRequestDTO> ReservationDetails { get; set; } = new List<ReservationDetailRequestDTO>();
    }
}
