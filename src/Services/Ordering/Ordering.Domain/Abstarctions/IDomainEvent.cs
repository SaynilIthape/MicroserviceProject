using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ordering.Domain.Abstarctions
{
    public interface IDomainEvent:INotification 
    {
        Guid eventId => Guid.NewGuid();  
        public DateTime OccurredOn => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
