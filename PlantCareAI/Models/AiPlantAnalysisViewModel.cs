namespace PlantCareAI.Models
{
    public class AiPlantAnalysisViewModel
    {
        public int PlantId { get; set; }

        public string PlantName { get; set; } = string.Empty;

        public string Symptoms { get; set; } = string.Empty;

        public string Environment { get; set; } = string.Empty;

        public string? AdditionalNotes { get; set; }

        public string? AnalysisResult { get; set; }
    }
}