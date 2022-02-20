using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Fintech.CreditLine.Api.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //context.Result = new BadRequestObjectResult(context.ModelState);
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();

                var responseObj = new
                {
                    Errors = errors
                };

                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = 400
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
