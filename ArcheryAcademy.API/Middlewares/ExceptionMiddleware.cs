using System.Net;
using System.Text.Json;
using ArcheryAcademy.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ArcheryAcademy.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Intenta ejecutar la petición normal (va al controlador -> handler)
            await next(context);
        }
        catch (Exception ex)
        {
            // Si algo explota, lo capturamos aquí centralizadamente
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // 1. Logueo en Consola (Lo que tenías en el controlador)
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"[ERROR CRÍTICO] Tipo: {exception.GetType().Name}");
        Console.WriteLine($"[MENSAJE]: {exception.Message}");
        if (exception.InnerException != null)
        {
            Console.WriteLine($"[DETALLE SQL]: {exception.InnerException.Message}");
        }
        Console.WriteLine("--------------------------------------------------");
        Console.ResetColor();

        // 2. Determinar Código de Estado HTTP según el tipo de Error
        var response = context.Response;
        
        string message;
        
        switch (exception)
        {
            // Caso 1: No encontrado (Lanzado manualmente desde el Handler)
            case NotFoundException:
            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                message = exception.Message;
                break;

            // Caso 2: Error de Base de Datos (Foreign Keys, Constraints)
            case DbUpdateException dbEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                message = "Operación inválida. Es posible que el registro tenga datos relacionados (Reservas, Pagos, etc).";
                // Podrías inspeccionar dbEx.InnerException.Message.Contains("FK") si quieres ser más específico
                break;

            // Caso 3: Validaciones de negocio (ArgumentException, etc)
            case ArgumentException:
            case InvalidOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                message = exception.Message;
                break;

            // Caso 4: Error no controlado (Bug)
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                message = "Ocurrió un error interno en el servidor.";
                break;
        }

        // 3. Devolver el JSON de error al Frontend
        var result = JsonSerializer.Serialize(new { message });
        return response.WriteAsync(result);
    }
}