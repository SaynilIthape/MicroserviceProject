

using Microsoft.AspNetCore.Mvc;


namespace Basket.API.Basket.StoreBasket
{
    //public class StoreBasketRequest
    //{
    //    public ShoppingCart Cart { get; set; } = default!;
    //}
    public record StoreBasketRequest(ShoppingCart cart);
    public record StoreBasketResponse(string UserName);
   
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async ([FromBody] StoreBasketRequest request, [FromServices] MediatR.ISender sender) =>
            {
                //var command = request.cart.Adapt<StoreBasketCommand>();
                var command = new StoreBasketCommand(request.cart);
                var result = await sender.Send(command);
                var response = result.Adapt<StoreBasketResponse>(); //new StoreBasketResponse(result.userName);

                return Results.Created($"/basket/{response.UserName}", response);
            }).WithName("CreateBasket")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new Basket")
            .WithDescription("Creates a new Basket in the catalogue with the provided details.");
        }
    }
}
