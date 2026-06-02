using AutoMapper;
using ITMRestaurant.API.DTOs.Request;
using ITMRestaurant.API.DTOs.Response;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ITMRestaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationDetailController : ControllerBase
{
    private readonly IGenericRepository<ReservationDetail> _reservationDetailRepository;
    private readonly IMapper _mapper;

    public ReservationDetailController(
        IGenericRepository<ReservationDetail> reservationDetailRepository,
        IMapper mapper)
    {
        _reservationDetailRepository = reservationDetailRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDetailResponseDTO>>> GetAll()
    {
        var details = await _reservationDetailRepository.GetAllAsync();
        var detailsDto = _mapper.Map<IEnumerable<ReservationDetailResponseDTO>>(details);
        return Ok(detailsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDetailResponseDTO>> GetById(int id)
    {
        var detail = await _reservationDetailRepository.GetByIdAsync(id);

        if (detail == null)
            return NotFound(new { message = $"Reservation detail with ID {id} not found." });

        var detailDto = _mapper.Map<ReservationDetailResponseDTO>(detail);
        return Ok(detailDto);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDetailResponseDTO>> Create(ReservationDetailRequestDTO dto)
    {
        try
        {
            var detail = _mapper.Map<ReservationDetail>(dto);
            var createdDetail = await _reservationDetailRepository.CreateAsync(detail);
            var responseDto = _mapper.Map<ReservationDetailResponseDTO>(createdDetail);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, ReservationDetailRequestDTO dto)
    {
        try
        {
            var existingDetail = await _reservationDetailRepository.GetByIdAsync(id);
            if (existingDetail == null)
                return NotFound(new { message = $"Reservation detail with ID {id} not found." });

            existingDetail.Quantity = dto.Quantity;
            existingDetail.UnitPrice = dto.UnitPrice;
            existingDetail.MenuItemId = dto.MenuItemId;
            existingDetail.ReservationId = dto.ReservationId;

            await _reservationDetailRepository.UpdateAsync(existingDetail);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var exists = await _reservationDetailRepository.ExistsAsync(id);
            if (!exists)
                return NotFound(new { message = $"Reservation detail with ID {id} not found." });

            await _reservationDetailRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}