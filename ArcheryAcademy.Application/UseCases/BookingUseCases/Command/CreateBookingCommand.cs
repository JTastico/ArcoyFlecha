using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Command;

public record CreateBookingCommand(BookingCreateDto Dto) : IRequest<BookingReadDto>;

internal sealed class CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateBookingCommand, BookingReadDto>
{
    public async Task<BookingReadDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // 1. VALIDAR SI EL ALUMNO YA ESTÁ INSCRITO (Evitar duplicados accidentales)
        var isBooked = await unitOfWork.BookingRepository.IsUserAlreadyBookedAsync(dto.UserId, dto.ScheduleId);
        if (isBooked) throw new InvalidOperationException("Este alumno ya está inscrito en esta clase.");

        // 2. VALIDAR CUPOS (El sistema avisa al Admin si la clase está llena)
        var schedule = await unitOfWork.ScheduleRepository.GetByIdAsync(dto.ScheduleId);
        if (schedule == null) throw new KeyNotFoundException("El horario seleccionado no existe.");
        
        var currentCount = await unitOfWork.BookingRepository.CountActiveBookingsAsync(dto.ScheduleId);
        if (currentCount >= schedule.MaxStudents)
        {
            throw new InvalidOperationException($"La clase está llena ({currentCount}/{schedule.MaxStudents}).");
        }

        // 3. OBTENER PLAN ACTIVO DEL ALUMNO (Requisito de la BD: user_plan_id NOT NULL)
        var activePlan = await unitOfWork.UserPlanRepository.GetCurrentActivePlanAsync(dto.UserId);

        if (activePlan == null)
            throw new InvalidOperationException("El alumno no tiene un plan activo. Asígnele un plan primero.");

        if (activePlan.RemainingClasses <= 0)
            throw new InvalidOperationException("El alumno se ha quedado sin clases en su plan.");

        // Validar fecha (DateOnly vs DateOnly)
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (activePlan.EndDate < today)
            throw new InvalidOperationException("El plan del alumno ha expirado.");

        // 4. CREAR LA RESERVA
        var booking = new Booking
        {
            UserId = dto.UserId,
            ScheduleId = dto.ScheduleId,
            UserPlanId = activePlan.Id, // Obligatorio según tu BD
            
            // Lógica de Admin: Si el Admin lo crea, nace Confirmada y Pagada
            StatusId = 2,        // 2 = Confirmed (según tus INSERTS SQL)
            PaymentStatusId = 2, // 2 = Paid (según tus INSERTS SQL)
            
            CreatedAt = DateTime.UtcNow
        };

        // 5. DESCONTAR LA CLASE DEL PLAN
        activePlan.RemainingClasses -= 1;

        // 6. GUARDAR TRANSACCIÓN
        await unitOfWork.BookingRepository.Insert(booking);
        await unitOfWork.UserPlanRepository.UpdateAsync(activePlan);
        await unitOfWork.CompleteAsync(cancellationToken);

        // Cargar relaciones para devolver datos completos al Admin
        booking.Schedule = schedule;
        booking.User = activePlan.User ?? await unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);

        return mapper.Map<BookingReadDto>(booking);
    }
}
