using HomeProject.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace HomeProject.Repositories.BaseRepository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _entitiySet;
    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _entitiySet = _dbContext.Set<T>();
    }

    public void Attach(T temp, T entity)
    {
        _entitiySet.Entry(temp).CurrentValues.SetValues(entity);
    }

    public void Detached(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return _entitiySet.AsNoTracking().AnyAsync(filter);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _entitiySet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _entitiySet.AddRangeAsync(entities);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            _entitiySet.Update(entity);
            var createdAtProp = typeof(T).GetProperty("CreatedAt");
            if (createdAtProp != null)
            {
                _dbContext.Entry(entity).Property("CreatedAt").IsModified = false;
            }
            var createdByProp = typeof(T).GetProperty("CreatedBy");
            if (createdByProp != null)
            {
                _dbContext.Entry(entity).Property("CreatedBy").IsModified = false;
            }
        });
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            _entitiySet.UpdateRange(entities);
        });
    }

    public async Task Remove(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _entitiySet.FindAsync(id, cancellationToken);

        if (entity != null)
        {
            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }
    }

    public async Task Remove(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _entitiySet.FindAsync(id, cancellationToken);

        if (entity != null)
        {
            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }
    }

    public async Task Remove(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _entitiySet.FindAsync(id, cancellationToken);

        if (entity != null)
        {
            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }
    }

    public async Task Remove(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _entitiySet.FindAsync(id, cancellationToken);

        if (entity != null)
        {
            await Task.Run(() =>
            {
                _entitiySet.Remove(entity);
            });
        }
    }

    public async Task Remove(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var entities = await _entitiySet.Where(predicate).ToListAsync();
        foreach (var entity in entities)
        {
            await Task.Run(() =>
            {
                _dbContext.Remove(entity);
            });
        }
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            _entitiySet.Remove(entity);
        });
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            _entitiySet.RemoveRange(entities);
        });
    }

    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
    {
        return await _entitiySet.FirstOrDefaultAsync(expression) != null;
    }

    public async Task<long> CountAsync(Expression<Func<T, bool>> filter)
    {
        return await _entitiySet.AsNoTracking().CountAsync(filter);
    }

    public async Task<long> CountAsync(List<Expression<Func<T, bool>>> filters)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }

        return await query.AsNoTracking().CountAsync();
    }

    public async Task<T?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _entitiySet.FindAsync(id);
    }

    public async Task<T?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _entitiySet.FindAsync(id);
    }

    public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _entitiySet.FindAsync(id);
    }

    public async Task<T?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _entitiySet.FindAsync(id);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _entitiySet.AsNoTracking().FirstOrDefaultAsync(expression);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _dbContext.Set<T>().Where(expression);

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entitiySet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _entitiySet.Where(expression).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? skip = null, int? take = null,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (skip < 0)
        {
            return await query.ToListAsync();
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(
        List<Expression<Func<T, bool>>>? filters = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? skip = null, int? take = null,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (skip < 0)
        {
            return await query.ToListAsync();
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllPagingAsync(int skip = default, int take = default)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        query = query.Skip(skip);
        query = query.Take(take);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetManyAsync(
        Expression<Func<T, bool>>? filters = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? skip = null, int? take = null,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (filters != null)
        {
            query = query.Where(filters);
        }

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (skip < 0)
        {
            return await query.ToListAsync();
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }

    public IQueryable<T> GetAllQueryable(
        Expression<Func<T, bool>>? filters = null,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (filters != null)
        {
            query = query.Where(filters);
        }

        if (includeProperties != null)
        {
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        return query.AsQueryable();
    }

    public async Task<int> CompleteAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
