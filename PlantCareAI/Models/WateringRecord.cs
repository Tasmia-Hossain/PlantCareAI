using System.ComponentModel.DataAnnotations;

namespace PlantCareAI.Models
{
    public class WateringRecord
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        [Required]
        [Display(Name = "Watered At")]
        public DateTime WateredAt { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Notes { get; set; }

        public Plant? Plant { get; set; }
    }
}