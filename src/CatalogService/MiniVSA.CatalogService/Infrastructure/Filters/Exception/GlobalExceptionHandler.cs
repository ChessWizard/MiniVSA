using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using MiniVSA.CatalogService.Application.Models.Result;
using MiniVSA.CatalogService.Application.Models.Exception;
using System.Linq;

namespace MiniVSA.CatalogService.Infrastructure.Filters.Exception
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            var (detail, title, statusCode) = GetExceptionDetails(httpContext, exception);

            var problemDetails = CreateProblemDetails(detail, title, statusCode, httpContext.TraceIdentifier);

            var exceptionModel = exception switch
            {
                ValidationException validationException => HandleValidationException(validationException, problemDetails),
                _ => HandleGeneralException(problemDetails)
            };

            logger.LogError("Error Message: {exceptionMessage}, Time of occurrence {time}", problemDetails, DateTime.UtcNow);

            var result = Result<GlobalExceptionResponseModel>.Error(exceptionModel, problemDetails.Status.Value);
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
            return true;
        }

        #region Private Methods

        private static (string Detail, string Title, int StatusCode) GetExceptionDetails(HttpContext httpContext, System.Exception exception)
        {
            return exception switch
            {
                ValidationException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
                _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)
            };
        }

        private static ProblemDetails CreateProblemDetails(string detail, string title, int statusCode, string traceId)
        {
            return new ProblemDetails
            {
                Detail = detail,
                Title = title,
                Status = statusCode,
                Extensions = { ["traceId"] = traceId }
            };
        }

        private static GlobalExceptionResponseModel HandleValidationException(ValidationException validationException, ProblemDetails problemDetails)
        {
            var errors = validationException.Errors
                .Select(error => new Error(error.PropertyName, error.ErrorMessage))
                .ToList();

            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            return new GlobalExceptionResponseModel(problemDetails.Title, errors);
        }

        private static GlobalExceptionResponseModel HandleGeneralException(ProblemDetails problemDetails)
        {
            var errors = new List<Error> { new Error("Exception", problemDetails.Detail) };
            return new GlobalExceptionResponseModel(problemDetails.Title, errors);
        }

        #endregion
    }
}
