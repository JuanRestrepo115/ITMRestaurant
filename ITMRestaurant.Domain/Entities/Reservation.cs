using ITMRestaurant.Domain.Enums;
namespace ITMRestaurant.Domain.Entities
{
    public class Reservation: AuditBase
    {

        // Foreign Keys
        public int RestaurantId{ get; set; }

        public int CustomerId { get; set; }

        public int TableId { get; set; }

        public DateTime ReservationTime { get; set; }

        public int NumberOfGuests { get; set; }

        public string Observations { get; set; } = string.Empty;

        //Enum de Reservation
        public ReservationState State { get; set; }

        //Navigation Properties

        public Customer Customer { get; set; } = null!;

        public Restaurant Restaurant { get; set; } = null!;

        public Table Table { get; set; } = null!;

        public ICollection<ReservationDetail> ReservationDetails { get; set; } = new List<ReservationDetail>();


    }
}
