using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ArcheryAcademy.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

//Register all services  for extension method
builder.Services.AddApiServices(builder.Configuration);




var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
        };
    });



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
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();