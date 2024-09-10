using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers; // SERVER

[ApiController]
[Route("websocket")]
public class WebSocketController : ControllerBase
{
    // [HttpGet("connect")]
    // public async Task<ActionResult> Connect()
    // {
    //     if(HttpContext.WebSockets.IsWebSocketRequest)
    //     {
    //         //Aceita conexão
    //         using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
    //         Console.WriteLine($"Requisição estabelecia com sucesso!!!");
    //         //Gerenciar eventos do websocket.
    //         await HandleWebSocket(HttpContext, websocket);
    //     }
    //     else
    //     {
    //         //HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    //         return BadRequest();
    //     }

    //     return Ok();


    // }

    // async Task HandleWebSocket(HttpContext context, WebSocket webSocket)
    // {
    //     //buffer
    //     var buffer = new byte[1024 * 4];
    //     // Escutar o meio CLIENTs
    //     WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);

    //     // laço que escuta se a conexão não foi fechada, ou seja, se está aberta.
    //     while(!result.CloseStatus.HasValue)
    //     {   //Processamento da mensagem recebida
    //         var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
    //         Console.WriteLine($"Mensagem recebida: {message}");

    //         result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
    //     }

    //     // fecha a conexão
    //     await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

    // }

    [HttpGet("connect")]
        public async Task<ActionResult> Connect()
        {

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {

                // Aceitar conexão
                using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Requisição estabelecidada com sucesso.");
                // Gerenciar eventos do websocket
                await HandleWebSocket(HttpContext, websocket);

            }
            else
            {
                // HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                return BadRequest();
            }

            return Ok();

        }

        async Task HandleWebSocket(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Mensagem recebida: {message}");

                if (message == "criar_usuario")
                {
                    Console.WriteLine("Fazendo consulta para criar um usuário no BD...");
                    var responseMessage = "Usuário criado com sucesso";
                    var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
                if (message == "listar_usuarios")
                {
                    Console.WriteLine("Fazendo consulta para listar usuários do BD...");
                    var responseMessage = "[usuario1, usuario2 ]";
                    var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

        }

    }



