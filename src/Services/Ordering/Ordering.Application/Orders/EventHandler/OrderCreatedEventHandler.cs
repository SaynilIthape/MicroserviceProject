using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandler
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreateEvent>
    {
        public Task Handle(OrderCreateEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Order created with Id: {OrderId}", notification.order.Id);
            return Task.CompletedTask;  
        }
    }
}
