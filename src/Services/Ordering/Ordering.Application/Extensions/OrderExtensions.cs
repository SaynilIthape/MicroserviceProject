using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
        {
           return orders.Select(order => new OrderDto
            (
                id: order.Id,
                CustomerId: order.CustomerId,
                OrderName: order.OrderName,
                ShippingAddress: new AddressDto(
                    order.ShippingAddress.FirstName,
                    order.ShippingAddress.LastName,
                    order.ShippingAddress.EmailAddress,
                    order.ShippingAddress.AddressLine,
                    order.ShippingAddress.Country,
                    order.ShippingAddress.State,
                    order.ShippingAddress.ZipCode
                ),
                BillingAddress: new AddressDto(
                    order.BiilingAddress.FirstName,
                    order.BiilingAddress.LastName,
                    order.BiilingAddress.EmailAddress,
                    order.BiilingAddress.AddressLine,
                    order.BiilingAddress.Country,
                    order.BiilingAddress.State,
                    order.BiilingAddress.ZipCode
                ),
                Payment: new PaymentDto
                (
                   order.Payment.CardName,
                   order.Payment.CardNumber,
                   order.Payment.Expiration,
                   order.Payment.CVV,
                   order.Payment.PaymentMethod
                ),
                OrderStatus:order.Status,    
                OrderItems:order.OrderItems.Select(oi=> new OrderItemDto(
                    oi.Id, 
                    oi.ProductId, 
                    oi.Price, 
                    oi.Quantity)).ToList()
            ));    
        }
    }
}
