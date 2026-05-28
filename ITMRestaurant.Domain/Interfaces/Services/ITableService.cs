using ITMRestaurant.Domain.Entities;
namespace ITMRestaurant.Domain.Interfaces.Services
{
    public interface ITableService
    {
        Task <IEnumerable<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(int id);
        Task<Table> CreateAsync(Table table);
        Task UpdateAsync(int id, Table table);
        Task DeleteAsync(int id);
    }
}
