using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
namespace ITMRestaurant.Domain.Interfaces.Repositories
{
    public interface IMenuItemRepository: IGenericRepository<MenuItem>
    {
        Task <IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory menuCategory);
        Task<IEnumerable<MenuItem>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<MenuItem>> GetByNameAsync(string name);
        Task<IEnumerable<MenuItem>> GetAvailableItemsAsync();
        Task UpdateAvailabilityAsync(int id, bool isAvailable);

    }
}
