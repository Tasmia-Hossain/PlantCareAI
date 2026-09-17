using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PlantCareAI.Services
{
    public class AiPlantService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AiPlantService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            _apiKey = configuration["Groq:ApiKey"]
                ?? throw new Exception("Groq:ApiKey is not configured in User Secrets.");

            _httpClient.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> AnalyzePlantHealthAsync(
            string plantName,
            string symptoms,
            string environment,
            string? additionalNotes)
        {
            var prompt =
                $"""
                You are a plant care assistant helping a user understand possible causes
                of plant health problems.

                Plant:
                {plantName}

                Symptoms:
                {symptoms}

                Growing Environment:
                {environment}

                Additional Notes:
                {additionalNotes ?? "None"}

                Provide a concise, practical response in Markdown.

                Use exactly these sections:

                ## Possible Causes

                Explain 2-4 plausible causes based only on the information provided.
                Clearly indicate that these are possibilities, not a definitive diagnosis.

                ## Recommended Actions

                Give practical steps the user can take safely.

                ## Care Tips

                Give general care suggestions relevant to the plant and symptoms.

                Important:
                - Do not provide a definitive diagnosis.
                - Do not claim certainty when the information is incomplete.
                - Use phrases such as "possible cause", "may be related to", or "could indicate".
                - Do not recommend dangerous chemicals or unsafe treatments.
                - If the symptoms could have multiple causes, explain that briefly.
                - Keep the response concise and easy to understand.
                """;

            var requestBody = new
            {
                model = "openai/gpt-oss-120b",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Groq API error: {response.StatusCode} - {errorText}");
            }

            var result = await response.Content.ReadFromJsonAsync<GroqChatResponse>();

            return result?.Choices?.FirstOrDefault()?.Message?.Content
                ?? "No response received from AI service.";
        }
    }

    public class GroqChatResponse
    {
        [JsonPropertyName("choices")]
        public List<GroqChoice>? Choices { get; set; }
    }

    public class GroqChoice
    {
        [JsonPropertyName("message")]
        public GroqMessage? Message { get; set; }
    }

    public class GroqMessage
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}