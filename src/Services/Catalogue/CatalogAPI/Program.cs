var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddMarten(opt => {
    opt.Connection(builder.Configuration.GetConnectionString("MartenConnection")!);
});

var app = builder.Build();



app.MapCarter();    
app.Run();
