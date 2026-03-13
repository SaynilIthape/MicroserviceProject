
using CatalogAPI.Products.CreateProduct;

namespace CatalogAPI.Products.UpdateProducts
{
    public record UpdateProductRequest(Guid Id,string Name, List<string> Category,string Description,
        string ImageFile, decimal Price);   

    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products",async(UpdateProductRequest request,ISender sender)=>
            {
                var command = request.Adapt<UpdateProductHandlerCommand>();
                var result =  await sender.Send(command);    
                return Results.Ok(result.Adapt<UpdateProductResponse>());

            }).WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update product")
            .WithDescription("Update product in the catalogue with the provided details.");
        }
    }
}
