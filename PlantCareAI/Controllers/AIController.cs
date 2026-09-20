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


        // GET: AI/PlantHealth?plantId=5
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
            {
                return NotFound();
            }

            var model = new AiPlantAnalysisViewModel
            {
                PlantId = plant.Id,
                PlantName = plant.Name
            };

            return View(model);
        }


        // POST: AI/PlantHealth
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlantHealth(
            AiPlantAnalysisViewModel model)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            // Verify that the selected plant belongs
            // to the currently logged-in user.
            var plant = await _context.Plants
                .FirstOrDefaultAsync(
                    p => p.Id == model.PlantId &&
                         p.UserId == userId);

            if (plant == null)
            {
                return NotFound();
            }


            // Preserve the plant name when validation fails.
            if (!ModelState.IsValid)
            {
                model.PlantName = plant.Name;

                return View(model);
            }


            try
            {
                // Send plant information to the AI service.
                var rawResult =
                    await _aiPlantService.AnalyzePlantHealthAsync(
                        plant.Name,
                        model.Symptoms,
                        model.Environment,
                        model.AdditionalNotes);


                model.PlantName = plant.Name;


                // Convert AI Markdown output to HTML.
                // Raw HTML from the AI response is disabled
                // for safer rendering.
                var pipeline = new MarkdownPipelineBuilder()
                    .DisableHtml()
                    .Build();

                model.AnalysisResult =
                    Markdown.ToHtml(
                        rawResult,
                        pipeline);


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