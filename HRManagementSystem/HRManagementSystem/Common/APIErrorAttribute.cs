using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.Common
{
    public class APIErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception != null && !(context.Exception is APIError))
            {
                return;
            }

            context.Result = new ObjectResult(((APIError)context.Exception).GetApiResult());
        }
    }
}
