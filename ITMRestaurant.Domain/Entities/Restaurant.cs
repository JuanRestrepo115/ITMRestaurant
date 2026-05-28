using System;
using System.Collections.Generic;
using System.Text;

namespace ITMRestaurant.Domain.Entities
{
    public class Restaurant: AuditBase
    {
        public string Branch { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        // Navigation properties

        public ICollection<Table> Tables { get; set; } = new List<Table>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
