namespace RAGImplementation
{
    public interface IEmbeddingGenerator
    {
        Task<float[]> GenerateEmbeddingAsync(string text);

    }
}
