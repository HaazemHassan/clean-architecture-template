using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users.Commands.RequestModels;

namespace Template.Core.Mapping.Users {
    public partial class UserProfile {
        public void RegisterMapping() {
            CreateMap<RegisterCommand, DomainUser>();

        }
    }
}
