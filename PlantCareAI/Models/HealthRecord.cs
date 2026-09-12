using System.ComponentModel.DataAnnotations;

namespace PlantCareAI.Models
{
    public class HealthRecord
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        [Required]
        [Display(Name = "Recorded At")]
        public DateTime RecordedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please select a health status.")]
        public string? HealthStatus { get; set; }

        [StringLength(500)]
        public string? Symptoms { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public Plant? Plant { get; set; }
    }
}