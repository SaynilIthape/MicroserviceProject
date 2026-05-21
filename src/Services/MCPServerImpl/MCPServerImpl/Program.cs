using MCPServerImpl;
using MCPServerImpl.Models;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ToolExecutor>();


var app = builder.Build();

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:11434") // Ollama server URL    
};
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

app.MapPost("/ask", async (HttpContext context, ToolExecutor executor) =>
{
    var request = await context.Request.ReadFromJsonAsync<Dictionary<string,string>>();
    var question = request["question"];

    //Step 1-Define Tool
    var tool = new[]
    {
        new ToolDefinition
        {
            name = "get_discount",
            description = "Get discount for a product",
            parameters = new
            {
                type="object",
                properties = new
                {
                    productName = new
                    {
                        type = "string",
                        description = "Name of the product"
                    }
                },  
                required = new[] { "productName" }  
            }
        }
    };

    //var prompt = $$"""
    //    You are an AI assistant.

    //    You have access to a tool:

    //    Tool Name: get_discount  
    //    Description: Get discount for a product  

    //    Input:
    //    - productName (string)

    //    RULES:
    //    - If user asks about discount → respond ONLY in JSON
    //    - Do NOT answer directly

    //    FORMAT:

    //    {
    //      "tool": "get_discount",
    //      "arguments": {
    //        "productName": "<product>"
    //      }
    //    }

    //    Question: {{question}}
    //    """;


    // Step2: Send request to Ollama

    var ollamaRequest = new
    {
        model = "llama3:8b-instruct-q4_0",
        message = new
        {
            role = "user",
            content = question
        },
        tools = tool,
        Stream = false
    };

    //var ollamaRequest = new
    //{
    //    model = "llama3:8b-instruct-q4_0",
    //    prompt = prompt,
    //    stream = false,
    //    tool=tool
    //};

    //var response =  await httpClient.PostAsJsonAsync("/api/chat", ollamaRequest);
    var response = await httpClient.PostAsync("http://localhost:11434/api/generate",
               new StringContent(JsonSerializer.Serialize(ollamaRequest), Encoding.UTF8));

   var json = await response.Content.ReadAsStringAsync();

    var ollamaResponse = System.Text.Json.JsonSerializer.Deserialize<OllamaResponse>(json, new System.Text.Json.JsonSerializerOptions
    {
       // PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    }); 

    //step3 -  check tool call
    if(ollamaResponse?.message?.tool_calls != null
    && ollamaResponse.message.tool_calls.Count > 0)
    {
        var toolcall = ollamaResponse.message.tool_calls[0];

        // STEP 4: Execute tool
        var toolResult = await executor.ExecuteAsync(toolcall.function.name, toolcall.function.arguments);

        //STEP 5: Send tool result back
        var secondOllamaRequest = new
        {
            model = "llama3:8b-instruct-q4_0",
            messages = new object[]
            {
                new { role = "user", content = question },
                new { role = "tool", content = toolResult }
            },
            //tools = tool,
            //tool_calls = new[]
            //{
            //    new
            //    {
            //        function = new
            //        {
            //            name = toolcall.function.name,
            //            arguments = toolcall.function.arguments,
            //            result = result
            //        }
            //    }
            //},
            Stream = false
        };
        var finalResponse = await httpClient.PostAsJsonAsync("/api/chat", secondOllamaRequest);
        var finaljson = await finalResponse.Content.ReadAsStringAsync();
        var finalOllamaResponse = System.Text.Json.JsonSerializer.Deserialize<OllamaResponse>(finaljson,
            new System.Text.Json.JsonSerializerOptions
            {
                //PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
    }
      

    return Results.Ok(ollamaResponse?.message.content);
});  

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.Run();

//internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}

