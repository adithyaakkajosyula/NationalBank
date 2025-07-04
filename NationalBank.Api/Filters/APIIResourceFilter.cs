using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace NationalBank.Api.Filters
{
    public class APIIResourceFilter:IResourceFilter
    {
        private const string APIKEYHEADERNAME = "Authorization";
        private readonly string _apikey;

        public APIIResourceFilter(IConfiguration configuration) => _apikey = configuration["AppSettings:ApiKey"];  // Get Api Key value from configuration at object creation time

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYHEADERNAME, out var extractedapikey)) // extract and check the value of the key in headers
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (!string.Equals(_apikey, extractedapikey)) // compare value of the key to our configurations value 
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
           
        }
    }
    
}
