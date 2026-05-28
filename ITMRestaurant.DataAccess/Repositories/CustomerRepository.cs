using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
namespace ITMRestaurant.DataAccess.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(x => x.Email == email);
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            var customer = await _dbSet.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
            if (customer == null)   
                throw new InvalidOperationException($"Customer with email '{email}' not found.");
            return customer;
        }

        public async Task<Customer> GetCustomerWithReservationsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}
