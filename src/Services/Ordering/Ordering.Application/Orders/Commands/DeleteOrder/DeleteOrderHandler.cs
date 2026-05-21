using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = request.orderId;
            var order = await dbContext.Orders.FindAsync(new object[] { orderId }, cancellationToken);
            if (order == null)
            {
                //return new DeleteOrderResult(false); // Order not found, return failure result
                throw new OrderNotFoundException($"Order with ID {orderId} not found.");
            }
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(true); // Successfully deleted the order
        }   
    }
}
