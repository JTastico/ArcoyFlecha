using ArcheryAcademy.Application.DTOs.SettingsDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.Settings.Queries;

public record GetSettingsQuery() : IRequest<AppSettingDto>;

public class GetSettingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetSettingsQuery, AppSettingDto>
{
    public async Task<AppSettingDto> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        // Intentamos obtener la configuración. Si no existe, creamos una por defecto.
        var settings = (await unitOfWork.Repository<AppSetting>().GetAllAsync()).FirstOrDefault();

        if (settings == null)
        {
            settings = new AppSetting { Id = Guid.NewGuid() };
            await unitOfWork.Repository<AppSetting>().Insert(settings);
            await unitOfWork.CompleteAsync(cancellationToken);
        }

        return mapper.Map<AppSettingDto>(settings);
    }
}