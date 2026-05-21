using BuildingBlocks.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameQueryResult>
    {
        public async Task<GetOrdersByNameQueryResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders.Include(o => o.OrderItems).AsNoTracking()
                .Where(o => o.OrderName.Contains(query.name))
                .OrderBy(o=>o.OrderName)
                .ToListAsync(cancellationToken);

            var orderDto =  MapToOrderDto(orders);  
            return new GetOrdersByNameQueryResult(orders.ToOrderDtoList());    
        }

        private IEnumerable<OrderDto> MapToOrderDto(List<Order> orders)
        {
            foreach (var order in orders)
            {
                        yield return new OrderDto(
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
                    Payment: new PaymentDto(
                        order.Payment.CardName,
                        order.Payment.CardNumber,
                        order.Payment.Expiration,
                        order.Payment.CVV,
                        order.Payment.PaymentMethod
                    ),
                    OrderStatus: order.Status,
                    OrderItems: order.OrderItems.Select(oi => new OrderItemDto(
                        oi.OrderId,
                        oi.ProductId,
                        oi.Price,
                        oi.Quantity
                    )).ToList()
                );
            }
        }
    }
}
