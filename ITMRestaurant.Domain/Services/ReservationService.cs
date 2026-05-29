using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;
using ITMRestaurant.Domain.Interfaces.Repositories;
using ITMRestaurant.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ITMRestaurant.Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            IReservationRepository reservationRepository,
            ICustomerRepository customerRepository,
            ITableRepository tableRepository,
            IRestaurantRepository restaurantRepository,
            ILogger<ReservationService> logger)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _tableRepository = tableRepository;
            _restaurantRepository = restaurantRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all reservations");
            return await _reservationRepository.GetAllAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving reservation with ID: {Id}", id);
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation == null)
                _logger.LogWarning("Reservation with ID {Id} not found", id);

            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetByStateAsync(ReservationState state)
        {
            _logger.LogInformation("Retrieving reservations by state: {State}", state);
            return await _reservationRepository.GetByStateAsync(state);
        }

        public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            // Validacion de negocio
            if (startDate > endDate)
            {
                _logger.LogWarning("Invalid date range: {StartDate} > {EndDate}", startDate, endDate);
                throw new InvalidOperationException("Start date cannot be greater than end date.");
            }

            _logger.LogInformation("Retrieving reservations from {StartDate} to {EndDate}", startDate, endDate);
            return await _reservationRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task<Reservation?> GetReservationWithDetailsAsync(int id)
        {
            _logger.LogInformation("Retrieving reservation with details for ID: {Id}", id);
            var reservation = await _reservationRepository.GetReservationWithDetailsAsync(id);

            if (reservation == null)
            {
                _logger.LogWarning("Reservation with ID {Id} not found", id);
                throw new KeyNotFoundException($"Reservation with ID {id} not found.");
            }

            return reservation;
        }

        public async Task<Reservation> CreateAsync(Reservation reservation)
        {
            // 1ra V: Validar que el cliente existe
            var customerExists = await _customerRepository.ExistsAsync(reservation.CustomerId);
            if (!customerExists)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found", reservation.CustomerId);
                throw new KeyNotFoundException($"Customer with ID {reservation.CustomerId} not found.");
            }

            // 2da V: Validar que el restaurante existe
            var restaurantExists = await _restaurantRepository.ExistsAsync(reservation.RestaurantId);
            if (!restaurantExists)
            {
                _logger.LogWarning("Restaurant with ID {RestaurantId} not found", reservation.RestaurantId);
                throw new KeyNotFoundException($"Restaurant with ID {reservation.RestaurantId} not found.");
            }

            // 3ra V: Validar que la mesa existe
            var table = await _tableRepository.GetByIdAsync(reservation.TableId);
            if (table == null)
            {
                _logger.LogWarning("Table with ID {TableId} not found", reservation.TableId);
                throw new KeyNotFoundException($"Table with ID {reservation.TableId} not found.");
            }

            // 4ta V:  Validar que la mesa este disponible
            if (table.State != TableState.Available)
            {
                _logger.LogWarning("Table with ID {TableId} is not available", reservation.TableId);
                throw new InvalidOperationException($"Table with ID {reservation.TableId} is not available.");
            }

            // 5ta V: Validar que la fecha de reserva sea futura
            if (reservation.ReservationTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Reservation time {ReservationTime} is not in the future", reservation.ReservationTime);
                throw new InvalidOperationException("Reservation time must be in the future.");
            }

            // 6ta v: Validar que el numero de personas no supere la capacidad de la mesa
            if (reservation.NumberOfGuests > table.Capacity)
            {
                _logger.LogWarning("Number of guests {NumberOfGuests} exceeds table capacity {Capacity}", reservation.NumberOfGuests, table.Capacity);
                throw new InvalidOperationException($"Number of guests exceeds table capacity of {table.Capacity}.");
            }

            // 7ta Validar que haya al menos 1 persona
            if (reservation.NumberOfGuests <= 0)
            {
                _logger.LogWarning("Invalid number of guests: {NumberOfGuests}", reservation.NumberOfGuests);
                throw new InvalidOperationException("Number of guests must be at least 1.");
            }

            // Cambiar estado de la mesa a Reserved
            await _tableRepository.UpdateStateAsync(reservation.TableId, TableState.Reserved);
            _logger.LogInformation("Table with ID {TableId} state changed to Reserved", reservation.TableId);

            _logger.LogInformation("Creating reservation for customer ID: {CustomerId}", reservation.CustomerId);
            return await _reservationRepository.CreateAsync(reservation);
        }

        public async Task UpdateAsync(int id, Reservation reservation)
        {
            var existingReservation = await _reservationRepository.GetByIdAsync(id);
            if (existingReservation == null)
            {
                _logger.LogWarning("Attempt to update a non-existing reservation with ID: {Id}", id);
                throw new KeyNotFoundException($"Reservation with ID {id} not found.");
            }

            // No se puede modificar una reserva cancelada o completada
            if (existingReservation.State == ReservationState.Cancelled ||
                existingReservation.State == ReservationState.Completed)
            {
                _logger.LogWarning("Attempt to update a {State} reservation with ID: {Id}", existingReservation.State, id);
                throw new InvalidOperationException($"Cannot modify a {existingReservation.State} reservation.");
            }

            // Validar fecha futura si cambio
            if (existingReservation.ReservationTime != reservation.ReservationTime)
            {
                if (reservation.ReservationTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Reservation time {ReservationTime} is not in the future", reservation.ReservationTime);
                    throw new InvalidOperationException("Reservation time must be in the future.");
                }
            }

            // Validar mesa si cambio
            if (existingReservation.TableId != reservation.TableId)
            {
                var table = await _tableRepository.GetByIdAsync(reservation.TableId);
                if (table == null)
                {
                    _logger.LogWarning("Table with ID {TableId} not found", reservation.TableId);
                    throw new KeyNotFoundException($"Table with ID {reservation.TableId} not found.");
                }

                if (table.State != TableState.Available)
                {
                    _logger.LogWarning("Table with ID {TableId} is not available", reservation.TableId);
                    throw new InvalidOperationException($"Table with ID {reservation.TableId} is not available.");
                }

                if (reservation.NumberOfGuests > table.Capacity)
                {
                    _logger.LogWarning("Number of guests {NumberOfGuests} exceeds table capacity {Capacity}", reservation.NumberOfGuests, table.Capacity);
                    throw new InvalidOperationException($"Number of guests exceeds table capacity of {table.Capacity}.");
                }
            }

            existingReservation.CustomerId = reservation.CustomerId;
            existingReservation.TableId = reservation.TableId;
            existingReservation.RestaurantId = reservation.RestaurantId;
            existingReservation.ReservationTime = reservation.ReservationTime;
            existingReservation.NumberOfGuests = reservation.NumberOfGuests;
            existingReservation.Observations = reservation.Observations;
            existingReservation.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating reservation with ID: {Id}", id);
            await _reservationRepository.UpdateAsync(existingReservation);
        }

        public async Task UpdateStateAsync(int id, ReservationState newState)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                _logger.LogWarning("Attempt to update state of a non-existing reservation with ID: {Id}", id);
                throw new KeyNotFoundException($"Reservation with ID {id} not found.");
            }

            // No se puede cambiar el estado de una reserva cancelada o completada
            if (reservation.State == ReservationState.Cancelled ||
                reservation.State == ReservationState.Completed)
            {
                _logger.LogWarning("Attempt to update state of a {State} reservation with ID: {Id}", reservation.State, id);
                throw new InvalidOperationException($"Cannot change state of a {reservation.State} reservation.");
            }

            // Si se cancela o completa la reserva, liberar la mesa
            if (newState == ReservationState.Cancelled || newState == ReservationState.Completed)
            {
                await _tableRepository.UpdateStateAsync(reservation.TableId, TableState.Available);
                _logger.LogInformation("Table with ID {TableId} state changed back to Available", reservation.TableId);
            }

            _logger.LogInformation("Updating state of reservation with ID: {Id} to {State}", id, newState);
            await _reservationRepository.UpdateStateAsync(id, newState);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _reservationRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Attempt to delete a non-existing reservation with ID: {Id}", id);
                throw new KeyNotFoundException($"Reservation with ID {id} not found.");
            }

            _logger.LogInformation("Deleting reservation with ID: {Id}", id);
            await _reservationRepository.DeleteAsync(id);
        }
    }
}
