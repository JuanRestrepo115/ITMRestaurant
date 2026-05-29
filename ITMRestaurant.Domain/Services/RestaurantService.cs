using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ITMRestaurant.Domain.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(IRestaurantRepository restaurantRepository, ILogger<RestaurantService> logger)
        {
            _restaurantRepository = restaurantRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all restaurants");
            return await _restaurantRepository.GetAllAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving restaurant with ID: {Id}", id);
            var restaurant = await _restaurantRepository.GetByIdAsync(id);

            if (restaurant == null)
                _logger.LogWarning("Restaurant with ID {Id} not found", id);

            return restaurant;
        }

        public async Task<IEnumerable<Restaurant>> GetActiveRestaurantsAsync()
        {
            _logger.LogInformation("Retrieving all active restaurants");
            return await _restaurantRepository.GetActiveRestaurantAsync();
        }

        public async Task<Restaurant?> GetRestaurantWithTablesAsync(int id)
        {
            _logger.LogInformation("Retrieving restaurant with tables for ID: {Id}", id);
            var restaurant = await _restaurantRepository.GetRestaurantsWithTablesAsync(id);

            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant with ID {Id} not found", id);
                throw new KeyNotFoundException($"Restaurant with ID {id} not found.");
            }

            return restaurant;
        }

        public async Task UpdateIsActiveAsync(int id, bool isActive)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null)
            {
                _logger.LogWarning("Attempt to update a non-existing restaurant with ID: {Id}", id);
                throw new KeyNotFoundException($"Restaurant with ID {id} not found.");
            }

            // Validar que no este ya en el mismo estado
            if (restaurant.IsActive == isActive)
            {
                _logger.LogWarning("Restaurant with ID {Id} is already {IsActive}", id, isActive ? "active" : "inactive");
                throw new InvalidOperationException($"Restaurant is already {(isActive ? "active" : "inactive")}.");
            }

            _logger.LogInformation("Updating restaurant with ID: {Id} IsActive to {IsActive}", id, isActive);
            await _restaurantRepository.UpdateIsActiveAsync(id, isActive);
        }

        public async Task<Restaurant> CreateAsync(Restaurant restaurant)
        {
            // Validar que no exista una sucursal con el mismo nombre
            var existingRestaurant = await _restaurantRepository.GetByBranchAsync(restaurant.Branch);
            if (existingRestaurant != null)
            {
                _logger.LogWarning("Attempt to create a restaurant with an existing branch: {Branch}", restaurant.Branch);
                throw new InvalidOperationException("A restaurant with the specified branch already exists.");
            }

            // IsActive siempre true al crear
            restaurant.IsActive = true;

            _logger.LogInformation("Creating a new restaurant: {Branch}", restaurant.Branch);
            return await _restaurantRepository.CreateAsync(restaurant);
        }

        public async Task UpdateAsync(int id, Restaurant restaurant)
        {
            var existingRestaurant = await _restaurantRepository.GetByIdAsync(id);
            if (existingRestaurant == null)
            {
                _logger.LogWarning("Attempt to update a non-existing restaurant with ID: {Id}", id);
                throw new KeyNotFoundException($"Restaurant with ID {id} not found.");
            }

            // Validar nombre de sucursal unico si cambio
            if (existingRestaurant.Branch != restaurant.Branch)
            {
                var branchConflict = await _restaurantRepository.GetByBranchAsync(restaurant.Branch);
                if (branchConflict != null)
                {
                    _logger.LogWarning("Attempt to update restaurant with an existing branch: {Branch}", restaurant.Branch);
                    throw new InvalidOperationException("A restaurant with the specified branch already exists.");
                }
            }

            existingRestaurant.Branch = restaurant.Branch;
            existingRestaurant.Address = restaurant.Address;
            existingRestaurant.PhoneNumber = restaurant.PhoneNumber;
            existingRestaurant.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating restaurant with ID: {Id}", id);
            await _restaurantRepository.UpdateAsync(existingRestaurant);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _restaurantRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Attempt to delete a non-existing restaurant with ID: {Id}", id);
                throw new KeyNotFoundException($"Restaurant with ID {id} not found.");
            }

            _logger.LogInformation("Deleting restaurant with ID: {Id}", id);
            await _restaurantRepository.DeleteAsync(id);
        }
    }
}
