using System.Text.Json.Serialization;

namespace RAGImplementation
{
    public class OllamaEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public float[] Embedding { get; set; } = [];
    }
}
