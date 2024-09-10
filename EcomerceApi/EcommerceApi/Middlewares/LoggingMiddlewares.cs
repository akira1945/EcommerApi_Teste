using EcommerceApi.Data;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Middlewares
{
    public class LoggingMiddlewares
    {
        // 1 - Injetar o método next: Serve para dar Prosseguimento à requisição:
        private readonly RequestDelegate _next;
        public LoggingMiddlewares(RequestDelegate next)
        {
            _next = next;
        }

        // 2 - Criar um método Invoke ou InvokeAsync - Nome do método Obrigatorio.

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext, TokenServices tokenServices)
        {
            string? token = context.Request.Headers.Authorization;
            string? path = context.Request.Path;
            string? method = context.Request.Method;


            Console.WriteLine($"Caminho: {path}");
            Console.WriteLine($"Método: {method}");
            Console.WriteLine($"Token: {token}");

            if(path != "teste/login")
            {
                var savedToken = await dbContext.Tokens.FirstOrDefaultAsync(t => t.token == token);
                if(savedToken == null)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Acesso não autorizado");
                    return;
                }
                else
                {
                   tokenServices.Delete(savedToken.token);
                }
                
            }
            
            await _next(context);
        }
    }
    
}