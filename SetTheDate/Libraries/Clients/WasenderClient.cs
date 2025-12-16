using SetTheDate.Libraries.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class WasenderClient
{
    private readonly HttpClient _httpClient;
    private readonly SettingService _settingService;

    public WasenderClient(
        HttpClient httpClient,
        SettingService settingService)
    {
        _httpClient = httpClient;
        _settingService = settingService;
    }

    public async Task SendMessage(string mobileNumber, string message)
    {
        var bearerToken = (await _settingService.GetSettings())
            .FirstOrDefault(x => x.Name == "wasender.bearertoken")?.Value ?? "";

        var payload = new
        {
            to = mobileNumber,
            text = message
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json")
        };

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", bearerToken);

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }
}
