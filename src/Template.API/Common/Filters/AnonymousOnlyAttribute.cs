using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Template.API.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AnonymousOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var statusCode = StatusCodes.Status403Forbidden;

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = "Forbidden",
                    Instance = context.HttpContext.Request.Path,
                    Extensions =
                    {
                        ["errors"] = new[]
                        {
                            new { Code = "Auth.AlreadyLoggedIn", Description = "You are already Logged in !" }
                        },
                        ["traceId"] = context.HttpContext.TraceIdentifier
                    }
                };

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = statusCode
                };
            }
        }
    }
}