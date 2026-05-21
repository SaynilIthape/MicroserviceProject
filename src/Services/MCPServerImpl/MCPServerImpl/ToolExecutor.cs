namespace MCPServerImpl
{
    public class ToolExecutor
    {
        public async Task<string> ExecuteAsync(string toolName, Dictionary<string, object> args)
        {
            if (toolName == "get_discount")
            {
                var product = args["productName"]?.ToString();

                // For now hardcoded
                return $"Discount for {product} is 10%";
            }

            return "Unknown tool";
        }
    }
}
