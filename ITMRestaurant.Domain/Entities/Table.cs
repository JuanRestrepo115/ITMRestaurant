using ITMRestaurant.Domain.Enums;
namespace ITMRestaurant.Domain.Entities
{
    public class Table: AuditBase
    {
        public int TableNumber { get; set; }

        //Foreign Key
        public int RestaurantId { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; } = string.Empty;

        public TableState State { get; set; }

        //Navigation propierties

        //1:N

        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
