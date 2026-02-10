using MediatR;
using Template.Core.Bases.Responses;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Features.Users.Queries.Models {
    public class CheckEmailAvailabilityQuery : IRequest<Response<CheckEmailAvailabilityResponse>> {
        public string Email { get; set; }
    }
}
