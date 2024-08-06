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

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Caminho: {context.Request.Path}");
            Console.WriteLine($"Método: {context.Request.Method}");
            await _next(context);
        }
    }
    
}