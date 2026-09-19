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


        // =========================================================
        // WATERING
        // =========================================================

        // GET: Care/AddWatering/5
        [HttpGet]
        public async Task<IActionResult> AddWatering(int plantId)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == plantId &&
                         p.UserId == userId);

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


        // POST: Care/AddWatering
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWatering(
            WateringRecord record)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

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


        // GET: Care/EditWatering/5
        [HttpGet]
        public async Task<IActionResult> EditWatering(int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var record =
                await _context.WateringRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (record == null)
                return NotFound();

            ViewBag.PlantName =
                record.Plant!.Name;

            return View(record);
        }


        // POST: Care/EditWatering
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWatering(
            int id,
            WateringRecord record)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var existing =
                await _context.WateringRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (existing == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.PlantName =
                    existing.Plant!.Name;

                return View(record);
            }

            existing.WateredAt =
                record.WateredAt;

            existing.Notes =
                record.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = existing.PlantId });
        }


        // POST: Care/DeleteWatering/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWatering(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var record =
                await _context.WateringRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (record == null)
                return NotFound();

            var plantId =
                record.PlantId;

            _context.WateringRecords.Remove(record);

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = plantId });
        }


        // =========================================================
        // FERTILIZER
        // =========================================================

        // GET: Care/AddFertilizer/5
        [HttpGet]
        public async Task<IActionResult> AddFertilizer(
            int plantId)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == plantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            ViewBag.PlantName =
                plant.Name;

            var record = new FertilizingRecord
            {
                PlantId = plantId,
                FertilizedAt = DateTime.Now
            };

            return View(record);
        }


        // POST: Care/AddFertilizer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFertilizer(
            FertilizingRecord record)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == record.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var newRecord =
                    new FertilizingRecord
                    {
                        PlantId = plant.Id,
                        FertilizedAt =
                            record.FertilizedAt,
                        Fertilizer =
                            record.Fertilizer,
                        Notes =
                            record.Notes
                    };

                _context.FertilizingRecords
                    .Add(newRecord);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details",
                    "Plants",
                    new { id = plant.Id });
            }

            ViewBag.PlantName =
                plant.Name;

            return View(record);
        }


        // GET: Care/EditFertilizer/5
        [HttpGet]
        public async Task<IActionResult> EditFertilizer(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var record =
                await _context.FertilizingRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (record == null)
                return NotFound();

            ViewBag.PlantName =
                record.Plant!.Name;

            return View(record);
        }


        // POST: Care/EditFertilizer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFertilizer(
            int id,
            FertilizingRecord record)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var existing =
                await _context.FertilizingRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (existing == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.PlantName =
                    existing.Plant!.Name;

                return View(record);
            }

            existing.FertilizedAt =
                record.FertilizedAt;

            existing.Fertilizer =
                record.Fertilizer;

            existing.Notes =
                record.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = existing.PlantId });
        }


        // POST: Care/DeleteFertilizer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFertilizer(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var record =
                await _context.FertilizingRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (record == null)
                return NotFound();

            var plantId =
                record.PlantId;

            _context.FertilizingRecords
                .Remove(record);

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = plantId });
        }


        // =========================================================
        // HEALTH
        // =========================================================

        // GET: Care/AddHealth/5
        [HttpGet]
        public async Task<IActionResult> AddHealth(
            int plantId)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == plantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            ViewBag.PlantName =
                plant.Name;

            var record = new HealthRecord
            {
                PlantId = plantId,
                RecordedAt = DateTime.Now
            };

            return View(record);
        }


        // POST: Care/AddHealth
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHealth(
            HealthRecord record)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == record.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var newRecord =
                    new HealthRecord
                    {
                        PlantId = plant.Id,
                        RecordedAt =
                            record.RecordedAt,
                        HealthStatus =
                            record.HealthStatus,
                        Symptoms =
                            record.Symptoms,
                        Notes =
                            record.Notes
                    };

                _context.HealthRecords
                    .Add(newRecord);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details",
                    "Plants",
                    new { id = plant.Id });
            }

            ViewBag.PlantName =
                plant.Name;

            return View(record);
        }


        // GET: Care/EditHealth/5
        [HttpGet]
        public async Task<IActionResult> EditHealth(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var record =
                await _context.HealthRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (record == null)
                return NotFound();

            ViewBag.PlantName =
                record.Plant!.Name;

            return View(record);
        }


        // POST: Care/EditHealth
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHealth(
            int id,
            HealthRecord record)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var existing =
                await _context.HealthRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (existing == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.PlantName =
                    existing.Plant!.Name;

                return View(record);
            }

            existing.RecordedAt =
                record.RecordedAt;

            existing.HealthStatus =
                record.HealthStatus;

            existing.Symptoms =
                record.Symptoms;

            existing.Notes =
                record.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = existing.PlantId });
        }


        // POST: Care/DeleteHealth/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHealth(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var record =
                await _context.HealthRecords
                    .Include(r => r.Plant)
                    .FirstOrDefaultAsync(
                        r => r.Id == id &&
                             r.Plant!.UserId == userId);

            if (record == null)
                return NotFound();

            var plantId =
                record.PlantId;

            _context.HealthRecords
                .Remove(record);

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = plantId });
        }


        // =========================================================
        // JOURNAL
        // =========================================================

        // GET: Care/AddJournal/5
        [HttpGet]
        public async Task<IActionResult> AddJournal(
            int plantId)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == plantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            ViewBag.PlantName =
                plant.Name;

            var entry = new JournalEntry
            {
                PlantId = plantId,
                CreatedAt = DateTime.Now
            };

            return View(entry);
        }


        // POST: Care/AddJournal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddJournal(
            JournalEntry entry)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == entry.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var newEntry =
                    new JournalEntry
                    {
                        PlantId = plant.Id,
                        CreatedAt =
                            entry.CreatedAt,
                        Title =
                            entry.Title,
                        Content =
                            entry.Content
                    };

                _context.JournalEntries
                    .Add(newEntry);

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    "Details",
                    "Plants",
                    new { id = plant.Id });
            }

            ViewBag.PlantName =
                plant.Name;

            return View(entry);
        }


        // GET: Care/EditJournal/5
        [HttpGet]
        public async Task<IActionResult> EditJournal(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var entry =
                await _context.JournalEntries
                    .Include(e => e.Plant)
                    .FirstOrDefaultAsync(
                        e => e.Id == id &&
                             e.Plant!.UserId == userId);

            if (entry == null)
                return NotFound();

            ViewBag.PlantName =
                entry.Plant!.Name;

            return View(entry);
        }


        // POST: Care/EditJournal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJournal(
            int id,
            JournalEntry entry)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var existing =
                await _context.JournalEntries
                    .Include(e => e.Plant)
                    .FirstOrDefaultAsync(
                        e => e.Id == id &&
                             e.Plant!.UserId == userId);

            if (existing == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.PlantName =
                    existing.Plant!.Name;

                return View(entry);
            }

            existing.CreatedAt =
                entry.CreatedAt;

            existing.Title =
                entry.Title;

            existing.Content =
                entry.Content;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = existing.PlantId });
        }


        // POST: Care/DeleteJournal/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJournal(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var entry =
                await _context.JournalEntries
                    .Include(e => e.Plant)
                    .FirstOrDefaultAsync(
                        e => e.Id == id &&
                             e.Plant!.UserId == userId);

            if (entry == null)
                return NotFound();

            var plantId =
                entry.PlantId;

            _context.JournalEntries
                .Remove(entry);

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Plants",
                new { id = plantId });
        }
    }
}