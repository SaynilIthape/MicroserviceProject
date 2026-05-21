namespace MCPServerImpl.Models
{
     public class ToolDefinition
    {
        public string name { get; set; }
        public string description { get; set; }
        public object parameters { get; set; }
    }

    public class OllamaResponse
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
        public List<ToolCall> tool_calls { get; set; }
    }

    public class ToolCall
    {
        public FunctionCall function { get; set; }
    }

    public class FunctionCall
    {
        public string name { get; set; }
        public Dictionary<string, object> arguments { get; set; }
    }

}
