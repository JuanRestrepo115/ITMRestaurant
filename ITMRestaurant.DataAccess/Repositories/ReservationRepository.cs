using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace ITMRestaurant.DataAccess.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(RestaurantDbContext context) : base(context)
        {
        }


        public override async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .Include(r => r.ReservationDetails)
                    .ThenInclude(rd => rd.MenuItem)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public override async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _dbSet
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet.Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(r => r.ReservationTime >= startDate && r.ReservationTime <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByStateAsync(ReservationState state)
        {
            return await _dbSet.Where(r => r.State == state)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByTableIdAsync(int tableId)
        {
            return await _dbSet.Where(r => r.TableId == tableId)
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .Include(r => r.Restaurant)
                .Include(r => r.ReservationDetails)
                    .ThenInclude(rd => rd.MenuItem)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateStateAsync(int id, ReservationState newState)
        {
            var reservation = await _dbSet.FindAsync(id);
            if (reservation != null)
            {
                reservation.State = newState;
                reservation.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
