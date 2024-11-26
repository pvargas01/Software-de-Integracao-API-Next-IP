using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

[Route("api/[controller]")]
[ApiController]
public class WhatsAppController : ControllerBase
{
    private readonly WhatsAppApiClient _whatsAppApiClient;
    private readonly IHubContext<ChatHub> _hubContext;

    private const string PhoneNumberId = "idNumero";
    private const string AccessToken = "tokenDeAcesso"; 
    private const string VerifyToken = "senha";

    public WhatsAppController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
        _whatsAppApiClient = new WhatsAppApiClient(AccessToken);
    }

    // Método para verificar o webhook
    [HttpGet("webhook")]
    public IActionResult VerifyWebhook([FromQuery] WebhookVerification verification)
    {
        // Verifica parâmetros
        Console.WriteLine($"hubMode: {verification.HubMode}");
        Console.WriteLine($"hubChallenge: {verification.HubChallenge}");
        Console.WriteLine($"hubVerifyToken: {verification.HubVerifyToken}");
    
        if (verification.HubVerifyToken == VerifyToken)
        {
            return Ok(verification.HubChallenge);
        }
        else
        {
            Console.WriteLine("Token de verificação inválido");
            return Unauthorized();
        }
    }

    // Método para enviar mensagens
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.MessageBody))
        {
            return BadRequest("O corpo da requisição está inválido.");
        }

        try
        {
            await _whatsAppApiClient.SendMessageAsync(PhoneNumberId, request.To, request.MessageBody);
            return Ok("Mensagem enviada com sucesso!");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            return BadRequest($"Erro ao enviar mensagem: {ex.Message}");
        }
    }

    // Método para receber mensagens
    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveMessage([FromBody] WhatsAppMessageRequest request)
    {
        if (request != null && request.Entry != null)
        {
            foreach (var entry in request.Entry)
            {
                foreach (var messaging in entry.Messaging)
                {
                    var messageText = messaging.Message.Text;
                    var senderId = messaging.Sender.Id;

                    // Envia a mensagem recebida ao frontend usando SignalR
                    await SendToFrontend(senderId, messageText);
                }
            }
            return Ok("Mensagem recebida com sucesso!");
        }

        Console.WriteLine("Requisição de mensagem recebida inválida.");
        return BadRequest("Requisição inválida.");
    }

    // Método para enviar mensagens para o frontend usando SignalR
    private async Task SendToFrontend(string senderId, string messageText)
    {
        var message = new
        {
            SenderId = senderId,
            MessageText = messageText
        };

        // Envia a mensagem para todos os clientes conectados ao SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}


// Classes auxiliares para o corpo da requisição
public class SendMessageRequest
{
    public string To { get; set; }
    public string MessageBody { get; set; }
}

public class WhatsAppMessageRequest
{
    public List<WhatsAppEntry> Entry { get; set; }
}

public class WhatsAppEntry
{
    public string Id { get; set; }
    public List<WhatsAppMessaging> Messaging { get; set; }
}

public class WhatsAppMessaging
{
    public WhatsAppSender Sender { get; set; }
    public WhatsAppMessage Message { get; set; }
}

public class WhatsAppSender
{
    public string Id { get; set; }
}

public class WhatsAppMessage
{
    public string Text { get; set; }
}

// Classe para capturar os parâmetros do webhook
public class WebhookVerification
{
    [FromQuery(Name = "hub.mode")]
    public string HubMode { get; set; }

    [FromQuery(Name = "hub.challenge")]
    public string HubChallenge { get; set; }

    [FromQuery(Name = "hub.verify_token")]
    public string HubVerifyToken { get; set; }
}




