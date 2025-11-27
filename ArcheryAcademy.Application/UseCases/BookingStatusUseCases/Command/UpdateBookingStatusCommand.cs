using ArcheryAcademy.Application.DTOs.BookingStatusDto;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Command;

public record UpdateBookingStatusCommand(int Id, BookingStatusUpdateDto BookingStatusDto) : IRequest<BookingStatus?>;

internal sealed class UpdateBookingStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateBookingStatusCommand, BookingStatus?>
{
    public async Task<BookingStatus?> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Repository<BookingStatus>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (existing is null)
            return null;

        mapper.Map(request.BookingStatusDto, existing);

        await unitOfWork.Repository<BookingStatus>().UpdateAsync(existing);
        await unitOfWork.CompleteAsync(cancellationToken);

        return existing;
    }
}