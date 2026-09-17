using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantCareAI.Data;
using PlantCareAI.Models;
using PlantCareAI.Services;
using System.Security.Claims;

namespace PlantCareAI.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AiPlantService _aiPlantService;

        public AIController(
            ApplicationDbContext context,
            AiPlantService aiPlantService)
        {
            _context = context;
            _aiPlantService = aiPlantService;
        }

        [HttpGet]
        public async Task<IActionResult> PlantHealth(int plantId)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == plantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            var model = new AiPlantAnalysisViewModel
            {
                PlantId = plant.Id,
                PlantName = plant.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlantHealth(
            AiPlantAnalysisViewModel model)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == model.PlantId &&
                         p.UserId == userId);

            if (plant == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.PlantName = plant.Name;
                return View(model);
            }

            try
            {
                var rawResult =
                    await _aiPlantService.AnalyzePlantHealthAsync(
                        plant.Name,
                        model.Symptoms,
                        model.Environment,
                        model.AdditionalNotes);

                model.PlantName = plant.Name;

                model.AnalysisResult =
                    Markdown.ToHtml(rawResult);

                return View(model);
            }
            catch
            {
                model.PlantName = plant.Name;

                ModelState.AddModelError(
                    "",
                    "The AI service is temporarily unavailable. Please try again later.");

                return View(model);
            }
        }
    }
}