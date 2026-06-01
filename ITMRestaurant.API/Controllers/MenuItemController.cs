using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ITMRestaurant.API.DTOs.Request;
using ITMRestaurant.API.DTOs.Response;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Services;

namespace ITMRestaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;
    private readonly IMapper _mapper;

    public MenuItemController(IMenuItemService menuItemService, IMapper mapper)
    {
        _menuItemService = menuItemService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemResponseDTO>>> GetAll()
    {
        var menuItems = await _menuItemService.GetAllAsync();
        var menuItemsDto = _mapper.Map<IEnumerable<MenuItemResponseDTO>>(menuItems);
        return Ok(menuItemsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemResponseDTO>> GetById(int id)
    {
        var menuItem = await _menuItemService.GetByIdAsync(id);

        if (menuItem == null)
            return NotFound(new { message = $"Menu item with ID {id} not found." });

        var menuItemDto = _mapper.Map<MenuItemResponseDTO>(menuItem);
        return Ok(menuItemDto);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<MenuItemResponseDTO>>> GetAvailable()
    {
        var menuItems = await _menuItemService.GetAvailableItemsAsync();
        var menuItemsDto = _mapper.Map<IEnumerable<MenuItemResponseDTO>>(menuItems);
        return Ok(menuItemsDto);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<MenuItemResponseDTO>>> GetByCategory(MenuCategory category)
    {
        var menuItems = await _menuItemService.GetByCategoryAsync(category);
        var menuItemsDto = _mapper.Map<IEnumerable<MenuItemResponseDTO>>(menuItems);
        return Ok(menuItemsDto);
    }

    [HttpGet("price-range")]
    public async Task<ActionResult<IEnumerable<MenuItemResponseDTO>>> GetByPriceRange(
        [FromQuery] decimal minPrice,
        [FromQuery] decimal maxPrice)
    {
        try
        {
            var menuItems = await _menuItemService.GetByPriceRangeAsync(minPrice, maxPrice);
            var menuItemsDto = _mapper.Map<IEnumerable<MenuItemResponseDTO>>(menuItems);
            return Ok(menuItemsDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemResponseDTO>> Create(MenuItemRequestDTO dto)
    {
        try
        {
            var menuItem = _mapper.Map<MenuItem>(dto);
            var createdMenuItem = await _menuItemService.CreateAsync(menuItem);
            var responseDto = _mapper.Map<MenuItemResponseDTO>(createdMenuItem);

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
    public async Task<ActionResult> Update(int id, MenuItemRequestDTO dto)
    {
        try
        {
            var menuItem = _mapper.Map<MenuItem>(dto);
            await _menuItemService.UpdateAsync(id, menuItem);
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

    [HttpPatch("{id}/availability")]
    public async Task<ActionResult> UpdateAvailability(int id, [FromBody] bool isAvailable)
    {
        try
        {
            await _menuItemService.UpdateAvailabilityAsync(id, isAvailable);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _menuItemService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}