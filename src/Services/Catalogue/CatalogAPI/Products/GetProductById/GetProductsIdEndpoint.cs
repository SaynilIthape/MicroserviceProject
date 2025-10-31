
using CatalogAPI.Products.GetProducts;

namespace CatalogAPI.Products.GetProductById
{
    //getproducts by id response
    public record GetProductsByIdResponse(Product Product);

    public class GetProductsIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id,ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByIdQuery(id));
                var response = result.Adapt<GetProductsByIdResponse>();
                return Results.Ok(response);
            }).WithName("GetProductsbyID")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get Products")
            .WithDescription("Retrieves a list of all products in the catalogue.");
        }
    }
}
