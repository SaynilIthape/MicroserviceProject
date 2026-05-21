using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstarctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventInterceptor(IMediator _mediator) :SaveChangesInterceptor
    {
       public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context != null)
            {
                DispatchDomainEvents(context).GetAwaiter().GetResult();
            }
            return base.SavingChanges(eventData, result);
        }   

       public  override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context != null)
            {
                DispatchDomainEvents(context).GetAwaiter().GetResult();
            }
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }   

        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;
            var entitiesWithDomainEvents = context.ChangeTracker.Entries<IAggregate>()
                .Select(e => e.Entity)
                .Where(e => e. DomainEvents != null && e.DomainEvents.Any())
                .ToList();
            var domainEvents = entitiesWithDomainEvents
                .SelectMany(e => e.DomainEvents)
                .ToList();
            entitiesWithDomainEvents.ForEach(e => e.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
            {
                // Dispatch the domain event
                // For example, using MediatR:
                await _mediator.Publish(domainEvent);
            }

            // Here you would typically dispatch the domain events to your event handlers
            // For example, using MediatR or a custom event dispatcher
        }
    }
}
