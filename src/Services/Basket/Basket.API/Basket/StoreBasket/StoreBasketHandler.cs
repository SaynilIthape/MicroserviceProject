
using Basket.API.Data;
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    // Fix: Implement ICommand<StoreBasketResult> for StoreBasketCommand
    public record StoreBasketCommand(ShoppingCart Cart) :ICommand<StoreBasketResult>;
    public record StoreBasketResult(string userName);
    public class StoreBasketCommandHandler
        (IBasketRepository repository,DiscountProtoService.DiscountProtoServiceClient discount ) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            ShoppingCart cart = request.Cart;
            foreach (var item in cart.Items)
            {
                var coupon= await discount.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
                item.Price -= coupon.Amount;  
            }
            await repository.StoreBasket(cart, cancellationToken);
            return new StoreBasketResult(cart.UserName);
        }
    }
}
