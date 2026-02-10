using Microsoft.EntityFrameworkCore;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Entities.UserEntities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Repositories {
    public class UserRepository : GenericRepository<DomainUser>, IUserRepository {

        private readonly DbSet<DomainUser> _users;


        public UserRepository(AppDbContext context) : base(context) {
            _users = context.Set<DomainUser>();
        }

    }
}
