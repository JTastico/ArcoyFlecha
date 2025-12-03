using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Command;

public record CreateBookingCommand(BookingCreateDto BookingDto) : IRequest<Booking>;

internal sealed class CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateBookingCommand, Booking>
{
    public async Task<Booking> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = mapper.Map<Booking>(request.BookingDto);

        await unitOfWork.Repository<Booking>().Insert(booking);
        await unitOfWork.CompleteAsync(cancellationToken);

        return booking;
    }
}
