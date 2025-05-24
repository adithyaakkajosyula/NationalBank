using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace AdithyaBank.BackEnd.Authorization
{


    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Retrieve the token from the cookie
            var token = context.Request.Cookies["LoginToken"]; // Replace "YourCookieName" with the name of your cookie

            // Append the token to the request headers
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers["Authorization"] = "Bearer " + token;
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
