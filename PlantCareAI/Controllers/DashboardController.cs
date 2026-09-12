using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantCareAI.Data;
using System.Security.Claims;

namespace PlantCareAI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plants = await _context.Plants
                .Where(p => p.UserId == userId)
                .Include(p => p.WateringRecords)
                .Include(p => p.FertilizingRecords)
                .Include(p => p.HealthRecords)
                .ToListAsync();

            var totalPlants = plants.Count;

            var healthyPlants = plants.Count(p =>
                p.HealthRecords
                    .OrderByDescending(h => h.RecordedAt)
                    .FirstOrDefault()?.HealthStatus == "Healthy");

            var needsAttention = plants.Count(p =>
                p.HealthRecords
                    .OrderByDescending(h => h.RecordedAt)
                    .FirstOrDefault()?.HealthStatus == "Needs Attention");

            var totalWaterings = plants
                .SelectMany(p => p.WateringRecords)
                .Count();

            var totalFertilizations = plants
                .SelectMany(p => p.FertilizingRecords)
                .Count();

            ViewBag.TotalPlants = totalPlants;
            ViewBag.HealthyPlants = healthyPlants;
            ViewBag.NeedsAttention = needsAttention;
            ViewBag.TotalWaterings = totalWaterings;
            ViewBag.TotalFertilizations = totalFertilizations;

            return View();
        }
    }
}