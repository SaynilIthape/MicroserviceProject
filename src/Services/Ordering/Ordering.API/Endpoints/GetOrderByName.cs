using Carter;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints
{
    //public record GetOrderByNameRequest(string OrderName);   commented out as we can pass the order name as a query parameter 
    public record GetOrderByNameResponse(IEnumerable<OrderDto> OrderDtos);
    public class GetOrderByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
          app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
          {
              var query = new GetOrdersByNameQuery(orderName);
              var result = await sender.Send(query);
              var response = new GetOrderByNameResponse(result.orders);
              return Results.Ok(response);
            }).
            WithName("GetOrderByName").
            Produces<GetOrderByNameResponse>(StatusCodes.Status200OK).
            ProducesProblem(StatusCodes.Status400BadRequest).
            WithSummary("Get Order By Name").
            WithDescription("Get Order By Name");   
        }
    }
}
