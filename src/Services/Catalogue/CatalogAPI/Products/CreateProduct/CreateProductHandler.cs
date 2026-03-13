
using FluentValidation;

namespace CatalogAPI.Products.CreateProduct
{

    public record CreateProductCommand
    (
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price
    ) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(a=>a.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(a=>a.Category).NotEmpty().WithMessage("At least one category is required.");    
            RuleFor(a=>a.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
            RuleFor(a=>a.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");

        }
    }
    internal class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            //var result = await validator.ValidateAsync(request, cancellationToken);
            // var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            //if (errors.Any()) {
            //    throw new ValidationException(errors.FirstOrDefault());
            //}


            var product= new Product { 
                Id=Guid.NewGuid(),
                Name=request.Name, 
                Category=request.Category,
                Description=request.Description,
                ImageFile=request.ImageFile,
                Price=request.Price
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
            
        }
    }
}
