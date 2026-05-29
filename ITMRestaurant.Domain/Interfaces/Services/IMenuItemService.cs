using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;

namespace ITMRestaurant.Domain.Interfaces.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem?> GetByIdAsync(int id);
        Task<IEnumerable<MenuItem>> GetAvailableItemsAsync();
        Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category);
        Task<IEnumerable<MenuItem>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<MenuItem> CreateAsync(MenuItem menuItem);
        Task UpdateAsync(int id, MenuItem menuItem);
        Task DeleteAsync(int id);
    }
}
