using System.Net;
using System.Text.Json;
using FluentValidation;

namespace FarmManagement.API.Middlewares;

/// <summary>
/// Middleware para manejar excepciones globalmente y formatear las respuestas de error.
/// Captura errores de validación, acceso no autorizado y otros errores internos.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>
    /// Constructor del Middleware de Excepciones.
    /// </summary>
    /// <param name="next">Siguiente middleware en la cadena de ejecución.</param>
    /// <param name="logger">Servicio de logging para registrar errores.</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Método principal que intercepta todas las solicitudes HTTP.
    /// Si hay una excepción, la captura y la maneja con `HandleExceptionAsync`.
    /// </summary>
    /// <param name="context">El contexto de la solicitud HTTP.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Ejecuta el siguiente middleware en la cadena
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // Maneja cualquier excepción capturada
        }
    }

    /// <summary>
    /// Maneja la excepción, genera una respuesta JSON y la registra en los logs.
    /// </summary>
    /// <param name="context">El contexto HTTP de la solicitud.</param>
    /// <param name="exception">La excepción capturada.</param>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception); // Determina el código de estado HTTP
        var errorDetails = GetExceptionDetails(exception); // Obtiene detalles del error

        // Crea la respuesta JSON con un mensaje claro y los detalles del error
        var response = new
        {
            message = exception switch
            {
                ValidationException => "Error de validación.",
                KeyNotFoundException => "Recurso no encontrado.",
                UnauthorizedAccessException => "Acceso no autorizado.",
                _ => "Error interno del servidor."
            },
            details = errorDetails
        };

        // Serializa la respuesta en formato JSON
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            WriteIndented = true // 🔥 Formatea el JSON para que sea más legible en los logs
        });

        // 🔥 Loguea la excepción con el JSON del error en los logs
        _logger.LogError("Error en {Path} - Detalles del error: {ErrorJson}",
            context.Request.Path, jsonResponse);

        // Configura la respuesta HTTP
        context.Response.ContentType = "application/json"; // Indica que el contenido es JSON
        context.Response.StatusCode = (int)statusCode; // Asigna el código de estado HTTP

        // Escribe la respuesta JSON en el cuerpo de la respuesta HTTP
        await context.Response.WriteAsync(jsonResponse);
    }

    /// <summary>
    /// Determina el código de estado HTTP adecuado según el tipo de excepción.
    /// </summary>
    /// <param name="exception">La excepción capturada.</param>
    /// <returns>El código de estado HTTP correspondiente.</returns>
    private static HttpStatusCode GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => HttpStatusCode.BadRequest, // 400 - Datos inválidos
            KeyNotFoundException => HttpStatusCode.NotFound, // 404 - Recurso no encontrado
            UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401 - No autorizado
            _ => HttpStatusCode.InternalServerError // 500 - Error interno
        };

    /// <summary>
    /// Extrae los detalles del error, incluyendo los mensajes de validación si aplica.
    /// </summary>
    /// <param name="exception">La excepción capturada.</param>
    /// <returns>Un objeto con la información detallada del error.</returns>
    private static object GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            // Si la excepción es de validación, obtiene los errores individuales
            ValidationException validationException => validationException.Errors.Select(e => new
            {
                Property = e.PropertyName, // Nombre del campo con error
                Error = e.ErrorMessage // Mensaje de error
            }),

            // Para cualquier otra excepción, devuelve solo el mensaje del error
            _ => new[] { exception.Message }
        };
    }
}
