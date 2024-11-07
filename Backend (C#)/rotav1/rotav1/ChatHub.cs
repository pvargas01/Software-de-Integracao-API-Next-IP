using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string recipientId, string messageText)
    {
        // Envia a mensagem para todos os clientes conectados
        await Clients.All.SendAsync("ReceiveMessage", new { SenderId = "Server", MessageText = messageText });
    }
}
