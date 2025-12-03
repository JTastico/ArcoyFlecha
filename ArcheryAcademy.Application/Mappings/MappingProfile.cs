using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Application.Dtos.PaymentDto;
using ArcheryAcademy.Application.Dtos.PlanDto;
using ArcheryAcademy.Application.DTOs.RolDto;
using ArcheryAcademy.Application.DTOs.UserPlanDto;
using ArcheryAcademy.Application.DTOs.ScheduleDto;
using ArcheryAcademy.Application.DTOs.BookingStatusDto;
using ArcheryAcademy.Application.DTOs.PaymentMethodDto;
using ArcheryAcademy.Application.DTOs.PaymentStatusDto;
using ArcheryAcademy.Application.DTOs.UserRoleDto;
using ArcheryAcademy.Domain.Entities;
using AutoMapper;

namespace ArcheryAcademy.Application.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        //User_plan
        CreateMap<UserPlan, UserPlanReadDto>()
            // Mapeo especial: Sacar el nombre del Plan relacionado (Navegación)
            .ForMember(dest => dest.PlanName,
                opt => opt.MapFrom(src => src.Plan != null ? src.Plan.Name : "Plan Desconocido"))
            .ReverseMap();
        CreateMap<UserPlanCreateDto, UserPlan>().ReverseMap();
        CreateMap<UserPlanUpdateDto, UserPlan>().ReverseMap();
        
        //UserRole
        CreateMap<UserRole, UserRoleReadDto>().ReverseMap();
        CreateMap<UserRoleCreateDto, UserRole>().ReverseMap();
        CreateMap<UserRoleUpdateDto, UserRole>().ReverseMap();  

        //Schedule
        CreateMap<Schedule, ScheduleReadDto>().ReverseMap();
        CreateMap<ScheduleCreateDto, Schedule>().ReverseMap();
        CreateMap<ScheduleUpdateDto, Schedule>().ReverseMap();

        //Plan
        CreateMap<Plan, PlanReadDto>().ReverseMap();
        CreateMap<PlanCreateDto, Plan>().ReverseMap();
        CreateMap<PlanUpdateDto, Plan>().ReverseMap();

        //Role
        CreateMap<Role, RolCreateDto>().ReverseMap();
        CreateMap<Role, RolReadDto>().ReverseMap();
        CreateMap<Role, RolUpdateDto>().ReverseMap();

        //Booking
        CreateMap<Booking, BookingReadDto>().ReverseMap();
        CreateMap<BookingCreateDto, Booking>().ReverseMap();
        CreateMap<BookingUpdateDto, Booking>().ReverseMap();

        //BookingStatus
        CreateMap<BookingStatus, BookingStatusReadDto>().ReverseMap();
        CreateMap<BookingStatusCreateDto, BookingStatus>().ReverseMap();
        CreateMap<BookingStatusUpdateDto, BookingStatus>().ReverseMap();
        
        //Payment
        CreateMap<Payment, PaymentReadDto>()
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method != null ? src.Method.Name : null))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status != null ? src.Status.Name : null));
        CreateMap<PaymentCreateDto, Payment>().ReverseMap();
        CreateMap<PaymentUpdateDto, Payment>().ReverseMap();

        //PaymentMethod
        CreateMap<PaymentMethod, PaymentMethodReadDto>().ReverseMap();
        CreateMap<PaymentMethodCreateDto, PaymentMethod>().ReverseMap();
        CreateMap<PaymentMethodUpdateDto, PaymentMethod>().ReverseMap();
        
        //PaymentStatus
        CreateMap<PaymentStatus, PaymentStatusReadDto>().ReverseMap();
        CreateMap<PaymentStatusCreateDto, PaymentStatus>().ReverseMap();
        CreateMap<PaymentStatusUpdateDto, PaymentStatus>().ReverseMap();
    }
}