using ITMRestaurant.Domain.Entities;
namespace ITMRestaurant.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> CreateAsync(Customer customer);
        Task UpdateAsync(int id, Customer customer);
        Task DeleteAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersWithReservationsAsync();
    }
}
