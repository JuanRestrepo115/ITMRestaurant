using ITMRestaurant.Domain.Enums;
namespace ITMRestaurant.Domain.Entities
{
    public class Reservation: AuditBase
    {
        public int RestaurantID { get; set; }

        public int CustomerID { get; set; }

        public int TableID { get; set; }

        public DateTime ReservationTime { get; set; }

        public int NumberOfGuests { get; set; }

        //Enum de Reservation

        public ReservationState State { get; set; }

        public string Observations { get; set; } = string.Empty;

        //Navigation Properties

        public Customer Customer { get; set; } = null!;

        public Restaurant Restaurant { get; set; } = null!;

        public Table Table { get; set; } = null!;

        public ICollection<ReservationDetail> ReservationDetails { get; set; } = new List<ReservationDetail>();


    }
}
