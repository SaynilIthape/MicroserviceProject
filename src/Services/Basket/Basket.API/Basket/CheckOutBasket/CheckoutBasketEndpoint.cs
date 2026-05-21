using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;

namespace Basket.API.Basket.CheckOutBasket
{
    public record CheckoutBasketEndpointRequest(BasketCheckoutDto basketCheckoutDto);
    public record CheckoutBasketEndpointResponse(bool Success);

    public class CheckoutBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/bakset/checkout", async (CheckoutBasketEndpointRequest request, ISender sender) =>
            {
                var command = new CheckoutBasketCommand(request.basketCheckoutDto);
                var result = await sender.Send(command);
                var response = result.Adapt<CheckoutBasketEndpointResponse>();
                return Results.Ok(response);
            }).WithName("CheckoutBasket").
            Produces<CheckoutBasketEndpointResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Checkout Basket") 
            .WithDescription("Checkout Basket") 
                ;
        }
    }
}
