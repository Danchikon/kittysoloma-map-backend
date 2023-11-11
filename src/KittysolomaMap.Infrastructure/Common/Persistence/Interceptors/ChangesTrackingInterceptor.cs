using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using KittysolomaMap.Domain.Common;
using KittysolomaMap.Domain.Common.Entities;

namespace KittysolomaMap.Infrastructure.Common.Persistence.Interceptors
{
    public class ChangesTrackingInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context is null)
            {
                return;
            }

            var modifiedEntities = context.ChangeTracker.Entries<EntityBase>().Where(entity => entity.State is EntityState.Modified);

            foreach (var entry in  modifiedEntities)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}