


namespace CatalogAPI.Products.GetProducts
{

    public record GetProductsQuery(): IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);

    public class GetProductsQueryHandler(IDocumentSession documentSession,ILogger<GetProductsQueryHandler> logger):
        IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetProductsQuery");
            var products = await documentSession
                .Query<Product>()
                .ToListAsync(cancellationToken);
            //var products = new List<Product>()  ;
            logger.LogInformation("Retrieved {Count} products", products.Count);
            return new GetProductsResult(products);
        }
    
    }
}
