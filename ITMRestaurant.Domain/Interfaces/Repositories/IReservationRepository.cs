using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;

namespace ITMRestaurant.Domain.Interfaces.Repositories
{
    public interface IReservationRepository: IGenericRepository<Reservation>
    {
        Task<Reservation?> GetReservationWithDetailsAsync(int id);
        Task<IEnumerable<Reservation>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Reservation>> GetByStateAsync(ReservationState state);
        Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Reservation>> GetByTableIdAsync(int tableId);
        Task UpdateStateAsync(int id, ReservationState newState);
    }
}
