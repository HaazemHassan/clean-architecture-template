using ErrorOr;
using MediatR;

namespace Template.Application.Features.Users.Queries.CheckEmailAvailability {
    public class CheckEmailAvailabilityQuery : IRequest<ErrorOr<CheckEmailAvailabilityQueryResponse>> {
        public string Email { get; set; }
    }
}
