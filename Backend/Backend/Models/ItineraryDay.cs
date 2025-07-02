namespace TravelRecommender.Api.Models
{
    public class ItineraryDay
    {
        public int Day { get; set; }
        public string MainActivity { get; set; } = "";
        public string Location { get; set; } = "";
        public string SuggestedFood { get; set; } = "";
    }
}
