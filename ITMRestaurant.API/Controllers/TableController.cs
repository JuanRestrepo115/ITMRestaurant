using AutoMapper;
using ITMRestaurant.API.DTOs.Request;
using ITMRestaurant.API.DTOs.Response;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITMRestaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{
    private readonly ITableService _tableService;
    private readonly IMapper _mapper;

    public TableController(ITableService tableService, IMapper mapper)
    {
        _tableService = tableService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TableResponseDTO>>> GetAll()
    {
        var tables = await _tableService.GetAllAsync();
        var tablesDto = _mapper.Map<IEnumerable<TableResponseDTO>>(tables);
        return Ok(tablesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TableResponseDTO>> GetById(int id)
    {
        var table = await _tableService.GetByIdAsync(id);

        if (table == null)
            return NotFound(new { message = $"Table with ID {id} not found." });

        var tableDto = _mapper.Map<TableResponseDTO>(table);
        return Ok(tableDto);
    }

    [HttpGet("state/{state}")]
    public async Task<ActionResult<IEnumerable<TableResponseDTO>>> GetByState(TableState state)
    {
        var tables = await _tableService.GetByStateAsync(state);
        var tablesDto = _mapper.Map<IEnumerable<TableResponseDTO>>(tables);
        return Ok(tablesDto);
    }

    [HttpGet("{id}/reservations")]
    public async Task<ActionResult<TableResponseDTO>> GetWithReservations(int id)
    {
        try
        {
            var table = await _tableService.GetTableWithReservationsAsync(id);
            var tableDto = _mapper.Map<TableResponseDTO>(table);
            return Ok(tableDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TableResponseDTO>> Create(TableRequestDTO dto)
    {
        try
        {
            var table = _mapper.Map<Table>(dto);
            var createdTable = await _tableService.CreateAsync(table);
            var responseDto = _mapper.Map<TableResponseDTO>(createdTable);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
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

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TableRequestDTO dto)
    {
        try
        {
            var table = _mapper.Map<Table>(dto);
            await _tableService.UpdateAsync(id, table);
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

    [HttpPatch("{id}/state")]
    public async Task<ActionResult> UpdateState(int id, [FromBody] TableState newState)
    {
        try
        {
            await _tableService.UpdateStateAsync(id, newState);
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
            await _tableService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}