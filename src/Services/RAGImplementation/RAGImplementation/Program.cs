using Microsoft.AspNetCore.Mvc;
using RAGImplementation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


var connectionString = "Server=localhost;Port=5432;Database=catalogDb;User Id=postgres;Password=mysecret;";
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Required configuration settings are missing.");
}

//register services
builder.Services.AddSingleton<IEmbeddingGenerator>(sp =>
    new OllamaEmbeddingGenerator(new Uri("http://127.0.0.1:11434"), "phi"));

builder.Services.AddSingleton<TextRepository>(sp =>
    new TextRepository(
        connectionString,
        sp.GetRequiredService<IEmbeddingGenerator>()));

builder.Services.AddSingleton<RegService>(sp =>
    new RegService(
        sp.GetRequiredService<TextRepository>(),
        new Uri("http://127.0.0.1:11434"),
        "phi"));



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.MapPost("/chat", async ([FromBody] ChatRequest request) =>
//{
//var client = new HttpClient{
//    BaseAddress = new Uri("http://localhost:11434")

//};

//    var requestsend = new {
//        model = "phi",
//        prompt = request.Question,
//        stream = false
//    };

//    var response = await client.PostAsJsonAsync("/api/generate", requestsend);
//    var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
//    return Results.Ok(result.response.ToString());
//});

// Minimal API endpoints
        app.MapPost("/add-text", async (TextRepository textRepository, HttpContext context) =>
        {
            var request = await context.Request.ReadFromJsonAsync<AddTextRequest>();
            if (string.IsNullOrWhiteSpace(request?.Content))
            {
                return Results.BadRequest("Content is required.");
            }
            await textRepository.StoreTextAsync(request.Content);

            return Results.Ok("Text added successfully");
        });

app.MapPost("/ask", async (RegService regService, HttpContext context) =>
{
    var request = await context.Request.ReadFromJsonAsync<AskRequest>();

    if (string.IsNullOrWhiteSpace(request?.Question))
    {
        return Results.BadRequest("Question is required.");
    }

    var response = await regService.GetAnswersAsync(request.Question);
    return Results.Ok(response);
});


app.Run();

public class OllamaResponse
{
    public Message message { get; set; }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

public class AddTextRequest
{
    public string Content { get; set; }
}
public record AskRequest(string Question);