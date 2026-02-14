using Ardalis.Specification;
using System.Linq.Expressions;

namespace Template.Domain.Abstracts.RepositoriesAbstracts;

public interface IGenericRepository<T> : IRepositoryBase<T> where T : class {

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct);
}
