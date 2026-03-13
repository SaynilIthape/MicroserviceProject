
using Basket.API.Data;

namespace Basket.API.Basket.StoreBasket
{
    // Fix: Implement ICommand<StoreBasketResult> for StoreBasketCommand
    public record StoreBasketCommand(ShoppingCart Cart) :ICommand<StoreBasketResult>;
    public record StoreBasketResult(string userName);
    public class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            ShoppingCart cart = request.Cart;
            await repository.StoreBasket(cart, cancellationToken);
            return new StoreBasketResult(cart.UserName);
        }
    }
}
