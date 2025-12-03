using ArcheryAcademy.Application.DTOs.ScheduleDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.ScheduleUseCase.Queries;

// Recibe una fecha de referencia (ej. hoy) y opcionalmente un instructor
public record GetWeeklyScheduleQuery(DateTime DateRef, Guid? InstructorId) : IRequest<List<ScheduleReadDto>>;

internal sealed class GetWeeklyScheduleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetWeeklyScheduleQuery, List<ScheduleReadDto>>
{
    public async Task<List<ScheduleReadDto>> Handle(GetWeeklyScheduleQuery request, CancellationToken cancellationToken)
    {
        // Calculamos inicio y fin de la semana (Lunes a Domingo) o rango mensual
        var startOfWeek = request.DateRef.Date; // O lógica para buscar el lunes previo
        var endOfWeek = startOfWeek.AddDays(7);
        // Usamos el Repo Específico que ya trae los Includes necesarios
        var schedules = await unitOfWork.ScheduleRepository.GetSchedulesWithOccupancyAsync(startOfWeek, endOfWeek, request.InstructorId);
        // El Mapper se encarga de restar MaxStudents - CurrentBookings
        return mapper.Map<List<ScheduleReadDto>>(schedules);
    }
}