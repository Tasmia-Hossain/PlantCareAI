using Google.GenAI;

namespace PlantCareAI.Services
{
    public class GeminiPlantService
    {
        private readonly Client _client;

        public GeminiPlantService()
        {
            var apiKey = System.Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("GEMINI_API_KEY is not configured.");
            }

            _client = new Client(apiKey: apiKey);
        }

        public async Task<string> AnalyzePlantHealthAsync(
            string plantName,
            string symptoms,
            string environment,
            string? additionalNotes = null)
        {
            var notesSection = string.IsNullOrWhiteSpace(additionalNotes)
                ? "No additional notes were provided."
                : $"""
                  Additional notes:
                  {additionalNotes}
                  """;

            var prompt = $"""
                You are an AI plant care assistant.

                Help the user understand possible causes of the
                plant's reported symptoms and provide practical
                general care guidance.

                Do not claim to provide a definitive diagnosis.

                Plant name:
                {plantName}

                Reported symptoms:
                {symptoms}

                Growing environment:
                {environment}

                {notesSection}

                Return a clear, practical and structured analysis
                using Markdown.

                Use exactly these sections:

                ## 1. Plant Overview
                Briefly describe the plant based on the provided
                plant name. Do not invent specific information if
                the plant identity is unclear.

                ## 2. Possible Causes
                List the most likely possible causes of the reported
                symptoms.

                Prioritize the most relevant causes first.

                ## 3. Recommended Actions
                Give practical steps the user can take to help the
                plant.

                Use concise bullet points.

                ## 4. Watering Guidance
                Explain what the user should check regarding
                watering, soil moisture and drainage based on the
                reported symptoms.

                ## 5. Environment & Light
                Provide relevant suggestions regarding light,
                temperature, humidity or placement when appropriate.

                ## 6. What to Monitor
                List signs the user should monitor over the next
                several days.

                ## 7. When to Seek Further Help
                Explain when the user should consider getting
                additional expert advice or inspecting the plant
                more closely.

                Important rules:
                - Do not provide a definitive plant disease diagnosis.
                - Clearly use phrases such as "possible cause" when
                  discussing potential problems.
                - Do not invent symptoms that the user did not provide.
                - Do not assume information that is not present.
                - Keep recommendations practical for a home plant owner.
                - Prioritize the most relevant actions.
                - Avoid generic motivational content.
                - Keep the response concise and easy to understand.

                Important disclaimer:
                End the response with a short note explaining that
                the AI-generated information is general plant-care
                guidance and is not a definitive diagnosis.

                User's plant information:
                {plantName}
                """;

            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.6-flash",
                contents: prompt
            );

            return response.Candidates?[0]?.Content?.Parts?[0]?.Text
                   ?? "No plant health analysis was generated.";
        }
    }
}