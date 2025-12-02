using ArcheryAcademy.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

//Register all services  for extension method
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// use Swagger

var enableSwagger = app.Configuration.GetValue<bool>("EnableSwagger");
if (app.Environment.IsDevelopment() || enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        // Esta línea hace que Swagger se abra en la raíz (localhost:5283/)
        options.RoutePrefix = string.Empty; 
    });
}

// Routing
app.UseRouting();
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", time = DateTime.UtcNow }));
// Middlewares personalizados 
//app.UseMiddleware<ErrorHandlingMiddleware>();           
//app.UseMiddleware<ParameterValidationMiddleware>();

// Autenticación y Autorización
//app.UseAuthentication();
//app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();