using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ITMRestaurant.Domain.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ILogger<MenuItemService> _logger;

        public MenuItemService(IMenuItemRepository menuItemRepository, ILogger<MenuItemService> logger)
        {
            _menuItemRepository = menuItemRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all menu items");
            return await _menuItemRepository.GetAllAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving menu item with ID: {Id}", id);
            var menuItem = await _menuItemRepository.GetByIdAsync(id);

            if (menuItem == null)
                _logger.LogWarning("Menu item with ID {Id} not found", id);

            return menuItem;
        }

        public async Task<IEnumerable<MenuItem>> GetAvailableItemsAsync()
        {
            _logger.LogInformation("Retrieving all available menu items");
            return await _menuItemRepository.GetAvailableItemsAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category)
        {
            _logger.LogInformation("Retrieving menu items by category: {Category}", category);
            return await _menuItemRepository.GetByCategoryAsync(category);
        }

        public async Task<IEnumerable<MenuItem>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            // Verificar que los precios sean válidos
            if (minPrice < 0 || maxPrice < 0)
            {
                _logger.LogWarning("Invalid price range: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
                throw new InvalidOperationException("Price range values cannot be negative.");
            }

            if (minPrice > maxPrice)
            {
                _logger.LogWarning("Invalid price range: {MinPrice} > {MaxPrice}", minPrice, maxPrice);
                throw new InvalidOperationException("Minimum price cannot be greater than maximum price.");
            }

            _logger.LogInformation("Retrieving menu items by price range: {MinPrice} - {MaxPrice}", minPrice, maxPrice);
            return await _menuItemRepository.GetByPriceRangeAsync(minPrice, maxPrice);
        }

        public async Task<MenuItem> CreateAsync(MenuItem menuItem)
        {
            // Validar que el nombre del plato sea unico
            var existingItems = await _menuItemRepository.GetByNameAsync(menuItem.Name);
            if (existingItems.Any())
            {
                _logger.LogWarning("Attempt to create a menu item with an existing name: {Name}", menuItem.Name);
                throw new InvalidOperationException("A menu item with the specified name already exists.");
            }

            // Validar precio del plato sea positivo
            if (menuItem.Price <= 0)
            {
                _logger.LogWarning("Attempt to create a menu item with invalid price: {Price}", menuItem.Price);
                throw new InvalidOperationException("Price must be greater than zero.");
            }

            menuItem.IsAvailable = true;

            _logger.LogInformation("Creating a new menu item: {Name}", menuItem.Name);
            return await _menuItemRepository.CreateAsync(menuItem);
        }

        public async Task UpdateAsync(int id, MenuItem menuItem)
        {
            var existingItem = await _menuItemRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                _logger.LogWarning("Attempt to update a non-existing menu item with ID: {Id}", id);
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }

            // Validar nombre unico si cambio
            if (existingItem.Name != menuItem.Name)
            {
                var nameConflict = await _menuItemRepository.GetByNameAsync(menuItem.Name);
                if (nameConflict.Any())
                {
                    _logger.LogWarning("Attempt to update menu item with an existing name: {Name}", menuItem.Name);
                    throw new InvalidOperationException("A menu item with the specified name already exists.");
                }
            }

            // Validar precio positivo
            if (menuItem.Price <= 0)
            {
                _logger.LogWarning("Attempt to update menu item with invalid price: {Price}", menuItem.Price);
                throw new InvalidOperationException("Price must be greater than zero.");
            }

            existingItem.Name = menuItem.Name;
            existingItem.Description = menuItem.Description;
            existingItem.Price = menuItem.Price;
            existingItem.Category = menuItem.Category;
            existingItem.IsAvailable = true;
            existingItem.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating menu item with ID: {Id}", id);
            await _menuItemRepository.UpdateAsync(existingItem);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _menuItemRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Attempt to delete a non-existing menu item with ID: {Id}", id);
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }

            _logger.LogInformation("Deleting menu item with ID: {Id}", id);
            await _menuItemRepository.DeleteAsync(id);
        }

        public async Task UpdateAvailabilityAsync(int id, bool isAvailable)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
            {
                _logger.LogWarning("Attempt to update availability of a non-existing menu item with ID: {Id}", id);
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }

            // Validar que no este ya en el mismo estado
            if (menuItem.IsAvailable == isAvailable)
            {
                _logger.LogWarning("Menu item with ID {Id} is already {IsAvailable}", id, isAvailable ? "available" : "unavailable");
                throw new InvalidOperationException($"Menu item is already {(isAvailable ? "available" : "unavailable")}.");
            }

            _logger.LogInformation("Updating availability of menu item with ID: {Id} to {IsAvailable}", id, isAvailable);
            await _menuItemRepository.UpdateAvailabilityAsync(id, isAvailable);
        }
    }
}