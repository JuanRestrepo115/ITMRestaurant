using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace ITMRestaurant.DataAccess.Repositories
{
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(RestaurantDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Restaurant>> GetActiveRestaurantAsync()
        {
            return await _dbSet.Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<Restaurant?> GetByBranchAsync(string branch)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Branch == branch);
        }

        public async Task UpdateIsActiveAsync(int id, bool isActive)
        {
            var restaurant = await _dbSet.FindAsync(id);
            if (restaurant != null)
            {
                restaurant.IsActive = isActive;
                restaurant.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}


