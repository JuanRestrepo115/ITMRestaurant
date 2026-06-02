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

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithReservationsAsync()
        {
            return await _dbSet
                .Include(c => c.Reservations)
                .Where(c => c.Reservations.Any())
                .ToListAsync();
        }

    }
}
