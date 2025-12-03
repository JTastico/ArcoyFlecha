using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Command;

public record UpdateBookingCommand(Guid Id, BookingUpdateDto BookingDto) 
    : IRequest<Booking?>;

internal sealed class UpdateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateBookingCommand, Booking?>
{
    public async Task<Booking?> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Repository<Booking>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (existing is null)
            return null;

        mapper.Map(request.BookingDto, existing);

        await unitOfWork.Repository<Booking>().UpdateAsync(existing);
        await unitOfWork.CompleteAsync(cancellationToken);

        return existing;
    }
}
