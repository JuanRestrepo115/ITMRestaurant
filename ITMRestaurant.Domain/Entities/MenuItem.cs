using ITMRestaurant.Domain.Enums;
namespace ITMRestaurant.Domain.Entities
{
    public class MenuItem: AuditBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        //Enum de MenuItem

        public MenuCategory Category { get; set; }

        public bool IsAvailable { get; set; }


        //Navigation properties

        public ICollection<ReservationDetail> ReservationDetails { get; set; } = new List<ReservationDetail>();



    }
}
