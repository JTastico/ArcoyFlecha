using ArcheryAcademy.Application.DTOs.BookingStatusDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Queries;

public record GetAllBookingStatusesQuery() : IRequest<List<BookingStatusReadDto>>;

internal sealed class GetAllBookingStatusesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllBookingStatusesQuery, List<BookingStatusReadDto>>
{
    public async Task<List<BookingStatusReadDto>> Handle(GetAllBookingStatusesQuery request, CancellationToken cancellationToken)
    {
        var bookingStatuses = await unitOfWork.Repository<BookingStatus>().GetAllAsync();
        return mapper.Map<List<BookingStatusReadDto>>(bookingStatuses);
    }
}