using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Data.Repositories
{
    internal class UserRepository : GenericRepository<DomainUser>, IUserRepository
    {

        private readonly DbSet<DomainUser> _users;


        public UserRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _users = context.Set<DomainUser>();
        }

    }
}
