using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace ITMRestaurant.DataAccess.Repositories
{
    public class TableRepository: GenericRepository<Table>, ITableRepository
    {
        public TableRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Table>> GetByStateAsync(TableState state)
        {
            return await _dbSet.Where(t => t.State == state)
                .ToListAsync();
        }

        public async Task<Table?> GetByTableNumberAsync(int tableNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TableNumber == tableNumber);
        }

        public async Task<Table?> GetTableWithReservationsAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Reservations)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateStateAsync(int id, TableState newState)
        {
            var table = await _dbSet.FindAsync(id);
            if (table != null)
            {
                table.State = newState;
                table.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

            }
        }
    }
}
