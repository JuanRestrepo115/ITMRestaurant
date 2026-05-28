namespace ITMRestaurant.Domain.Entities
{
    public class Customer: AuditBase
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;

        //Navigation propierties

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
