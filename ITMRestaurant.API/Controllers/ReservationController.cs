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
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public ReservationController(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetAll()
    {
        var reservations = await _reservationService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ReservationResponseDTO>>(reservations));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationResponseDTO>> GetById(int id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);

        if (reservation == null)
            return NotFound(new { message = $"Reservation with ID {id} not found." });

        var reservationDto = _mapper.Map<ReservationResponseDTO>(reservation);
        return Ok(reservationDto);
    }

    [HttpGet("state/{state}")]
    public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetByState(ReservationState state)
    {
        var reservations = await _reservationService.GetByStateAsync(state);
        var reservationsDto = _mapper.Map<IEnumerable<ReservationResponseDTO>>(reservations);
        return Ok(reservationsDto);
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var reservations = await _reservationService.GetByDateRangeAsync(startDate, endDate);
            var reservationsDto = _mapper.Map<IEnumerable<ReservationResponseDTO>>(reservations);
            return Ok(reservationsDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDTO>> Create(ReservationRequestDTO dto)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(dto);
            var createdReservation = await _reservationService.CreateAsync(reservation);

            // Recargar con detalles para el response
            var reservationWithDetails = await _reservationService.GetReservationWithDetailsAsync(createdReservation.Id);
            var responseDto = _mapper.Map<ReservationResponseDTO>(reservationWithDetails);

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
    public async Task<ActionResult> Update(int id, ReservationRequestDTO dto)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(dto);
            await _reservationService.UpdateAsync(id, reservation);
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
    public async Task<ActionResult> UpdateState(int id, [FromBody] ReservationState newState)
    {
        try
        {
            await _reservationService.UpdateStateAsync(id, newState);
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
            await _reservationService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
