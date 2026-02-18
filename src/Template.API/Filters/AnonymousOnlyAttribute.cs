using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Template.API.Filters
{


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AnonymousOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                context.Result = new ObjectResult(new
                {
                    succeeded = false,
                    message = " You are already Logged in !",
                    statusCode = StatusCodes.Status403Forbidden
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
