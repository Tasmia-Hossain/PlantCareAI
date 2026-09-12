using System.ComponentModel.DataAnnotations;

namespace PlantCareAI.Models
{
    public class FertilizingRecord
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        [Required]
        [Display(Name = "Fertilized At")]
        public DateTime FertilizedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter which fertilizer was used.")]
        [StringLength(100)]
        public string? Fertilizer { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public Plant? Plant { get; set; }
    }
}