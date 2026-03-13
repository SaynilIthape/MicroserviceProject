

namespace BuildingBlocks.Exceptions
{
    public class InternalServerErroException:Exception
    {
        public InternalServerErroException(string message) : base(message)
        {
        }
        public InternalServerErroException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
