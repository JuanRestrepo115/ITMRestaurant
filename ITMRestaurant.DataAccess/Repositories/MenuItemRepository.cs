using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace ITMRestaurant.DataAccess.Repositories
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MenuItem>> GetAvailableItemsAsync()
        {
            return await _dbSet.Where(m => m.IsAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory menuCategory)
        {
            return await _dbSet.Where(m => m.Category == menuCategory)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetByNameAsync(string name)
        {
            return await _dbSet.Where(m => m.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet.Where(m => m.Price >= minPrice && m.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task UpdateAvailabilityAsync(int id, bool isAvailable)
        {
            var menuItem = await _dbSet.FindAsync(id);
            if (menuItem != null)
            {
                menuItem.IsAvailable = isAvailable;
                menuItem.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
