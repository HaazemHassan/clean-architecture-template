using AutoMapper;
using Template.Application.Features.Users.Common;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandMappingProfile : Profile
    {
        public UpdateProfileCommandMappingProfile()
        {
            CreateMap<DomainUser, UpdateProfileCommandResponse>()
                 .IncludeBase<DomainUser, UserResponse>();
        }

    }
}
