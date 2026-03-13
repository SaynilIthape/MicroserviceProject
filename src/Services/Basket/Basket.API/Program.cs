

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
//);
//// Register Carter
//builder.Services.AddCarter();
//builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//builder.Services.AddMarten(opt =>
//{
//    opt.Connection(builder.Configuration.GetConnectionString("MartenConnection")!);
//    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
//}).UseLightweightSessions();



//var app = builder.Build();
//app.MapCarter();

//app.Run();

using BuildingBlocks.Exceptions.Handler;
using FluentValidation;
using ImTools;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMarten(opt => {
    opt.Connection(builder.Configuration.GetConnectionString("MartenConnection")!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>(); 

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    //options.InstanceName = "BasketAPI_";
});
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapCarter();


app.UseExceptionHandler(exceptionapp =>
{
    exceptionapp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception == null)
        {
            return;
        }

        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace
        };

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);

    });
});

app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health");
app.Run();



