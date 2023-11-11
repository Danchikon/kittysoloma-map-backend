namespace KittysolomaMap.Domain.Common.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}