using KittysolomaMap.Domain.Common.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace KittysolomaMap.Infrastructure.Common.UnitOfWork;

public class EfUnitOfWork<TDbContext> : IUnitOfWork where TDbContext: DbContext
{
    private readonly TDbContext _dbContext;

    public EfUnitOfWork(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}