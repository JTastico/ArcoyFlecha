using ArcheryAcademy.Application.DTOs.BookingStatusDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Queries;

public record GetBookingStatusByIdQuery(int Id) : IRequest<BookingStatusReadDto?>;

internal sealed class GetBookingStatusByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetBookingStatusByIdQuery, BookingStatusReadDto?>
{
    public async Task<BookingStatusReadDto?> Handle(GetBookingStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var bookingStatus = await unitOfWork.Repository<BookingStatus>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        return bookingStatus == null ? null : mapper.Map<BookingStatusReadDto>(bookingStatus);
    }
}