using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

    public record UpdateOrderResult(bool isSuccess);

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Order).NotNull().WithMessage("Order cannot be null.");
            RuleFor(x => x.Order.id).NotEmpty().WithMessage("Order ID us required.");
            RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("Customer ID is required.");    
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order name is required.");  

            // Add more validation rules as needed
        }
    }   
    //public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    //{
    //    public Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    //    {
    //        // Here you would typically add logic to update the order in a database
    //        // For this example, we'll just return a success result
    //        return Task.FromResult(new UpdateOrderResult(true));
    //    }
    //}
}
