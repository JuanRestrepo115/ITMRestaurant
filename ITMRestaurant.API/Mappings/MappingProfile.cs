using AutoMapper;
using ITMRestaurant.API.DTOs.Request;
using ITMRestaurant.API.DTOs.Response;
using ITMRestaurant.Domain.Entities;
using ITMRestaurant.Domain.Enums;

namespace ITMRestaurant.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ── Customer ──────────────────────────────────────────
            CreateMap<CustomerRequestDTO, Customer>();
            CreateMap<Customer, CustomerResponseDTO>();

            // ── Restaurant ────────────────────────────────────────
            CreateMap<RestaurantRequestDTO, Restaurant>()
                .ForMember(dest => dest.IsActive,
                           opt => opt.MapFrom(src => true));
            CreateMap<Restaurant, RestaurantResponseDTO>();

            // ── Table ─────────────────────────────────────────────
            CreateMap<TableRequestDTO, Table>()
                .ForMember(dest => dest.State,
                           opt => opt.MapFrom(src => TableState.Available));
            CreateMap<Table, TableResponseDTO>()
                .ForMember(dest => dest.RestaurantBranch,
                           opt => opt.MapFrom(src => src.Restaurant.Branch));

            // ── MenuItem ──────────────────────────────────────────
            CreateMap<MenuItemRequestDTO, MenuItem>();
            CreateMap<MenuItem, MenuItemResponseDTO>();

            // ── Reservation ───────────────────────────────────────
            CreateMap<ReservationRequestDTO, Reservation>()
                .ForMember(dest => dest.State,
                           opt => opt.MapFrom(src => ReservationState.Pending));
            CreateMap<Reservation, ReservationResponseDTO>()
                .ForMember(dest => dest.CustomerFullName,
                           opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
                .ForMember(dest => dest.TableNumber,
                           opt => opt.MapFrom(src => src.Table.TableNumber));


            // ── ReservationDetail ─────────────────────────────────
            CreateMap<ReservationDetailRequestDTO, ReservationDetail>();
            CreateMap<ReservationDetail, ReservationDetailResponseDTO>();
        }
    }
}