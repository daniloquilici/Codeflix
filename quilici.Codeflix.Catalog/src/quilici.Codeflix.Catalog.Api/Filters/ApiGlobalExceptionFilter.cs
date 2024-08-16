using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using quilici.Codeflix.Catalog.Domain.Exceptions;

namespace quilici.Codeflix.Catalog.Api.Filters
{
    public class ApiGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ApiGlobalExceptionFilter(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            var details = new ProblemDetails();
            var exception = context.Exception;

            if (_hostEnvironment.IsDevelopment())
                details.Extensions.Add("StackTrace", exception.StackTrace);

            if (exception is EntityValidationException)
            {
                var ex = exception as EntityValidationException;
                details.Title = "One or more validation errors ocurred";
                details.Status = StatusCodes.Status422UnprocessableEntity;
                details.Type = "UnprocessableEntity";
                details.Detail = ex!.Message;
            }
            else
            {
                details.Title = "An unexpected error ocurred";
                details.Status = StatusCodes.Status422UnprocessableEntity;
                details.Type = "Unexpected";
                details.Detail = exception.Message;
            }

            context.HttpContext.Response.StatusCode = (int)details.Status;
            context.Result = new ObjectResult(details);
            context.ExceptionHandled = true;
        }
    }
}
