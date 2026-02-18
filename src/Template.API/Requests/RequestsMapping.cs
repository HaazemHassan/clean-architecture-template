using AutoMapper;
using Template.Application.Features.Users.Commands.UpdateProfile;

namespace Template.API.Requests
{
    public class RequestsMappingProfile : Profile
    {
        public RequestsMappingProfile()
        {
            CreateMap<UpdateMyPorfileRequest, UpdateProfileCommand>();
        }

    }
}
