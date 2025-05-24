namespace AdithyaBank.BackEnd.Helpers;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch(error)
            {
                case AppException e:
                    if (e.Message.Contains("Unauthorized"))
                        response.StatusCode = (int)HttpStatusCode.Unauthorized; 
                    else
                        response.StatusCode = (int)HttpStatusCode.BadRequest; 
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}

/*
  How it enters the specific case in switch(error)?

The switch(error) checks if the error is of type InvalidOperationException.
The when condition checks if the exception message contains "Unable to resolve service".
If both conditions match, it enters the first case InvalidOperationException e when e.Message.Contains("Unable to resolve service"):.
 */