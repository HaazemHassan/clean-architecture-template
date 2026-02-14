using AutoMapper;
using Template.Application.Features.Users.Common;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.AddUser {
    public class AddUserCommandMappingProfile : Profile {
        public AddUserCommandMappingProfile() {
            CreateMap<AddUserCommand, DomainUser>();
            CreateMap<DomainUser, AddUserCommandResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }

    }
}
