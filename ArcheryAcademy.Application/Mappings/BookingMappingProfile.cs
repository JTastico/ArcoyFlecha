using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Domain.Entities;
using AutoMapper;

namespace ArcheryAcademy.Application.Mappings;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        // ---------- READ DTO ----------
        CreateMap<Booking, BookingReadDto>()
            // 1. Nombre del estudiante
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                src.User != null
                    ? $"{src.User.FirstName} {src.User.LastName}"
                    : "Usuario Desconocido"
            ))
            // 2. Información del horario
            .ForMember(dest => dest.ScheduleInfo, opt => opt.MapFrom(src =>
                src.Schedule != null
                    ? $"{src.Schedule.StartTime:dd/MM/yyyy HH:mm}"
                    : "Sin Horario"
            ))
            // 3. Estado de la reserva
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status != null
                    ? src.Status.Name
                    : "Desconocido"
            ))
            // 4. Estado del pago
            .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src =>
                src.PaymentStatus != null
                    ? src.PaymentStatus.Name
                    : "Desconocido"
            ));
        // ---------- CREATE DTO ----------
        // Aquí SÍ puedes usar ReverseMap si lo deseas
        CreateMap<BookingCreateDto, Booking>().ReverseMap();

        // ---------- UPDATE DTO (si es que existe) ----------
        CreateMap<BookingUpdateDto, Booking>().ReverseMap();
    }
}