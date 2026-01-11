namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class PredictionResponse
    {
        public string PredictedLabel { get; set; }
        public float Probability { get; set; }
        public string RiskLevel { get; set; }
        public string Recommendation { get; set; }
    }
}