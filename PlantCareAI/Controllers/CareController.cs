using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantCareAI.Data;
using PlantCareAI.Models;
using System.Security.Claims;

namespace PlantCareAI.Controllers
{
    [Authorize]
    public class CareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CareController(ApplicationDbContext context)
        {
            _context = context;
        }


        // =========================
        // WATERING
        // =========================

        [HttpGet]
        public async Task<IActionResult> AddWatering(int plantId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == plantId && p.UserId == userId);

            if (plant == null)
                return NotFound();

            ViewBag.PlantName = plant.Name;

            var record = new WateringRecord
            {
                PlantId = plantId,
                WateredAt = DateTime.Now
            };

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWatering(WateringRecord record)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == record.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var newRecord = new WateringRecord
                {
                    PlantId = plant.Id,
                    WateredAt = record.WateredAt,
                    Notes = record.Notes
                };

                _context.WateringRecords.Add(newRecord);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details",
                    "Plants",
                    new { id = plant.Id });
            }

            ViewBag.PlantName = plant.Name;

            return View(record);
        }


        // =========================
        // FERTILIZER
        // =========================

        [HttpGet]
        public async Task<IActionResult> AddFertilizer(int plantId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == plantId && p.UserId == userId);

            if (plant == null)
                return NotFound();

            ViewBag.PlantName = plant.Name;

            var record = new FertilizingRecord
            {
                PlantId = plantId,
                FertilizedAt = DateTime.Now
            };

            return View(record);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFertilizer(
            FertilizingRecord record)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == record.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.FertilizingRecords.Add(record);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details",
                    "Plants",
                    new { id = record.PlantId });
            }

            ViewBag.PlantName = plant.Name;

            return View(record);
        }


        // =========================
        // HEALTH
        // =========================

        [HttpGet]
        public async Task<IActionResult> AddHealth(int plantId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == plantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            ViewBag.PlantName = plant.Name;

            var record = new HealthRecord
            {
                PlantId = plantId,
                RecordedAt = DateTime.Now
            };

            return View(record);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHealth(
            HealthRecord record)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == record.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.HealthRecords.Add(record);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details",
                    "Plants",
                    new { id = record.PlantId });
            }

            ViewBag.PlantName = plant.Name;

            return View(record);
        }


        // =========================
        // JOURNAL
        // =========================

        [HttpGet]
        public async Task<IActionResult> AddJournal(int plantId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == plantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            ViewBag.PlantName = plant.Name;

            var entry = new JournalEntry
            {
                PlantId = plantId,
                CreatedAt = DateTime.Now
            };

            return View(entry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddJournal(
            JournalEntry entry)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == entry.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.JournalEntries.Add(entry);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details",
                    "Plants",
                    new { id = entry.PlantId });
            }

            ViewBag.PlantName = plant.Name;

            return View(entry);
        }
    }
}