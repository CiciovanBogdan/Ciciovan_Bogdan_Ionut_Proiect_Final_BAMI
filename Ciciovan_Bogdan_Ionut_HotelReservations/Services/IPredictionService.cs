using Ciciovan_Bogdan_Ionut_HotelReservations.Models;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Services
{
    public interface IPredictionService
    {
        Task<PredictionResponse> PredictCancellationAsync(PredictionRequest request);
    }
}