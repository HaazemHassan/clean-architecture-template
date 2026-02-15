using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Repositories {
    internal class GenericRepository<T> : RepositoryBase<T>, IGenericRepository<T> where T : class {


        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) {
            return await _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default) {
            if (predicate is not null)
                return await _dbContext.Set<T>().CountAsync(predicate, cancellationToken);
            return await _dbContext.Set<T>().CountAsync(cancellationToken);
        }
    }
}
