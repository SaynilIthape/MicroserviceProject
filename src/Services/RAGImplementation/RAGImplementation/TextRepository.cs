using Npgsql;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace RAGImplementation
{
    public class TextRepository(string connectionString, IEmbeddingGenerator embeddingGenerator)
    {
        private readonly string _connectionString = connectionString;
        private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;

        public async Task StoreTextAsync(string text)
        {

            var embedding = await _embeddingGenerator.GenerateEmbeddingAsync(text);

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            // Here you would execute an SQL command to insert the text and embedding into your database
            string query = "INSERT INTO text_contexts (content, embedding) VALUES (@content, @embedding)";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@content", text);
            command.Parameters.AddWithValue("@embedding", embedding);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<string>> RetrieveRelevantText(string input)
        {
            var queryEmbedding = await _embeddingGenerator.GenerateEmbeddingAsync(input);

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            //string querySql = @"SELECT content FROM text_contexts WHERE embedding <-> CAST(@queryEmbedding AS vector) > 0.7 ORDER BY embedding <-> CAST(@queryEmbedding AS vector) LIMIT 5";
            string querySql = @"SELECT content
                                FROM text_contexts
                                ORDER BY embedding <-> CAST(@queryEmbedding AS vector)
                                LIMIT 3";

            //string querySql = @"SELECT content,
            //                   embedding <=> CAST(@queryEmbedding AS vector) AS score
            //            FROM text_contexts
            //            ORDER BY score";


            using var command = new NpgsqlCommand(querySql, connection);
            string embeddingString = $"[{string.Join(",", queryEmbedding.Select(v => v.ToString("G", CultureInfo.InvariantCulture)))}]";

            command.Parameters.AddWithValue("@queryEmbedding", embeddingString);

            var results = new List<string>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(reader.GetString(reader.GetOrdinal("content")));
            }
            return results.Count > 0 ? results : new List<string> { "No relevant context found." };
        }
    }


    public class TextContext
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public float[] Embedding { get; set; } = [];
    }

}