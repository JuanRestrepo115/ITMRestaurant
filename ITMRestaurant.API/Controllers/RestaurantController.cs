using AutoMapper;
using ITMRestaurant.API.DTOs.Request;
using ITMRestaurant.API.DTOs.Response;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITMRestaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    private readonly IMapper _mapper;

    public RestaurantController(IRestaurantService restaurantService, IMapper mapper)
    {
        _restaurantService = restaurantService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantResponseDTO>>> GetAll()
    {
        var restaurants = await _restaurantService.GetAllAsync();
        var restaurantsDto = _mapper.Map<IEnumerable<RestaurantResponseDTO>>(restaurants);
        return Ok(restaurantsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantResponseDTO>> GetById(int id)
    {
        var restaurant = await _restaurantService.GetByIdAsync(id);

        if (restaurant == null)
            return NotFound(new { message = $"Restaurant with ID {id} not found." });

        var restaurantDto = _mapper.Map<RestaurantResponseDTO>(restaurant);
        return Ok(restaurantDto);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<RestaurantResponseDTO>>> GetActive()
    {
        var restaurants = await _restaurantService.GetActiveRestaurantsAsync();
        var restaurantsDto = _mapper.Map<IEnumerable<RestaurantResponseDTO>>(restaurants);
        return Ok(restaurantsDto);
    }

    [HttpPost]
    public async Task<ActionResult<RestaurantResponseDTO>> Create(RestaurantRequestDTO dto)
    {
        try
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            var createdRestaurant = await _restaurantService.CreateAsync(restaurant);
            var responseDto = _mapper.Map<RestaurantResponseDTO>(createdRestaurant);

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
    public async Task<ActionResult> Update(int id, RestaurantRequestDTO dto)
    {
        try
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            await _restaurantService.UpdateAsync(id, restaurant);
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

    [HttpPatch("{id}/active")]
    public async Task<ActionResult> UpdateIsActive(int id, [FromBody] bool isActive)
    {
        try
        {
            await _restaurantService.UpdateIsActiveAsync(id, isActive);
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
            await _restaurantService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}