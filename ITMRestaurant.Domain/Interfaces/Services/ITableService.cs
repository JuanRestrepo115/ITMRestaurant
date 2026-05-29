using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
namespace ITMRestaurant.Domain.Interfaces.Services
{
    public interface ITableService
    {
        Task<IEnumerable<Table>> GetAllAsync();
        Task<Table?> GetByIdAsync(int id);
        Task<IEnumerable<Table>> GetByStateAsync(TableState state);
        Task<Table?> GetTableWithReservationsAsync(int id);
        Task UpdateStateAsync(int id, TableState newState);
        Task<Table> CreateAsync(Table table);
        Task UpdateAsync(int id, Table table);
        Task DeleteAsync(int id);
    }
}
