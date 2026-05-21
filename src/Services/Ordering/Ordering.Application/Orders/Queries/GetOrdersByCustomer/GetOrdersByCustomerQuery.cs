using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;


namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public record GetOrdersByCustomerQuery(Guid customerId)  :IQuery<GetOrdersByCustomerQueryResult>;
    public record GetOrdersByCustomerQueryResult(IEnumerable<OrderDto> orders);
    
}
