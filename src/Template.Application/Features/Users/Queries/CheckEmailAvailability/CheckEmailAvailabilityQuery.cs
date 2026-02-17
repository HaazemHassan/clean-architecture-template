using MediatR;
using Template.Application.Common.Responses;

namespace Template.Application.Features.Users.Queries.CheckEmailAvailability {
    public class CheckEmailAvailabilityQuery : IRequest<Result<CheckEmailAvailabilityQueryResponse>> {
        public string Email { get; set; }
    }
}
