using ArcheryAcademy.Application.DTOs.ReportDto;
using AutoMapper;

namespace ArcheryAcademy.Application.Mappings;

public class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {
        // Mapeo directo de propiedades (ya que se llaman igual en Domain y DTO)
        
        CreateMap<(int TotalToday, int TotalRange, Dictionary<int, int> StatusCounts), BookingStatsDto>()
            
            // 1. Mapeo de Totales directos
            .ForMember(dest => dest.TotalToday, opt => opt.MapFrom(src => src.TotalToday))
            .ForMember(dest => dest.TotalThisWeek, opt => opt.MapFrom(src => src.TotalRange))
            
            // 2. Mapeo del Diccionario (Extracción segura)
            // Asumimos los IDs: 1=Pending, 2=Confirmed, 3=Cancelled, 4=Completed
            .ForMember(dest => dest.Pending, opt => opt.MapFrom(src => src.StatusCounts.GetValueOrDefault(1, 0)))
            .ForMember(dest => dest.Confirmed, opt => opt.MapFrom(src => src.StatusCounts.GetValueOrDefault(2, 0)))
            .ForMember(dest => dest.Cancelled, opt => opt.MapFrom(src => src.StatusCounts.GetValueOrDefault(3, 0)))
            .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.StatusCounts.GetValueOrDefault(4, 0)));
    }
    
}