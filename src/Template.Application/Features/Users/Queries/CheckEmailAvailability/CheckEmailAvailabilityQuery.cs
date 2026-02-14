using MediatR;
using Template.Application.Common.Responses;

namespace Template.Application.Features.Users.Queries.CheckEmailAvailability {
    public class CheckEmailAvailabilityQuery : IRequest<Response<CheckEmailAvailabilityQueryResponse>> {
        public string Email { get; set; }
    }
}
