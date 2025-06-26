using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AdithyaBank.Api.Filters
{
    public class APIIExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Customize the response sent when an unhandled exception occurs
            context.Result = new ObjectResult(new { message = "An error occurred.", details = context.Exception.Message })
            {
                StatusCode = 500
            };

            // Prevent the exception from propagating further
            context.ExceptionHandled = true;
        }
    }
}
