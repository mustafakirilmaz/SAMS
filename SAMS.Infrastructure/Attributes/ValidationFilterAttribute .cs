

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace SAMS.Infrastructure.Attributes
{
    public class ValidationFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Request.Method == "Get")
            {
                return;
            }
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(s => s.Value.Errors.Count > 0)
                    .Select(s => new { Key = s.Key, Message = s.Value.Errors.First().ErrorMessage })
                    .ToArray();

                var response = new { Error = true, Messages = errors };
                context.Result = new UnprocessableEntityObjectResult(response);
            }
        }
    }
}