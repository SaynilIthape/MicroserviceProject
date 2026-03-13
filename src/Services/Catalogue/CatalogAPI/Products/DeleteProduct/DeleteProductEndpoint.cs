
using CatalogAPI.Products.UpdateProducts;

namespace CatalogAPI.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapDelete("/products/{id}",async(Guid Id, ISender sender)=>
           {

               var res = sender.Send(new DeleteProductCommand(Id));
               return Results.Ok(res.Adapt<DeleteProductResponse>());

           }).WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete product")
            .WithDescription("Delete product in the catalogue with the provided details.");
        }
    }
}
