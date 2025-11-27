using ArcheryAcademy.Application.DTOs.BookingStatusDto;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Command;

public record CreateBookingStatusCommand(BookingStatusCreateDto BookingStatusDto) : IRequest<BookingStatus>;

internal sealed class CreateBookingStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateBookingStatusCommand, BookingStatus>
{
    public async Task<BookingStatus> Handle(CreateBookingStatusCommand request, CancellationToken cancellationToken)
    {
        var bookingStatus = mapper.Map<BookingStatus>(request.BookingStatusDto);

        await unitOfWork.Repository<BookingStatus>().Insert(bookingStatus);
        await unitOfWork.CompleteAsync(cancellationToken);
        return bookingStatus;
    }
}