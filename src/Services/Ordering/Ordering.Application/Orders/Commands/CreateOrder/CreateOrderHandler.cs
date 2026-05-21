using BuildingBlocks.CQRS;
using Microsoft.Win32.SafeHandles;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValudObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext dbcontext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    { 
        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order =  CreateNewOrder(request.Order);

            dbcontext.Orders.Add(order);
            await dbcontext.SaveChangesAsync(cancellationToken);    

            return new CreateOrderResult(order.Id);
        }

        private Order CreateNewOrder(OrderDto order)
        {

            var shippingAddress = Address.Of(order.ShippingAddress.FirstName, order.ShippingAddress.LastName, order.ShippingAddress.EmailAddress,
                 order.ShippingAddress.AddressLine, order.ShippingAddress.Country, order.ShippingAddress.State,order.ShippingAddress.ZipCode);    
            var billingAddress = Address.Of(order.ShippingAddress.FirstName, order.ShippingAddress.LastName, order.ShippingAddress.EmailAddress,
                 order.ShippingAddress.AddressLine, order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode);

            // var orderItems = order.OrderItems.Select(item => new OrderItem(item.ProductId, item.Quantity, item.UnitPrice)).ToList();

            var newOrder = Order.Create(
                orderId: Guid.NewGuid(),
                customerId :order.CustomerId, 
                OrderName : order.OrderName,    
                shippingAddress : shippingAddress,
                billingAddress : billingAddress,
                
                payment : Payment.Of(order.Payment.CardNumber, order.Payment.CardName, order.Payment.Expiration, order.Payment.Cvv,order.Payment.PaymentMethod)  
               );

            foreach (var item in order.OrderItems)
                {
                newOrder.AddOrderItem(item.ProductId, item.UnitPrice, item.Quantity);
            }   

            return newOrder;
        }
    }
}
