using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValudObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = request.Order.id;
            var order = await dbContext.Orders.FindAsync(new object[] { orderId }, cancellationToken);

            if (order == null)
            {
                throw new OrderNotFoundException($"Order with ID {orderId} not found.");
            }

            UpdateOrderNewValue(order, request.Order);

            dbContext.Orders.Update(order);

            // Presumably you want to save changes and return a result
            await dbContext.SaveChangesAsync(cancellationToken);

            // You may need to construct and return an UpdateOrderResult here
            return new UpdateOrderResult(true); // Adjust as needed
        }

        private void UpdateOrderNewValue(Order oldOrder, OrderDto newOrder)
        {
            
            var shippingAddress = Address.Of(newOrder.ShippingAddress.FirstName, newOrder.ShippingAddress.LastName, newOrder.ShippingAddress.EmailAddress,
                 newOrder.ShippingAddress.AddressLine, newOrder.ShippingAddress.Country, newOrder.ShippingAddress.State, newOrder.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(newOrder.ShippingAddress.FirstName, newOrder.ShippingAddress.LastName, newOrder.ShippingAddress.EmailAddress,
                 newOrder.ShippingAddress.AddressLine, newOrder.ShippingAddress.Country, newOrder.ShippingAddress.State, newOrder.ShippingAddress.ZipCode);
            
            oldOrder.Update(
                orderName: newOrder.OrderName,
                shippingAddress: shippingAddress,
                billingAddress: billingAddress,
                payment: Payment.Of(newOrder.Payment.CardNumber, newOrder.Payment.CardName, newOrder.Payment.Expiration, newOrder.Payment.Cvv, newOrder.Payment.PaymentMethod),
                status: oldOrder.Status // Assuming you want to keep the existing status
            );  

        }
    }
}
