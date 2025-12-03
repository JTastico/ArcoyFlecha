using ArcheryAcademy.Application.DTOs.PaymentStatusDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentStatusUseCases.Queries;

public record GetPaymentStatusByIdQuery(int Id) : IRequest<PaymentStatusReadDto?>;

internal sealed class GetPaymentStatusByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPaymentStatusByIdQuery, PaymentStatusReadDto?>
{
    public async Task<PaymentStatusReadDto?> Handle(GetPaymentStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.Repository<PaymentStatus>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return entity == null ? null : mapper.Map<PaymentStatusReadDto>(entity);
    }
}