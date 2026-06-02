using ITMRestaurant.Domain.Entities;
namespace ITMRestaurant.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<IEnumerable<Customer>> GetCustomersWithReservationsAsync();
        Task <bool> ExistsByEmailAsync(string email);
        

    }
}
