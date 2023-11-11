using System.Linq.Expressions;
using KittysolomaMap.Domain.Common.Errors;
using KittysolomaMap.Domain.Common.Exceptions;
using KittysolomaMap.Domain.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KittysolomaMap.Infrastructure.Common.Repositories;

public class EfRepository<TEntity, TDbContext> : IRepository<TEntity> where TDbContext: DbContext where TEntity : class
{
    protected readonly TDbContext DbContext;

    public EfRepository(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entry = await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        return entry.Entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entry = DbContext.Set<TEntity>().Update(entity);

        return Task.FromResult(entry.Entity);
    }

    public async Task<TEntity> FirstAsync(ICollection<Expression<Func<TEntity, bool>>>? wheres = null, CancellationToken cancellationToken = default)
    {
        var entity = await FirstOrDefaultAsync(wheres, cancellationToken);

        if (entity is null)
        {
            throw new BusinessException
            {
                ErrorCode = ErrorCode.NotFound,
                ErrorKind = ErrorKind.NotFound
            };
        }

        return entity;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(ICollection<Expression<Func<TEntity, bool>>>? wheres = null, CancellationToken cancellationToken = default)
    {
        var queryable = DbContext.Set<TEntity>().AsNoTracking();
            
        if (wheres is not null)
        {
            queryable = wheres.Aggregate(queryable, (accumulator, value) => accumulator.Where(value));
        }
        
        return await queryable.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> AnyAsync(ICollection<Expression<Func<TEntity, bool>>>? wheres = null, CancellationToken cancellationToken = default)
    {
        var queryable = DbContext.Set<TEntity>().AsNoTracking();
            
        if (wheres is not null)
        {
            queryable = wheres.Aggregate(queryable, (accumulator, value) => accumulator.Where(value));
        }
        
        return await queryable.AnyAsync(cancellationToken: cancellationToken);
    }
}