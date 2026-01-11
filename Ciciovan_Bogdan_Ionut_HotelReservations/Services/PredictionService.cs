using System.Text;
using System.Text.Json;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly HttpClient _httpClient;

        public PredictionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PredictionResponse> PredictCancellationAsync(PredictionRequest request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/predict", httpContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiPredictionResponse>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var isCanceled = apiResponse?.PredictedLabel?.ToLower() == "canceled";
                var probability = apiResponse?.Score ?? 0;

                string riskLevel;
                string recommendation;

                if (probability > 0.7f)
                {
                    riskLevel = "High Risk";
                    recommendation = "Strong chance of cancellation. Consider contacting the customer to confirm.";
                }
                else if (probability > 0.4f)
                {
                    riskLevel = "Medium Risk";
                    recommendation = "Moderate cancellation risk. Monitor this booking.";
                }
                else
                {
                    riskLevel = "Low Risk";
                    recommendation = "Low cancellation probability. Booking looks stable.";
                }

                return new PredictionResponse
                {
                    PredictedLabel = isCanceled ? "Canceled" : "Not Canceled",
                    Probability = probability,
                    RiskLevel = riskLevel,
                    Recommendation = recommendation
                };
            }
            catch (Exception ex)
            {
                return new PredictionResponse
                {
                    PredictedLabel = "Error",
                    Probability = 0,
                    RiskLevel = "Unknown",
                    Recommendation = $"Error calling prediction API: {ex.Message}"
                };
            }
        }

        private class ApiPredictionResponse
        {
            public string PredictedLabel { get; set; }
            public float Score { get; set; }
        }
    }
}