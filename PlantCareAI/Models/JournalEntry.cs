namespace PlantCareAI.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public Plant? Plant { get; set; }
    }
}