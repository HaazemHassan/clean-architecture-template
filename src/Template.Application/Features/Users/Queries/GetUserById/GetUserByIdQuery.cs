using ErrorOr;
using MediatR;
using Template.Application.Security;
using Template.Application.Security.Markers;
using Template.Application.Security.Policies;

namespace Template.Application.Features.Users.Queries.GetUserById
{
    [Authorize(Policy = AuthorizationPolicies.SelfOnly)]
    public class GetUserByIdQuery : IRequest<ErrorOr<GetUserByIdQueryResponse>>, IOwnedResourceRequest
    {
        public GetUserByIdQuery(int userId)
        {
            OwnerUserId = userId;
        }

        public int OwnerUserId { get; set; }
    }
}
