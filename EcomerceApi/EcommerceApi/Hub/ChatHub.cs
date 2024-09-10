// dotnet add package Microsoft.AspNetCore.SignalR
// SignalR -> biblioteca que abstrai conexões de websocket, simplificando a implementação
// Hub -> camada que permite ao server e ao client chamar classes uns dos outros
// Habilitar o CORS
// Adiciona SignalR ao container de serviços
// builder.Services.AddSignalR();
// Configura o endpoint para o SignalR Hub
// app.MapHub<ChatHub>("/chathub");

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    // Método para enviar mensagem para todos os clientes
    public async Task SendMessageToAll(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Método para enviar uma mensagem para um cliente específico
    public async Task SendMessageToClient(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
    }

    // Método que sobrescreve o método OnConnectedAsync para tratar conexões
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        // Aqui você pode salvar o connectionId em um banco ou em memória para rastrear usuários conectados
        Console.WriteLine($"Cliente conectado: {connectionId}");
        await base.OnConnectedAsync();
    }

    // Método que sobrescreve o método OnDisconnectedAsync para tratar desconexões
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        // Aqui você pode remover o connectionId do armazenamento quando um cliente desconectar
        Console.WriteLine($"Cliente desconectado: {connectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}
