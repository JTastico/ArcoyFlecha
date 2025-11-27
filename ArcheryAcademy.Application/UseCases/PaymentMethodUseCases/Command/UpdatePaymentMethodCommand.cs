using ArcheryAcademy.Application.DTOs.PaymentMethodDto;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Command;

public record UpdatePaymentMethodCommand(int Id, PaymentMethodUpdateDto PaymentMethodDto) : IRequest<PaymentMethod?>;

internal sealed class UpdatePaymentMethodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdatePaymentMethodCommand, PaymentMethod?>
{
    public async Task<PaymentMethod?> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Repository<PaymentMethod>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (existing is null)
            return null;

        mapper.Map(request.PaymentMethodDto, existing);

        await unitOfWork.Repository<PaymentMethod>().UpdateAsync(existing);
        await unitOfWork.CompleteAsync(cancellationToken);

        return existing;
    }
}