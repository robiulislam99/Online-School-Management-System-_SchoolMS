using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace SchoolMS.Controllers
{
    [Authorize]
    public class AIChatController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AIChatController(
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(
            [FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
                return Json(new { message = "Please enter a message." });

            var apiKey = _config["AIChat:ApiKey"];
            var apiUrl = _config["AIChat:ApiUrl"];
            var model = _config["AIChat:Model"] ?? "llama3-8b-8192";

            if (string.IsNullOrEmpty(apiKey) ||
                apiKey == "YOUR_GROQ_API_KEY_HERE")
            {
                return Json(new
                {
                    message = "AI chat is not configured. " +
                              "Please add your Groq API key in appsettings.json."
                });
            }

            try
            {
                // Build request body exactly as Groq expects
                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new
                        {
                            role = "system",
                            content = "You are a helpful school management " +
                                      "assistant. Help with student records, " +
                                      "attendance, marks, fees, and general " +
                                      "school administration questions. " +
                                      "Keep answers concise and practical."
                        },
                        new
                        {
                            role = "user",
                            content = request.Message
                        }
                    },
                    max_tokens = 500,
                    temperature = 0.7
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                // Set Authorization header
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders
                    .Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient
                    .PostAsync(apiUrl, content);

                var responseBody = await response.Content
                    .ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response
                    using var doc = JsonDocument.Parse(responseBody);
                    var aiMessage = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();

                    return Json(new
                    {
                        message = aiMessage ?? "No response from AI."
                    });
                }
                else
                {
                    // Log the actual error from Groq
                    Console.WriteLine(
                        $"Groq API Error {response.StatusCode}: {responseBody}");

                    return Json(new
                    {
                        message = $"AI service error: {response.StatusCode}. " +
                                  "Please try again."
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI Chat Exception: {ex.Message}");
                return Json(new
                {
                    message = "Could not connect to AI service. " +
                              "Please check your internet connection."
                });
            }
        }
    }

    // Request model
    public class ChatRequest
    {
        public string? Message { get; set; }
    }
}