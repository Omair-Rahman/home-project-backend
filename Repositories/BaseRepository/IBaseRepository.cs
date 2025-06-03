using System.Linq.Expressions;

namespace HomeProject.Repositories.BaseRepository;

public interface IBaseRepository<TEntity> where TEntity : class
{
    void Attach(TEntity temp, TEntity entity);
    void Detached(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task Remove(int id, CancellationToken cancellationToken = default);
    Task Remove(long id, CancellationToken cancellationToken = default);
    Task Remove(Guid id, CancellationToken cancellationToken = default);
    Task Remove(string id, CancellationToken cancellationToken = default);
    Task Remove(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> expression);
    Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);
    Task<long> CountAsync(List<Expression<Func<TEntity, bool>>> filters);
    Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(string id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? skip = default, int? take = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IEnumerable<TEntity>> GetAllAsync(List<Expression<Func<TEntity, bool>>>? filters = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? skip = default, int? take = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IEnumerable<TEntity>> GetAllPagingAsync(int skip = default, int take = default);
    Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? filters = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? skip = default, int? take = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
    IQueryable<TEntity> GetAllQueryable(
        Expression<Func<TEntity, bool>>? filters = null,
        params Expression<Func<TEntity, object>>[] includeProperties);
    Task<int> CompleteAsync();
}
