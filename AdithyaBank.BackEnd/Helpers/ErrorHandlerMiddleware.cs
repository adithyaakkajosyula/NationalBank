namespace NationalBank.BackEnd.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Net.Sockets;
using System.Security;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

            switch (error)
            {
                case AppException e:
                    response.StatusCode = e.Message.Contains("Unauthorized")
                        ? (int)HttpStatusCode.Unauthorized
                        : (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Application-specific error occurred.");
                    break;

                // 🔹 Base/System Exceptions
                case NullReferenceException:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "Null reference encountered. An object was not initialized.");
                    break;

                case IndexOutOfRangeException:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "Index out of bounds. An array or list was accessed incorrectly.");
                    break;

                case StackOverflowException:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "Stack overflow occurred due to excessive recursion.");
                    break;

                case OutOfMemoryException:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "Out of memory. The system cannot allocate more resources.");
                    break;

                case DivideByZeroException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Divide by zero operation attempted.");
                    break;

                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Invalid operation. The object's state does not support the method called.");
                    break;

                case NotImplementedException:
                    response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    _logger.LogError(error, "Feature or method not implemented.");
                    break;

                case PlatformNotSupportedException:
                    response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    _logger.LogError(error, "Operation not supported on the current platform.");
                    break;

                // 🔹 I/O and File Exceptions
                case FileNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError(error, "File not found. The system could not locate the specified file.");
                    break;

                case DirectoryNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError(error, "Directory not found. The specified folder does not exist.");
                    break;

                case DriveNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError(error, "Drive not found. The system could not locate the specified drive.");
                    break;

                case PathTooLongException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Path too long. File or directory path exceeds maximum length.");
                    break;

                case IOException:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "I/O error occurred during file or stream operation.");
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogError(error, "Unauthorized access to a file or resource was attempted.");
                    break;

                // 🔹 Data & Collection Exceptions
                case ArgumentNullException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Null argument passed to a method that does not accept it.");
                    break;

                case ArgumentOutOfRangeException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Argument value is out of range.");
                    break;

                case ArgumentException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Invalid argument provided to a method.");
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError(error, "Key not found in the dictionary or collection.");
                    break;

                case FormatException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Input format is invalid, likely during parsing.");
                    break;

                case OverflowException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError(error, "Arithmetic overflow. A value exceeded its allowed range.");
                    break;

                // 🔹 LINQ / Enumerable Exceptions
                case SystemException ex when ex.Message.Contains("no elements"):
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError(error, "Enumerable operation failed. Sequence contains no matching elements.");
                    break;

                // 🔹 Security / Authentication Exceptions
                case SecurityException:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    _logger.LogError(error, "Security violation occurred. Unauthorized code access attempt.");
                    break;

                case SecurityTokenSignatureKeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogError(error, "Token signature key not found. Validation failed.");
                    break;

                case SecurityTokenInvalidSignatureException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogError(error, "Invalid token signature detected.");
                    break;

                case SecurityTokenExpiredException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogError(error, "Security token has expired.");
                    break;

                case SecurityTokenException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogError(error, "Token validation failed. General security token error.");
                    break;

                // 🔹 Web / HTTP Exceptions
                case HttpRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadGateway;  // or 503 ServiceUnavailable depending on scenario
                    _logger.LogError(error, "HTTP request failed. Possible network issue or invalid endpoint.");
                    break;

                case SocketException:
                    response.StatusCode = (int)HttpStatusCode.BadGateway;  // or 503 ServiceUnavailable depending on scenario
                    _logger.LogError(error, "Network socket error occurred. Connection issue likely.");
                    break;

                // 🔹 Fallback for unhandled types
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "An unhandled exception occurred.");
                    break;
            }

            // Create a structured error response model for the client
            var errorResponse = new
            {
                message = error.Message,
                statuscode = response.StatusCode
            };

            var result = JsonSerializer.Serialize(errorResponse);
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