using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class WhatsAppApiClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://graph.facebook.com/v17.0/";

    public WhatsAppApiClient(string accessToken)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }

    public async Task SendMessageAsync(string phoneNumberId, string to, string messageBody)
    {
        var message = new
        {
            messaging_product = "whatsapp",
            to = to,
            type = "text",
            text = new { body = messageBody }
        };

        var jsonMessage = JsonConvert.SerializeObject(message);
        var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

        var url = $"{phoneNumberId}/messages"; // Use o ID do número correto aqui
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status: {response.StatusCode}, Conteúdo: {errorContent}");
        }
    }
}