using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;


namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;
    
    public record CreateOrderResult(Guid OrderId);  

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Order).NotNull().WithMessage("Order details must be provided.");
            RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("At least one order item is required.");
            // Additional validation rules can be added here
        }
    }
    //public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    //{
    //    public Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    //    {
    //        // Here you would typically add logic to save the order to a database
    //        // For this example, we'll just return a new OrderId
    //        var newOrderId = Guid.NewGuid(); // Simulate creating a new order and getting its ID
    //        return Task.FromResult(new CreateOrderResult(newOrderId));
    //    }
    //}

}
