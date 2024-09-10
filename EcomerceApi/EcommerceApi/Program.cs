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
builder.Services.AddScoped<TokenServices>();
builder.Services.AddScoped<NotificationController>();
builder.Services.AddScoped<WebSocketController>();
//builder.Services.AddScoped<LoginsController>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddSignalR();


builder.Services.AddControllers();

var app = builder.Build();

//app.UseMiddleware<LoggingMiddlewares>();

app.UseCors();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "ecommerceApi");
//     });
// }

app.UseWebSockets();

app.MapHub<ChatHub>("/chathub");

app.MapControllers();

app.Run();
