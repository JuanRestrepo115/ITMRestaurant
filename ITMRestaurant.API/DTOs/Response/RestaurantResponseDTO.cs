namespace ITMRestaurant.API.DTOs.Response
{
    public class RestaurantResponseDTO
    {
        public int Id { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
