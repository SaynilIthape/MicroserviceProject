using Marten.Linq.QueryHandlers;

namespace CatalogAPI.Products.GetProductByCategory
{

    public record GetProdudctbyCategoryQuery(string category):IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> product);

    public class GetProductByCategoryQueryHandler(IDocumentSession session,ILogger<GetProductByCategoryQueryHandler> logger) : 
        IQueryHandler<GetProdudctbyCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProdudctbyCategoryQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryQueryHandler.hanlde called with {Query}", request);

            var response = await session.Query<Product>().Where(a => a.Category.Contains(request.category))
                .ToListAsync(cancellationToken);

            return new GetProductByCategoryResult(response);
                
        }
    }
}
