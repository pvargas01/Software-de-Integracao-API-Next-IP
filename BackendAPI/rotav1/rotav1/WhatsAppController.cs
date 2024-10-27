using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class WhatsAppController : ControllerBase
{
    private readonly WhatsAppApiClient _whatsAppApiClient;
    private const string PhoneNumberId = "474526989073148";
    private const string AccessToken = "EAAhbIYhC26sBO62nuF2S9dKT4NUlGiwfpCirwNy83ahmH0ztHV7qBbsr0Wbx71UZAIuTZAXYO09JG00qZAbZBjKRZBTSfKjbVvdRRefU7LWwcaA5B8XW7s4a1colaFSbCCkN0qb1llVrsLdlGDODuw84cStrTkgMOeR9BZBkxh3aVxVdde7DcsNJLrVZCfNqGycYSGhhdLS7QNnoYJVD9dKJ9l3teEZD"; // Substitua pelo seu token de acesso

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
}

public class SendMessageRequest
{
    public string To { get; set; }
    public string MessageBody { get; set; }
}