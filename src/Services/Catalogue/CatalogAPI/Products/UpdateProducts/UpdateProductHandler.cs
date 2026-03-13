
using CatalogAPI.Exceptions;

namespace CatalogAPI.Products.UpdateProducts
{
    public record UpdateProductHandlerCommand(Guid id, string Name, List<string> Category, string Description,
        string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateCommandProductHandler (IDocumentSession session): ICommandHandler<UpdateProductHandlerCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductHandlerCommand request, CancellationToken cancellationToken)
        {
            var result = await session.LoadAsync<Product>(request.id,cancellationToken);
            if(result is not null)
            {
                
                result.Name = request.Name;
                result.Category = request.Category;
                result.Description = request.Description;
                result.ImageFile = request.ImageFile;
                result.Price = result.Price;
                session.Update(result);
                await session.SaveChangesAsync();
                return new UpdateProductResult(true);
            }
            else
            {

                throw new ProductNotFoundExceptions();
            }

        }
    }
}
