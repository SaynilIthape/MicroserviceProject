
namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.Map("/basket/{UserName}", async (string UserName, ISender send) =>
            {
                var res = await send.Send(new GetBasketQuery(UserName));
                var response = new GetBasketResponse(res.cart);
                return Results.Ok(response);
            });
        }
    }
}
