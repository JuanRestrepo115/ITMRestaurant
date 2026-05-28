using ITMRestaurant.Domain.Entities;

namespace ITMRestaurant.Domain.Interfaces.Repositories
{
    public interface IRestaurantRepository: IGenericRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetActiveRestaurantAsync();

        Task<Restaurant?> GetRestaurantsWithTablesAsync(int id);

        Task<Restaurant?> GetByBranchAsync(string branch);
    }
}
