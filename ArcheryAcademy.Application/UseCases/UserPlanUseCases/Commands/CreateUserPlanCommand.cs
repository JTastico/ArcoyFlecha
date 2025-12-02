using ArcheryAcademy.Application.DTOs.UserPlanDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserPlanUseCases.Commands;

public record CreateUserPlanCommand(UserPlanCreateDto CreateDto) : IRequest<UserPlanReadDto>;

internal sealed class CreateUserPlanCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateUserPlanCommand, UserPlanReadDto>
{
    public async Task<UserPlanReadDto> Handle(CreateUserPlanCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CreateDto;
        var startDate = dto.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        // 1. VALIDACIÓN: ¿Ya tiene plan activo? (Usando tu repo específico)
        var hasActive = await unitOfWork.UserPlanRepository.HasActivePlanAsync(dto.UserId);
        if (hasActive)
        {
            throw new InvalidOperationException("El usuario ya tiene un plan activo.");
        }

        // 2. OBTENER DATOS DEL PLAN BASE (Precio, Duración, Clases)
        var planBase = await unitOfWork.Repository<Plan>().GetByIdAsync(dto.PlanId);
        if (planBase == null) throw new KeyNotFoundException("El plan seleccionado no existe.");

        // 3. LÓGICA DE NEGOCIO: Calcular fechas y clases
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        var newUserPlan = new UserPlan
        {
            UserId = dto.UserId,
            PlanId = dto.PlanId,
            StartDate = startDate,
            // Calculamos EndDate: Hoy + Días del plan
            EndDate = today.AddDays(planBase.DurationDays ?? 30),
            // Asignamos clases: Las que diga el plan
            RemainingClasses = planBase.NumClasses,
            IsActive = true
        };

        // 4. INSERTAR
        await unitOfWork.UserPlanRepository.Insert(newUserPlan);
        await unitOfWork.CompleteAsync(cancellationToken);

        // 5. RESPUESTA
        newUserPlan.Plan = planBase; 
        return mapper.Map<UserPlanReadDto>(newUserPlan);
    }
}