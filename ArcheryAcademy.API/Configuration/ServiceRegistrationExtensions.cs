using ArcheryAcademy.Application.Mappings;
using ArcheryAcademy.Application.MediatR;
using ArcheryAcademy.Infrastructure.Configuration;
using Microsoft.OpenApi.Models;

namespace ArcheryAcademy.API.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Automaper for DTOs
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        // Habilitar controladores de la API
        services.AddControllers();
        // Registra HttpContextAccessor (común para obtener info del request)
        services.AddHttpContextAccessor();
        
        // Habilitar Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Mi API (Arco y Flecha)",
                Version = "v1",
                Description = "API para proyecto final."
            });
            
        });
        // Registrar servicios de la capa Infrastructure
        services.AddInfrastructureServices(configuration);
        //Register MedistR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(ApplicationAssemblyMarker).Assembly
        ));
        
        return services;
    }
}