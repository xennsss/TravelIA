using System.Collections.Generic;

namespace TravelRecommender.Api.Models
{
    public class TripRecommendation
    {
        public string Destination { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal EstimatedBudget { get; set; }
        public List<string> SuggestedHotels { get; set; } = new();
        public List<string> SuggestedFlights { get; set; } = new();
    }
}
