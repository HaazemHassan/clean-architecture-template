using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Template.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("defaultLimiter")]
    public class BaseController : ControllerBase
    {

        private IMediator? _mediatorInstance;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>()!;

        #region Actions

        protected ActionResult Problem(List<Error> errors)
        {
            var problem = Results.Problem(statusCode: GetStatusCode(errors.FirstOrDefault().Type));
            var problemDetail = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
            problemDetail!.Extensions = new Dictionary<string, object?>()
            {
                ["errors"] = errors.Select(e => new { e.Code, e.Description })
            };


            return new ObjectResult(problemDetail);


        }

        private static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError,
            };
        }

        #endregion


    }

}


