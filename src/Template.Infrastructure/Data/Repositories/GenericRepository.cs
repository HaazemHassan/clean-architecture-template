using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Template.Domain.Contracts.Repositories;

namespace Template.Infrastructure.Data.Repositories
{
    internal class GenericRepository<T> : RepositoryBase<T>, IGenericRepository<T> where T : class
    {


        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;

        public GenericRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            this._mapper = mapper;
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate, ct);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Set<T>().AnyAsync(predicate, ct);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
        {
            if (predicate is not null)
                return await _dbContext.Set<T>().CountAsync(predicate, ct);
            return await _dbContext.Set<T>().CountAsync(ct);
        }

        public async Task<List<TDestination>> ListAsync<TDestination>(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = ApplySpecification(spec);

            return await query
                .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<TDestination?> GetAsync<TDestination>(ISpecification<T> spec, CancellationToken ct)
        {
            var query = ApplySpecification(spec);

            return await query
                .ProjectTo<TDestination>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }


    }
}
