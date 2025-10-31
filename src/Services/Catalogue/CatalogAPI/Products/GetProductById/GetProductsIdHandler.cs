
using CatalogAPI.Exceptions;

namespace CatalogAPI.Products.GetProductById
{

    public record GetProductsByIdQuery(Guid Id) : IQuery<GetProductsByIdResult>;   
     public record GetProductsByIdResult(Product? Product);
    public class GetProductsByIdQueryIdHandler(IDocumentSession session,ILogger<GetProductsByIdQueryIdHandler> logger) : IQueryHandler<GetProductsByIdQuery, GetProductsByIdResult>
    {
        public async Task<GetProductsByIdResult> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductsByIdQueryHandler for Id: {@Query}", request);
            var product= await session.LoadAsync<Product>(request.Id,cancellationToken);
            if(product is null)
            {
                throw new ProductNotFoundExceptions();
            }
            else 
            {
                return new GetProductsByIdResult(product);
            }
        }
    }
}
