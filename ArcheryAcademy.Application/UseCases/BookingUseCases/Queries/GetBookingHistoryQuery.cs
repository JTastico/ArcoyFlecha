using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Queries;

public record GetBookingHistoryQuery(Guid UserId) : IRequest<List<BookingReadDto>>;

internal sealed class GetBookingHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetBookingHistoryQuery, List<BookingReadDto>>
{
    public async Task<List<BookingReadDto>> Handle(GetBookingHistoryQuery request, CancellationToken cancellationToken)
    {
        // Usamos el método optimizado del repositorio
        var history = await unitOfWork.BookingRepository.GetHistoryByUserIdAsync(request.UserId);
        return mapper.Map<List<BookingReadDto>>(history);
    }
}