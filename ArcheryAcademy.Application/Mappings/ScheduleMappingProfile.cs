using ArcheryAcademy.Application.DTOs.ScheduleDto;
using ArcheryAcademy.Domain.Entities;
using AutoMapper;

namespace ArcheryAcademy.Application.Mappings;

public class ScheduleMappingProfile : Profile
{
    public ScheduleMappingProfile()
    {
        CreateMap<Schedule, ScheduleReadDto>()
            .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? $"{src.Instructor.FirstName} {src.Instructor.LastName}" : "Sin asignar"))

            .ForMember(dest => dest.CurrentBookings, opt => opt.MapFrom(src => 
                src.Bookings != null ? src.Bookings.Count(b => b.StatusId == 1 || b.StatusId == 2) : 0)) // 1=Pending, 2=Confirmed segun tu script
            .ForMember(dest => dest.AvailableSpots, opt => opt.MapFrom(src => 
                src.MaxStudents - (src.Bookings != null ? src.Bookings.Count(b => b.StatusId == 1 || b.StatusId == 2) : 0)));
        CreateMap<ScheduleCreateDto, Schedule>().ReverseMap();
        CreateMap<ScheduleUpdateDto, Schedule>().ReverseMap();
    }
}