using System.Collections.Generic;

namespace TravelRecommender.Api.Models
{
    public class Destination
    {
        public string Name { get; set; }
        public int RecommendedDuration { get; set; }
        public decimal MinBudget { get; set; }
        public decimal MaxBudget { get; set; }
        public List<string> Interests { get; set; }
        public string Description { get; set; }
        public string LocalCurrency { get; set; }
        public string Language { get; set; }
        public string IdealSeason { get; set; }
        public string SafetyLevel { get; set; }
        public string Region { get; set; }
        public List<string> SuggestedHotels { get; set; }
        public List<string> SuggestedFlights { get; set; }
    }
}
