using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using MassTransit;

namespace Basket.API.Basket.CheckOutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool Success);

    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto is required.");
            RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required.");
        }
    }
    public class CheckoutBasketCommandHandler(BasketRepository repository,
        IPublishEndpoint endpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            
            var basket= repository.GetBasket(command.BasketCheckoutDto.UserName,cancellationToken);
            if(basket == null)
            {
                return new CheckoutBasketResult(false);
            }   

            var eventMessage =  command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
            await endpoint.Publish(eventMessage, cancellationToken);
            await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);
            return new CheckoutBasketResult(true);  

        }
    }
}
