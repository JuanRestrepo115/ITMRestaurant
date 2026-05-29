using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;


namespace ITMRestaurant.Domain.Interfaces.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetByStateAsync(ReservationState state);
        Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Reservation?> GetReservationWithDetailsAsync(int id);
        Task UpdateStateAsync(int id, ReservationState newState);
        Task<Reservation> CreateAsync(Reservation reservation);
        Task UpdateAsync(int id, Reservation reservation);
        Task DeleteAsync(int id);
    }
}
