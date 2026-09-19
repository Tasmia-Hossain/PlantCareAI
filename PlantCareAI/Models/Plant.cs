namespace PlantCareAI.Models
{
    public class Plant
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? PlantType { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? SunlightRequirement { get; set; }

        public string? WateringFrequency { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public string? ImageUrl { get; set; }

        public string UserId { get; set; } = string.Empty;


        // Navigation properties

        public ICollection<WateringRecord> WateringRecords { get; set; }
            = new List<WateringRecord>();

        public ICollection<FertilizingRecord> FertilizingRecords { get; set; }
            = new List<FertilizingRecord>();

        public ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();

        public ICollection<JournalEntry> JournalEntries { get; set; }
            = new List<JournalEntry>();
    }
}