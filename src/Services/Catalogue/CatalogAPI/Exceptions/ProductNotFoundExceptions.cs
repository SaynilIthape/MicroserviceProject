namespace CatalogAPI.Exceptions
{
    public class ProductNotFoundExceptions:Exception
    {
        public ProductNotFoundExceptions():base("The product with the specified ID was not found.")
        {
        }
    }
}
