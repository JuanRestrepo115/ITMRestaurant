using ITMRestaurant.Domain.Entities;
namespace ITMRestaurant.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer> GetCustomerWithReservationsAsync(int id);
        Task <bool> ExistsByEmailAsync(string email);
    }
}
