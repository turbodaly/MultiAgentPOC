using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class OpenAIService
{
    private readonly string _endpoint;
    private readonly string _apiKey;
    private readonly ILogger<OpenAIService> _logger;

    public OpenAIService(IConfiguration configuration, ILogger<OpenAIService> logger)
    {
        _endpoint = configuration["AzureOpenAI:Endpoint"] ?? throw new ArgumentNullException(nameof(configuration));
        _apiKey = configuration["AzureOpenAI:Key"] ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger;
    }

    public async Task<string> CallOpenAIAsync(string prompt)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

            var requestBody = new
            {
                messages = new[] { new { role = "system", content = "You are an AI assistant that helps people find information." }, new { role = "user", content = prompt } },
                max_tokens = 800,
                temperature = 0.7,
                frequency_penalty = 0,
                presence_penalty = 0,
                top_p = 0.95,
                stop = (string)null
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending request to OpenAI endpoint: {Endpoint}", _endpoint);
            var response = await httpClient.PostAsync(_endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Request to OpenAI endpoint failed. Status Code: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                response.EnsureSuccessStatusCode();
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
