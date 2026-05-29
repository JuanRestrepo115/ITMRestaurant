using ITMRestaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITMRestaurant.Domain.Interfaces.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task<IEnumerable<Restaurant>> GetActiveRestaurantsAsync();
        Task<Restaurant?> GetRestaurantWithTablesAsync(int id);
        Task UpdateIsActiveAsync(int id, bool isActive);
        Task<Restaurant> CreateAsync(Restaurant restaurant);
        Task UpdateAsync(int id, Restaurant restaurant);
        Task DeleteAsync(int id);
    }
}
