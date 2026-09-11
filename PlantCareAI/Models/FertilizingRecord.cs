namespace PlantCareAI.Models
{
    public class FertilizingRecord
    {
        public int Id { get; set; }

        public int PlantId { get; set; }

        public DateTime FertilizedAt { get; set; } = DateTime.Now;

        public string? Fertilizer { get; set; }

        public string? Notes { get; set; }

        public Plant? Plant { get; set; }
    }
}