using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Queries;

public record GetAllBookingsQuery() : IRequest<List<BookingReadDto>>;

internal sealed class GetAllBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllBookingsQuery, List<BookingReadDto>>
{
    public async Task<List<BookingReadDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await unitOfWork.Repository<Booking>().GetAllAsync();

        return mapper.Map<List<BookingReadDto>>(bookings);
    }
}