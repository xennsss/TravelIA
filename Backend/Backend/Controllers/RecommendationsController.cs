using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TravelRecommender.Api.Models;

namespace TravelRecommender.Api.Controllers
{
    [ApiController]
    //TODO: add a file for resource routes
    [Route("api/[controller]")]
    //TODO: add authoritzation policies
    public class RecommendationsController : ControllerBase
    {
        private readonly string _jsonPath = "MockData/Destinations.json";

        [HttpPost]
        public IActionResult Post([FromBody] UserPrompt prompt)
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
    }
}
