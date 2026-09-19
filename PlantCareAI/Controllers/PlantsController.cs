using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantCareAI.Data;
using PlantCareAI.Models;
using System.Security.Claims;

namespace PlantCareAI.Controllers
{
    [Authorize]
    public class PlantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PlantsController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Plants
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plants = await _context.Plants
                .Where(p => p.UserId == userId)
                .Include(p => p.WateringRecords)
                .Include(p => p.HealthRecords)
                .OrderByDescending(p => p.DateAdded)
                .ToListAsync();

            return View(plants);
        }

        // GET: Plants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .Include(p => p.WateringRecords)
                .Include(p => p.FertilizingRecords)
                .Include(p => p.HealthRecords)
                .Include(p => p.JournalEntries)
                .FirstOrDefaultAsync(
                    p => p.Id == id &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            return View(plant);
        }

        // GET: Plants/Create
        public IActionResult Create()
        {
            return View(new Plant
            {
                DateAdded = DateTime.Now
            });
        }

        // POST: Plants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Plant plant,
            IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    plant.ImageUrl =
                        await SavePlantImageAsync(imageFile);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(
                        "ImageUrl",
                        ex.Message);

                    return View(plant);
                }

                plant.UserId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier)!;

                _context.Plants.Add(plant);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(plant);
        }

        // GET: Plants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == id &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            return View(plant);
        }

        // POST: Plants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Plant plant,
            IFormFile? imageFile)
        {
            if (id != plant.Id)
                return NotFound();

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var existingPlant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == id &&
                         p.UserId == userId);

            if (existingPlant == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingPlant.Name =
                    plant.Name;

                existingPlant.PlantType =
                    plant.PlantType;

                existingPlant.Description =
                    plant.Description;

                existingPlant.Location =
                    plant.Location;

                existingPlant.SunlightRequirement =
                    plant.SunlightRequirement;

                existingPlant.WateringFrequency =
                    plant.WateringFrequency;

                existingPlant.DateAdded =
                    plant.DateAdded;

                // Replace image only if a new image was uploaded
                if (imageFile != null &&
                    imageFile.Length > 0)
                {
                    try
                    {
                        var newImagePath =
                            await SavePlantImageAsync(
                                imageFile);

                        DeletePlantImage(
                            existingPlant.ImageUrl);

                        existingPlant.ImageUrl =
                            newImagePath;
                    }
                    catch (InvalidOperationException ex)
                    {
                        ModelState.AddModelError(
                            "ImageUrl",
                            ex.Message);

                        return View(plant);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(Index));
            }

            return View(plant);
        }

        // GET: Plants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .Include(p => p.WateringRecords)
                .Include(p => p.FertilizingRecords)
                .Include(p => p.HealthRecords)
                .Include(p => p.JournalEntries)
                .FirstOrDefaultAsync(
                    p => p.Id == id &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            return View(plant);
        }

        // POST: Plants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == id &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            // Delete uploaded image from wwwroot
            DeletePlantImage(plant.ImageUrl);

            _context.Plants.Remove(plant);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // Image Upload Helpers
        // =========================================================

        private async Task<string?> SavePlantImageAsync(
            IFormFile? imageFile)
        {
            if (imageFile == null ||
                imageFile.Length == 0)
            {
                return null;
            }

            var allowedExtensions =
                new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

            var extension =
                Path.GetExtension(
                    imageFile.FileName)
                    .ToLowerInvariant();

            if (!allowedExtensions.Contains(
                    extension))
            {
                throw new InvalidOperationException(
                    "Only JPG, JPEG, PNG, and WEBP images are allowed.");
            }

            const long maxFileSize =
                5 * 1024 * 1024;

            if (imageFile.Length > maxFileSize)
            {
                throw new InvalidOperationException(
                    "Image size must be 5 MB or smaller.");
            }

            var uploadFolder =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "plants");

            Directory.CreateDirectory(
                uploadFolder);

            var fileName =
                $"{Guid.NewGuid():N}{extension}";

            var filePath =
                Path.Combine(
                    uploadFolder,
                    fileName);

            await using var stream =
                new FileStream(
                    filePath,
                    FileMode.Create);

            await imageFile.CopyToAsync(stream);

            return $"/uploads/plants/{fileName}";
        }


        private void DeletePlantImage(
            string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(
                    imagePath))
            {
                return;
            }

            // Only delete files created by our upload system.
            if (!imagePath.StartsWith(
                    "/uploads/plants/",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fileName =
                Path.GetFileName(imagePath);

            if (string.IsNullOrWhiteSpace(
                    fileName))
            {
                return;
            }

            var filePath =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "plants",
                    fileName);

            if (System.IO.File.Exists(
                    filePath))
            {
                System.IO.File.Delete(
                    filePath);
            }
        }
    }
}