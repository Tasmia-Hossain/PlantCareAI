namespace PlantCareAI.Models
{
    public class HealthRecord
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.Now;

        public string? HealthStatus { get; set; }

        public string? Symptoms { get; set; }

        public string? Notes { get; set; }

        public Plant? Plant { get; set; }
    }
}