using AutoMapper;
using Template.API.Requests.Client.Users;
using Template.API.Requests.Management.Users;
using Template.Application.Features.Users.Commands.UpdateProfile;

namespace Template.API.Requests
{
    public class RequestsMappingProfile : Profile
    {
        public RequestsMappingProfile()
        {

            //Users requests
            CreateMap<UpdateMyPorfileRequest, UpdateProfileCommand>();


            //Admin requests
            CreateMap<UpdateUserRequest, UpdateProfileCommand>();
        }

    }
}
