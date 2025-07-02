using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TravelRecommender.Api.Models;
using TravelRecommender.Api.Services;

namespace TravelRecommender.Api.Controllers
{
    [ApiController]
    //TODO: add a file for resource routes
    [Route("api/[controller]")]
    //TODO: add authoritzation policies
    public class RecommendationsController : ControllerBase
    {
        private readonly string _jsonPath = "MockData/Destinations.json";
        private readonly LLMService _llmService;

        public RecommendationsController(LLMService llmService)
        {
            _llmService = llmService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserPrompt prompt)
        {
            if (!System.IO.File.Exists(_jsonPath))
                return NotFound("Destinations file not found.");

            var json = System.IO.File.ReadAllText(_jsonPath);
            var destinations = JsonSerializer.Deserialize<List<Destination>>(json);

            if (destinations == null || !destinations.Any())
                return BadRequest("No destinations could be loaded.");

            var recommendations = destinations
                .Where(d =>
                    d.RecommendedDuration >= prompt.DurationDays - 2 &&
                    d.RecommendedDuration <= prompt.DurationDays + 2 &&
                    d.MinBudget <= prompt.Budget &&
                    d.MaxBudget >= prompt.Budget
                )
                .Take(3)
                .Select(d => new TripRecommendation
                {
                    Destination = d.Name,
                    Description = d.Description,
                    EstimatedBudget = (d.MinBudget + d.MaxBudget) / 2,
                    SuggestedHotels = d.SuggestedHotels,
                    SuggestedFlights = d.SuggestedFlights
                })
                .ToList();

            return Ok(recommendations);
        }

        [HttpPost("ai")]
        public async Task<IActionResult> PostWithLLMAsync([FromBody] UserPrompt prompt)
        {
            var userPrompt = $"I recently traveled to {prompt.PreviousDestination} for {prompt.DurationDays} days with a budget of {prompt.Budget} euros from {prompt.Origin}. I really enjoyed the trip. Can you suggest 3 similar travel destinations, with a similar duration and budget? For each one, include: - Destination name - A short description - Estimated budget - Sample hotels - Sample flights. Please format your response as JSON.";

            var response = await _llmService.GenerateResponseAsync(userPrompt);

            // Optional: try to parse response as JSON if you're confident in the LLM's formatting
            return Ok(response);
        }

    }
}
