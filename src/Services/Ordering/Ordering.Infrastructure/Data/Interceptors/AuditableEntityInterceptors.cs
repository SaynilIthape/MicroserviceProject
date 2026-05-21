using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstarctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptors : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context != null)
            {
                UpdateEntities(context);
            }
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context != null)
            {
                UpdateEntities(context);
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;
            var entries = context.ChangeTracker.Entries<IEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "sunil";
                    entry.Entity.LastModifiedBy = "Sunil";
                    entry.Entity.LastModified = DateTime.UtcNow;

                }
                else if (entry.State == EntityState.Added || entry.State == EntityState.Modified || Extensions.HasChangedOwnEntiates(entry))
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "sunil";
                    entry.Entity.LastModifiedBy = "Sunil";
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }

            // Removed the invalid call to base.SaveChangesCanceled(context);
            // No return statement is needed here.
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnEntiates(this EntityEntry state) =>
            state.References.Any(r => r.TargetEntry != null && r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));



    }
}
