using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Queries;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingReadDto?>;

internal sealed class GetBookingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetBookingByIdQuery, BookingReadDto?>
{
    public async Task<BookingReadDto?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.Repository<Booking>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return booking == null 
            ? null 
            : mapper.Map<BookingReadDto>(booking);
    }
}
