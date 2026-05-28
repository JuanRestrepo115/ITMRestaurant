using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
namespace ITMRestaurant.Domain.Interfaces.Repositories
{
    public interface ITableRepository : IGenericRepository<Table>
    {
        Task<IEnumerable<Table>> GetByStateAsync(TableState state);
        Task<Table?> GetByTableNumberAsync(int tableNumber);
        Task<Table?> GetTableWithReservationsAsync(int id);
        Task UpdateStateAsync(int id, TableState newState);

    }
}
