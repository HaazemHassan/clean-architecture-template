using AutoMapper;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.Register {
    public partial class RegisterCommandMappingProfile : Profile {

        public RegisterCommandMappingProfile() {
            CreateMap<RegisterCommand, DomainUser>();

        }

    }
}
