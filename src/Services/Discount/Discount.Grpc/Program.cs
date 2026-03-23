using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectString = builder.Configuration.GetConnectionString("Database");
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountDBContext>(a => a.UseSqlite
("Data Source=/app/data/discountDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMigrationDB();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
