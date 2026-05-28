using ITMRestaurant.Domain.Enums;

namespace ITMRestaurant.API.DTOs.Request
{
    public class MenuItemRequestDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public MenuCategory Category { get; set; }

    }
}
