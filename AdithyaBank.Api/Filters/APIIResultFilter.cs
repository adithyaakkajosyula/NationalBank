using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AdithyaBank.BackEnd.Models;

namespace AdithyaBank.Api.Filters
{
    public class APIIResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                // Skip if already wrapped
                if (objectResult.Value is ApiBaseResultModel) return;

                var apiResponse = new ApiBaseResultModel
                {
                    Data = objectResult.Value,
                    IsSuccess = true
                };

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // No action needed here
        }
    }
}
