using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static EcommerceApi.DTOs.LogsDTOs;

[Route("notifications")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public NotificationController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    // Envia uma mensagem para todos os clientes conectados
    [HttpPost("send-to-all")]
    [Consumes("application/json")]
    public async Task<IActionResult> BroadcastMessage([FromBody] MessageWithoutClientDTO message)
    {
        Console.WriteLine(_hubContext.Clients);
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Servidor", message.message);
        return Ok("Mensagem enviada para todos os clientes.");
    }

    // Envia uma mensagem para um cliente específico
    [HttpPost("send-to-client")]
    [Consumes("application/json")]
    public async Task<IActionResult> SendMessageToClient([FromBody] MessageWithClientDTO message)
    {
        await _hubContext.Clients.Client(message.connectionId).SendAsync("ReceiveMessage", "Servidor", message.message);
        return Ok($"Mensagem enviada para o cliente com connectionId {message.connectionId}");
    }
}
