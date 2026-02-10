using MediatR;
using Template.Core.Bases.Responses;
using Template.Core.Features.Users.Commands.Responses;

namespace Template.Core.Features.Users.Commands.RequestModels {
    public class UpdateProfileCommand : IRequest<Response<UpdateProfileResponse>> {

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
