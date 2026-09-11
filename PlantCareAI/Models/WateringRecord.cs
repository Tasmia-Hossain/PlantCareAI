namespace PlantCareAI.Models
{
    public class WateringRecord
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        public DateTime WateredAt { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        public Plant? Plant { get; set; }
    }
}