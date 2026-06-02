using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ITMRestaurant.API.DTOs.Request;
using ITMRestaurant.API.DTOs.Response;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Interfaces.Services;

namespace ITMRestaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomerController(ICustomerService customerService, IMapper mapper)
    {
        _customerService = customerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDTO>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        var customersDto = _mapper.Map<IEnumerable<CustomerResponseDTO>>(customers);
        return Ok(customersDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
            return NotFound(new { message = $"Customer with ID {id} not found." });

        var customerDto = _mapper.Map<CustomerResponseDTO>(customer);
        return Ok(customerDto);
    }

    [HttpGet("with-reservations")]
    public async Task<ActionResult<IEnumerable<CustomerResponseDTO>>> GetWithReservations()
    {
        var customers = await _customerService.GetCustomersWithReservationsAsync();
        var customersDto = _mapper.Map<IEnumerable<CustomerResponseDTO>>(customers);
        return Ok(customersDto);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDTO>> Create(CustomerRequestDTO dto)
    {
        try
        {
            var customer = _mapper.Map<Customer>(dto);
            var createdCustomer = await _customerService.CreateAsync(customer);
            var responseDto = _mapper.Map<CustomerResponseDTO>(createdCustomer);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CustomerRequestDTO dto)
    {
        try
        {
            var customer = _mapper.Map<Customer>(dto);
            await _customerService.UpdateAsync(id, customer);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _customerService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}