
namespace CatalogAPI.Products.GetProductByCategory
{
    //public record GetProductByCategory(string category):IQuery<GetProductByCategoryResponse>;
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            
            app.MapGet("/products/category/{category}", async(string category, ISender sender)=>
            {
                var result =  await sender.Send(new GetProdudctbyCategoryQuery(category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);

            }).WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)  
            .WithSummary("Get Products by Category")
            .WithDescription("Retrieves a list of products in the catalogue by category.");
        }
    }
}
