using ITMRestaurant.Domain.Interfaces.Services;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
namespace ITMRestaurant.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        protected readonly ICustomerRepository _customerRepository;

        protected readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }


        public async Task<Customer> CreateAsync(Customer customer)
        {
            //Validacion de Negocio que el Email sea Unico
            var ExistingCustomer = await _customerRepository.GetByEmailAsync(customer.Email);
            if (ExistingCustomer != null) {
                _logger.LogWarning("Attempt to create a customer with an existing email: {Email}", customer.Email);
                throw new InvalidOperationException("A customer with the specified email already exists.");
            }

            _logger.LogInformation("Creating a new customer with email: {Email}", customer.Email);
            return await _customerRepository.CreateAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            //Verificar que exista el cliente antes de intentar eliminarlo
            var exists = await _customerRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Attempt to delete a non-existing customer with ID: {Id}", id);
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            _logger.LogInformation("Deleting customer with ID: {Id}", id);
            await _customerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all customers");
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving customer with ID: {Id}", id);
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null) {
                _logger.LogWarning("Customer with ID: {Id} not found", id);
            }
            return customer;
        }

        public async Task UpdateAsync(int id, Customer customer)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            //Validar si el cliente existe antes de intentar actualizarlo
            if (existingCustomer == null)
            {
                _logger.LogWarning("Attempt to update a non-existing customer with ID: {Id}", id);
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            //Validar correo unico (Si cambio)
            if (existingCustomer.Email != customer.Email)
            {
                var emailOwner = await _customerRepository.GetByEmailAsync(customer.Email);
                if (emailOwner != null)
                {
                    _logger.LogWarning("Attempt to update customer with an email that already exists: {Email}", customer.Email);
                    throw new InvalidOperationException("A customer with the specified email already exists.");
                }
            }


            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating customer with ID: {Id}", id);
            await _customerRepository.UpdateAsync(existingCustomer);


        }
    }
}
