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

        public PlantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Plants
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plants = await _context.Plants
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return View(plants);
        }

        // GET: Plants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        // GET: Plants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            if (ModelState.IsValid)
            {
                plant.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                _context.Add(plant);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(plant);
        }

        // GET: Plants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        // POST: Plants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plant plant)
        {
            if (id != plant.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingPlant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (existingPlant == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingPlant.Name = plant.Name;
                existingPlant.Species = plant.Species;
                existingPlant.Description = plant.Description;
                existingPlant.Location = plant.Location;
                existingPlant.SunlightRequirement = plant.SunlightRequirement;
                existingPlant.WateringFrequency = plant.WateringFrequency;
                existingPlant.DateAdded = plant.DateAdded;
                existingPlant.ImageUrl = plant.ImageUrl;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(plant);
        }

        // GET: Plants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        // POST: Plants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plant == null)
            {
                return NotFound();
            }

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}