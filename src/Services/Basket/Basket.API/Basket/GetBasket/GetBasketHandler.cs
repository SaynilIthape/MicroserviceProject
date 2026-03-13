

using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName): IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart cart);

    public class GetBasketQueryHandler(IBasketRepository repository, ILogger<GetBasketQueryHandler> logger) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetBasketQuery for UserName: {UserName}", request.UserName);
           var response = await repository.GetBasket(request.UserName, cancellationToken);
            logger.LogInformation("Retrieved basket for UserName: {UserName}", request.UserName);
            return new GetBasketResult(response);

        }
    }
}
