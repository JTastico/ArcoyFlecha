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
        
        CreateMap<(string Info, int Max, int Current), ClassOccupancyDto>()
            .ForMember(dest => dest.ScheduleInfo, opt => opt.MapFrom(src => src.Info))
            .ForMember(dest => dest.MaxCapacity, opt => opt.MapFrom(src => src.Max))
            .ForMember(dest => dest.CurrentBookings, opt => opt.MapFrom(src => src.Current))
            // Calculamos el porcentaje visual aquí (protegiendo división por cero)
            .ForMember(dest => dest.OccupancyPercentage, opt => opt.MapFrom(src => 
                src.Max > 0 ? Math.Round(((double)src.Current / src.Max) * 100, 2) : 0));
        // Origen: Tupla (Active, Expired, ByType) -> Destino: PlanStatsDto
        CreateMap<(int Active, int Expired, Dictionary<string, int> ByType), PlanStatsDto>()
            .ForMember(dest => dest.TotalActive, opt => opt.MapFrom(src => src.Active))
            .ForMember(dest => dest.TotalExpired, opt => opt.MapFrom(src => src.Expired))
            .ForMember(dest => dest.ByType, opt => opt.MapFrom(src => src.ByType));
        // Origen: Tupla (Total, New, Active) -> Destino: UserStatsDto
        CreateMap<(int Total, int New, int Active), UserStatsDto>()
            .ForMember(dest => dest.TotalUsers, opt => opt.MapFrom(src => src.Total))
            .ForMember(dest => dest.NewUsersThisMonth, opt => opt.MapFrom(src => src.New))
            .ForMember(dest => dest.ActiveStudents, opt => opt.MapFrom(src => src.Active));
    }
    
}