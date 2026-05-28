using ITMRestaurant.Domain.Enums;

namespace ITMRestaurant.API.DTOs.Response
{
    public class TableResponseDTO
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantBranch { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public TableState State { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
