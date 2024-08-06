using EcommerceApi.Data;
using EcommerceApi.Controllers;
using EcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Repositories;
using System.Security.Cryptography;
using EcommerceApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RemoteConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<SellersRepository>();
builder.Services.AddScoped<ClientsRepository>();
builder.Services.AddScoped<OrdersRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<SellersServices>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<LoggingMiddlewares>();

app.MapControllers();

app.Run();
