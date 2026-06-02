using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ITMRestaurant.Domain.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILogger<TableService> _logger;

        public TableService(
            ITableRepository tableRepository,
            IRestaurantRepository restaurantRepository,
            ILogger<TableService> logger)
        {
            _tableRepository = tableRepository;
            _restaurantRepository = restaurantRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all tables");
            return await _tableRepository.GetAllAsync();
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving table with ID: {Id}", id);
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
                _logger.LogWarning("Table with ID {Id} not found", id);

            return table;
        }

        public async Task<IEnumerable<Table>> GetByStateAsync(TableState state)
        {
            _logger.LogInformation("Retrieving tables by state: {State}", state);
            return await _tableRepository.GetByStateAsync(state);
        }

        public async Task UpdateStateAsync(int id, TableState newState)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null)
            {
                _logger.LogWarning("Attempt to update state of a non-existing table with ID: {Id}", id);
                throw new KeyNotFoundException($"Table with ID {id} not found.");
            }

            // Validar que no este ya en el mismo estado
            if (table.State == newState)
            {
                _logger.LogWarning("Table with ID {Id} is already in state {State}", id, newState);
                throw new InvalidOperationException($"Table is already in state {newState}.");
            }

            _logger.LogInformation("Updating state of table with ID: {Id} to {State}", id, newState);
            await _tableRepository.UpdateStateAsync(id, newState);
        }

        public async Task<Table> CreateAsync(Table table)
        {
            // Validar que el restaurante existe
            var restaurantExists = await _restaurantRepository.ExistsAsync(table.RestaurantId);
            if (!restaurantExists)
            {
                _logger.LogWarning("Restaurant with ID {RestaurantId} not found", table.RestaurantId);
                throw new KeyNotFoundException($"Restaurant with ID {table.RestaurantId} not found.");
            }

            // Validar numero de mesa unico
            var existingTable = await _tableRepository.GetByTableNumberAsync(table.TableNumber);
            if (existingTable != null)
            {
                _logger.LogWarning("Attempt to create a table with an existing number: {TableNumber}", table.TableNumber);
                throw new InvalidOperationException($"A table with number {table.TableNumber} already exists.");
            }

            // Validar capacidad positiva
            if (table.Capacity <= 0)
            {
                _logger.LogWarning("Attempt to create a table with invalid capacity: {Capacity}", table.Capacity);
                throw new InvalidOperationException("Table capacity must be greater than zero.");
            }

            // State siempre Available al crear
            table.State = TableState.Available;

            _logger.LogInformation("Creating a new table: {TableNumber}", table.TableNumber);
            return await _tableRepository.CreateAsync(table);
        }

        public async Task UpdateAsync(int id, Table table)
        {
            var existingTable = await _tableRepository.GetByIdAsync(id);
            if (existingTable == null)
            {
                _logger.LogWarning("Attempt to update a non-existing table with ID: {Id}", id);
                throw new KeyNotFoundException($"Table with ID {id} not found.");
            }

            // Validar numero de mesa unico si cambio
            if (existingTable.TableNumber != table.TableNumber)
            {
                var tableConflict = await _tableRepository.GetByTableNumberAsync(table.TableNumber);
                if (tableConflict != null)
                {
                    _logger.LogWarning("Attempt to update table with an existing number: {TableNumber}", table.TableNumber);
                    throw new InvalidOperationException($"A table with number {table.TableNumber} already exists.");
                }
            }

            // Validar capacidad positiva
            if (table.Capacity <= 0)
            {
                _logger.LogWarning("Attempt to update table with invalid capacity: {Capacity}", table.Capacity);
                throw new InvalidOperationException("Table capacity must be greater than zero.");
            }


            existingTable.TableNumber = table.TableNumber;
            existingTable.Capacity = table.Capacity;
            existingTable.Location = table.Location;
            existingTable.RestaurantId = table.RestaurantId;
            existingTable.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating table with ID: {Id}", id);
            await _tableRepository.UpdateAsync(existingTable);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _tableRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Attempt to delete a non-existing table with ID: {Id}", id);
                throw new KeyNotFoundException($"Table with ID {id} not found.");
            }

            _logger.LogInformation("Deleting table with ID: {Id}", id);
            await _tableRepository.DeleteAsync(id);
        }
    }
}