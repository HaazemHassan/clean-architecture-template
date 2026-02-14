using MediatR;
using Template.Application.Common.Responses;

namespace Template.Application.Features.Users.Commands.UpdateProfile {
    public class UpdateProfileCommand : IRequest<Response<UpdateProfileCommandResponse>> {

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
