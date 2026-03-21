using System.Text;
using System.Text.Json;

namespace RAGImplementation
{
    public class RegService(TextRepository textRepository, Uri Ollamauri, string modelId = "Phi")
    {

        private readonly TextRepository _textRepository = textRepository;
        private readonly Uri _ollamaUri = Ollamauri;
        private readonly string _modelId = modelId;
        private readonly HttpClient client = new();

        public async Task<object> GetAnswersAsync(string question)
        {
            // Retrieve multiple relevant texts
            List<string> contexts = await _textRepository.RetrieveRelevantText(question);

            // Combine multiple contexts into one string
            string combinedContext = string.Join("\n\n---\n\n", contexts);

            if (contexts.Count == 1 && contexts[0] == "No relevant context found.")
            {
                return new
                {
                    Context = "No relevant data found in the database.",
                    Response = "I don't know."
                };
            }

            //var requestBody = new
            //{
            //    model = _modelId,
            //    prompt = $"""
            //    You are an AI assistant. You MUST answer using the provided context. 
            //    If the answer is not fully in context, try to answer using context + general knowledge. "

            //    Context:
            //    {combinedContext}

            //    Question: {question}
            //    """,
            //    stream = false
            //};

            var prompt = $@"
                You are an AI assistant.

                Use the context below to answer the question.
                If the answer is not fully in context, try to answer using context + general knowledge.

                Context:
                {combinedContext}

                Question:
                {question}

                Answer:
                ";


            var requestBody = new
            {
                model = _modelId,
                messages = new[]
                {
                 new { role = "user", content = prompt }
                },
                stream = false
            };

            var response = await client.PostAsync(new Uri(_ollamaUri, "/api/chat"),
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                return new
                {
                    Context = combinedContext,
                    Response = "Error: Unable to generate response."
                };
            }
            var res = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Ollama Response: " + res);
            var serializationoption = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            //var completionResponse = JsonSerializer.Deserialize<OllamaResponse>(res, serializationoption);
            var completionResponse = JsonSerializer.Deserialize<OllamaResponse>(res, serializationoption);

            var answer = completionResponse?.message?.content;
            return new
            {
                Context = combinedContext,
                Response = answer ?? "I don't know. No relevant data found."
            };
        }
    }
}

   