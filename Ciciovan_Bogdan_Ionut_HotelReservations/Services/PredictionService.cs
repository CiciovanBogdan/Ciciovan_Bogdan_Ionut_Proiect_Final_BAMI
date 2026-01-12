using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PredictionService> _logger;

        public PredictionService(HttpClient httpClient, ILogger<PredictionService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PredictionResponse> PredictCancellationAsync(PredictionRequest request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    WriteIndented = true
                });

                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/predict", httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API returned error: {Status} - {Content}", response.StatusCode, errorContent);
                    throw new Exception($"API returned {response.StatusCode}: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiPredictionResponse>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                float probabilityCanceled = 0;
                float probabilityNotCanceled = 0;

                if (apiResponse?.Score != null && apiResponse.Score.Length >= 2)
                {
                    probabilityNotCanceled = apiResponse.Score[0];
                    probabilityCanceled = apiResponse.Score[1];
                }

                bool isCanceled = probabilityCanceled > 0.04f;
                float displayProbability = probabilityCanceled;

                string riskLevel;
                string recommendation;

                if (probabilityCanceled > 0.10f)
                {
                    riskLevel = "High Risk";
                    recommendation = "Strong chance of cancellation. Consider contacting the customer to confirm or request deposit.";
                }
                else if (probabilityCanceled > 0.05f)
                {
                    riskLevel = "Medium-High Risk";
                    recommendation = "Significant cancellation risk. Monitor this booking closely and consider confirmation call.";
                }
                else if (probabilityCanceled > 0.03f)
                {
                    riskLevel = "Medium Risk";
                    recommendation = "Moderate cancellation risk. Keep an eye on this booking.";
                }
                else if (probabilityCanceled > 0.015f)
                {
                    riskLevel = "Low-Medium Risk";
                    recommendation = "Some cancellation risk detected, but booking appears relatively stable.";
                }
                else if (probabilityCanceled > 0.005f)
                {
                    riskLevel = "Low Risk";
                    recommendation = "Low cancellation probability. Booking looks stable.";
                }
                else
                {
                    riskLevel = "Very Low Risk";
                    recommendation = "Excellent! This booking is very stable and unlikely to be canceled.";
                }

                return new PredictionResponse
                {
                    PredictedLabel = isCanceled ? "Canceled" : "Not Canceled",
                    Probability = displayProbability,
                    RiskLevel = riskLevel,
                    Recommendation = recommendation
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PredictCancellationAsync");
                return new PredictionResponse
                {
                    PredictedLabel = "Error",
                    Probability = 0,
                    RiskLevel = "Unknown",
                    Recommendation = $"Error calling prediction API: {ex.Message}. Please ensure the ML API is running."
                };
            }
        }

        private class ApiPredictionResponse
        {
            public string PredictedLabel { get; set; }
            public float[] Score { get; set; }
        }
    }
}