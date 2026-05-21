using BuildingBlocks.CQRS;
using FluentValidation;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(Guid orderId) : ICommand<DeleteOrderResult>;

    public record DeleteOrderResult(bool isSuccess); 

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
           RuleFor(x => x).NotNull().WithMessage("Delete order command cannot be null.");
            RuleFor(x=>x.orderId).NotEmpty().WithMessage("Order ID is required.");  
        }
    }   

}
