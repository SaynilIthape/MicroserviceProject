using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersbyCustomerHandler(IApplicationDbContext dbcontext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerQueryResult>   
    {
        public async Task<GetOrdersByCustomerQueryResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
        {
            var orders = await dbcontext.Orders.Include(o => o.OrderItems).AsNoTracking()
                .Where(o => o.CustomerId==query.customerId)
                .OrderBy(o => o.OrderName)
                .ToListAsync(cancellationToken);

            //var orderDto =  MapToOrderDto(orders);  
            return new GetOrdersByCustomerQueryResult(orders.ToOrderDtoList());
        }
    }
}
