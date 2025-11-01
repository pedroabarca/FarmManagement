using Serilog;
using FarmManagement.Infrastructure;
using FarmManagement.Application;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Scalar.AspNetCore;
using FarmManagement.API.Middlewares;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// 📌 Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 📌 Cargar configuración desde appsettings.json
var configuration = builder.Configuration;

// 📌 Agregar servicios al contenedor
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
builder.Services.AddOpenApi();  
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );

            var errorDetails = new
            {
                message = "Error de validación.",
                details = errors
            };

            // 🔥 Loggear los errores de validación en Serilog
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("Error de validación en {Path} - Detalles: {ErrorJson}", 
                context.HttpContext.Request.Path, JsonSerializer.Serialize(errorDetails));

            return new BadRequestObjectResult(errorDetails);
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// 📌 Middleware
app.UseSerilogRequestLogging(); // 📌 Loguea todas las peticiones HTTP automáticamente
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
