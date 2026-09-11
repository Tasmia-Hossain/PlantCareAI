using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlantCareAI.Models;

namespace PlantCareAI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        public DbSet<Plant> Plants { get; set; }

        public DbSet<WateringRecord> WateringRecords { get; set; }

        public DbSet<FertilizingRecord> FertilizingRecords { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<JournalEntry> JournalEntries { get; set; }
    }
}