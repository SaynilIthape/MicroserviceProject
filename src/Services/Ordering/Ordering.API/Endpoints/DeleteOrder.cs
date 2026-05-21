using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints
{
    //public record DeleteOrderRequest(Guid id);  not needed as we can pass the id as a route parameter
    public record DeleteOrderResponse(bool isSuccess);
    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
            {
                var request = await sender.Send(new DeleteOrderCommand(id));
                var response = request.Adapt<DeleteOrderResponse>();
              
                    return Results.Ok(response);
               
            }).WithName("DeleteOrder").
            Produces<DeleteOrderResponse>(StatusCodes.Status200OK).
            ProducesProblem(StatusCodes.Status400BadRequest).
            WithSummary("Delete Order").
            WithDescription("Delete Description");
        }
    }
}
