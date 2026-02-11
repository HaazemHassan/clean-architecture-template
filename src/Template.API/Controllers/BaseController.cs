using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using Template.Core.Bases.Responses;

namespace Template.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("defaultLimiter")]
    public class BaseController : ControllerBase {

        private IMediator? _mediatorInstance;
        protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>()!;

        #region Actions
        protected ObjectResult NewResult<T>(Response<T> response) {
            return response.StatusCode switch {
                HttpStatusCode.OK => new OkObjectResult(response),
                HttpStatusCode.Created => new CreatedResult(string.Empty, response),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
                HttpStatusCode.NotFound => new NotFoundObjectResult(response),
                HttpStatusCode.Accepted => new AcceptedResult(string.Empty, response),
                HttpStatusCode.UnprocessableEntity => new UnprocessableEntityObjectResult(response),
                _ => new BadRequestObjectResult(response),
            };
        }

        protected ObjectResult NewResult(Response response) {
            return response.StatusCode switch {
                HttpStatusCode.OK => new OkObjectResult(response),
                HttpStatusCode.Created => new CreatedResult(string.Empty, response),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
                HttpStatusCode.NotFound => new NotFoundObjectResult(response),
                HttpStatusCode.Accepted => new AcceptedResult(string.Empty, response),
                HttpStatusCode.UnprocessableEntity => new UnprocessableEntityObjectResult(response),
                _ => new BadRequestObjectResult(response),
            };
        }
        #endregion


    }

}

