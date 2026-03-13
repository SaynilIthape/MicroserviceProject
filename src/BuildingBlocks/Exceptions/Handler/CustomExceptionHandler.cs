using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            (string Detail, string Title, int StatusCode) details = exception switch
            {
                NotFoundException  => (exception.Message,exception.GetType().Name, StatusCodes.Status404NotFound),
                BadRequestException  => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
                InternalServerErroException  => (exception.Message, exception.GetType().Name,StatusCodes.Status500InternalServerError),
                ValidationException  validationException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
                _ => (exception.Message, exception.GetType().Name,StatusCodes.Status500InternalServerError)
            };  

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Status = details.StatusCode,
                Detail = details.Detail,
                Instance = httpContext.Request.Path
            };  

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
            if(exception is ValidationException validationEx)
            {
                problemDetails.Extensions.Add("ValidationErros", validationEx.Message);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;    
        }
    }
}
