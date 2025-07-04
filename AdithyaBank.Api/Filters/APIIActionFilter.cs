using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace NationalBank.Api.Filters
{
    public class APIIActionFilter : IActionFilter
    {
        private readonly ILogger<APIIActionFilter> _logger;
        private Stopwatch _stopwatch;

        public APIIActionFilter(ILogger<APIIActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var actionName = context.ActionDescriptor.DisplayName;
            var elapsedTime = _stopwatch.ElapsedMilliseconds;

            _logger.LogError($"Action {actionName} executed in {elapsedTime} ms");
        }
    }
}



/*private const string APIKEYHEADERNAME = "Authorization";
private readonly string _apikey;

public APIIActionFilter(IConfiguration configuration) => _apikey = configuration["AppSettings:ApiKey"];  // Get Api Key value from configuration at object creation time

public void OnActionExecuting(ActionExecutingContext context)
{
    if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYHEADERNAME,out var extractedapikey)) // extract and check the value of the key in headers
    {
        context.Result = new UnauthorizedResult();
        return;
    }
    if (!string.Equals(_apikey,extractedapikey)) // compare value of the key to our configurations value 
    {
        context.Result = new ForbidResult();
        return;
    }
}
public void OnActionExecuted(ActionExecutedContext context) { }*/