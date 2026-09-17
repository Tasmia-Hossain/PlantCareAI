using System.ComponentModel.DataAnnotations;

namespace PlantCareAI.Models
{
    public class AiPlantAnalysisViewModel
    {
        public int PlantId { get; set; }

        public string PlantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please describe the symptoms.")]
        [StringLength(1000)]
        public string Symptoms { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please describe the growing environment.")]
        [StringLength(500)]
        public string Environment { get; set; } = string.Empty;

        [StringLength(500)]
        public string? AdditionalNotes { get; set; }

        public string? AnalysisResult { get; set; }
    }
}