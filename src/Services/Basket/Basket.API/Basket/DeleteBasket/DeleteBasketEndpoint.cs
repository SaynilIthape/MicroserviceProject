

using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.DeleteBasket
{
   

    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{UserName}", async (string UserName, [FromServices]ISender sender) =>
            {

                var res = sender.Send(new DeleteBasketCommand(UserName));
                return Results.Ok(new DeleteBasketResponse(true));

            }).WithName("DeleteProduct")
             .Produces<DeleteBasketResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Delete product")
             .WithDescription("Delete product in the catalogue with the provided details.");
        }
    }
}
