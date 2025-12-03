using ArcheryAcademy.Application.DTOs.ScheduleDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.ScheduleUseCase.Commands;

public record CreateScheduleCommand(ScheduleCreateDto ScheduleDto) 
    : IRequest<ScheduleReadDto>;

internal sealed class CreateScheduleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateScheduleCommand, ScheduleReadDto>
{
    public async Task<ScheduleReadDto> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.ScheduleDto;
        
        // 1️ VALIDACIÓN: Horario coherente
        if (dto.StartTime >= dto.EndTime)
            throw new ArgumentException("La hora de inicio debe ser anterior a la hora fin.");
        
        // 2️ VALIDACIÓN: Solapamientos del instructor
        var hasOverlap = await unitOfWork.ScheduleRepository
            .HasOverlapAsync(dto.InstructorId, dto.StartTime, dto.EndTime);

        if (hasOverlap)
            throw new InvalidOperationException(
                "El instructor ya tiene una clase programada en ese rango de horario."
            );
        
        // 3️ VALIDACIÓN: Instructor existente
        // (opcional pero recomendado, depende si HasOverlap ya valida existencia)
        var instructor = await unitOfWork.Repository<User>()
            .GetByIdAsync(dto.InstructorId);

        if (instructor == null)
        {
            throw new KeyNotFoundException(
                $"Instructor with ID {dto.InstructorId} not found."
            );
        }

        // 4️ CREACIÓN de entidad Schedule
        var schedule = mapper.Map<Schedule>(dto);
        schedule.IsActive = true;
        await unitOfWork.ScheduleRepository.Insert(schedule); // Repo específico
        await unitOfWork.CompleteAsync(cancellationToken);

        // RETORNAMOS EL DTO COMPLETO
        return mapper.Map<ScheduleReadDto>(schedule);
    }
}