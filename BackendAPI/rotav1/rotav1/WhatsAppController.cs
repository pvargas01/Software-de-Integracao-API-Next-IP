using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class WhatsAppController : ControllerBase
{
    private readonly WhatsAppApiClient _whatsAppApiClient;
    private const string PhoneNumberId = "472462449282510";
    private const string AccessToken = "EAB7uOlnCeBEBOyIRWVnHlQ2RseDkpeMPQNUhgEgpgBlE6L6eWFZBrhHLAnrzZBupelA4hle9tvznhuCf7a9zZCLI1QE8hWMlbseuBY5e6zABEEdix4mqgLK8gMa1jIvErPucU6ER2ZBeL7zP9FS9JGsCnyMPiVuZCUZAyv7XfZCTtSEsCEnvTVwo9jFNbmFhU8C88cAOGiocPQK70HNpIQIK0YBetQZD"; // Substitua pelo seu token de acesso

    public WhatsAppController()
    {
        _whatsAppApiClient = new WhatsAppApiClient(AccessToken);
    }

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
            return BadRequest($"Erro ao enviar mensagem: {ex.Message}");
        }
    }
    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveMessage([FromBody] WhatsAppMessageRequest request)
    {
        // Verifica se a requisição tem mensagens
        if (request != null && request.Entry != null)
        {
            foreach (var entry in request.Entry)
            {
                foreach (var messaging in entry.Messaging)
                {
                    // acessar o texto da mensagem recebida
                    var messageText = messaging.Message.Text;
                    var senderId = messaging.Sender.Id;

                    //  processar a mensagem ou enviar para o frontend
                    // enviar pro frontend (nao implementado ainda)
                    await SendToFrontend(senderId, messageText);
                }
            }
        }
        return Ok("Mensagem recebida com sucesso!");
    }
    
    // PRECISA IMPLEMENTAR AINDA// PRECISA IMPLEMENTAR AINDA// PRECISA IMPLEMENTAR AINDA
    private Task SendToFrontend(string senderId, string messageText) // PRECISA IMPLEMENTAR AINDA
    {
       
        Console.WriteLine($"Mensagem de {senderId}: {messageText}");
        return Task.CompletedTask;
    }
    // PRECISA IMPLEMENTAR AINDA// PRECISA IMPLEMENTAR AINDA// PRECISA IMPLEMENTAR AINDA

}

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
