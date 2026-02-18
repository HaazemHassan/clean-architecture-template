using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Common.Security;
using Template.Application.Contracts.Requests;

namespace Template.Application.Features.Users.Commands.UpdateProfile
{
    [Authorize(Policy = AuthorizationPolicies.SelfOrAdmin)]
    public class UpdateProfileCommand : IRequest<Result<UpdateProfileCommandResponse>>, IOwnedResourceRequest
    {
        public int OwnerUserId { get; set; }  // Used for authorization policy
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

    }
}
