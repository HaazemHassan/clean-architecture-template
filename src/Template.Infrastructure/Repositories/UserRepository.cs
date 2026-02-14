using Microsoft.EntityFrameworkCore;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Domain.Entities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Repositories {
    public class UserRepository : GenericRepository<DomainUser>, IUserRepository {

        private readonly DbSet<DomainUser> _users;


        public UserRepository(AppDbContext context) : base(context) {
            _users = context.Set<DomainUser>();
        }

    }
}
