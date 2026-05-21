using BuildingBlocks.CQRS;
using MediatR;
using Ordering.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public record GetOrdersByNameQuery(string name) : IQuery<GetOrdersByNameQueryResult>;
    public record GetOrdersByNameQueryResult(IEnumerable<OrderDto> orders);

}
