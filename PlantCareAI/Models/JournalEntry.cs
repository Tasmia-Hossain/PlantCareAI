using System.ComponentModel.DataAnnotations;

namespace PlantCareAI.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter a title.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please write something in your journal entry.")]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public Plant? Plant { get; set; }
    }
}