using System;
using System.Collections.Generic;
using System.Text;

namespace ITMRestaurant.Domain.Entities
{
    public class ReservationDetail: AuditBase
    {
        // Foreing Keys
        public int ReservationId { get; set; }

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        //Navigation properties

        public Reservation Reservation { get; set; } = null!;

        public MenuItem MenuItem { get; set; } = null!;
    }
}
