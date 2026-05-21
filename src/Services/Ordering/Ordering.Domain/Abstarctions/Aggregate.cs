using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstarctions
{
    public class Aggregate<TId> : Entity<TId>,IAggregate<TId>
    {
        private readonly List<IDomainEvent> _domainEvents = new();  
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public IDomainEvent[] ClearDomainEvents()
        {
            IDomainEvent[] events = _domainEvents.ToArray();
            _domainEvents.Clear();
            return events;
        }
        public void AddDomainEvent(IDomainEvent Tid)
        {
              _domainEvents.Add(Tid);
        }
    }
}
