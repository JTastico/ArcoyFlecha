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
            // --- 2. CONFIGURACIÓN DE SWAGGER PARA JWT ---
            // Esto define el esquema de seguridad (Bearer token)
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Ingrese el token JWT así: Bearer {su_token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http, // También puede ser 'ApiKey' si prefieres
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            
            // Esto le dice a Swagger que aplique ese esquema a todas las operaciones
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
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