namespace TravelRecommender.Api.Models
{
    public class UserPrompt
    {
        public string Origin { get; set; } = "";
        public int DurationDays { get; set; }
        public decimal Budget { get; set; }
        public string FreeComment { get; set; } = "";
    }
}
