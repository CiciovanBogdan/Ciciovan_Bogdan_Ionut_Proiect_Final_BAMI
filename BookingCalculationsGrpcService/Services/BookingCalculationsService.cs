using Ciciovan_Bogdan_Ionut_HotelReservations.Data;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;
using Grpc.Core;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace BookingCalculationsGrpcService.Services
{
    public class BookingCalculationsService : BookingCalculations.BookingCalculationsBase
    {
        private readonly ILogger<BookingCalculationsService> _logger;
        private readonly HotelReservationsDbContext _context;

        public BookingCalculationsService(
            ILogger<BookingCalculationsService> logger,
            HotelReservationsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override Task<CostReply> CalculateTotalCost(CostRequest request, ServerCallContext context)
        {
            try
            {
                decimal accommodationCost = (request.NoOfWeekendNights + request.NoOfWeekNights) * (decimal)request.AvgPricePerRoom;

                decimal mealCost = 0;
                int totalNights = request.NoOfWeekendNights + request.NoOfWeekNights;

                switch (request.MealPlan?.ToLower())
                {
                    case "meal plan 1":
                        mealCost = 15 * totalNights;
                        break;
                    case "meal plan 2":
                        mealCost = 35 * totalNights;
                        break;
                    case "meal plan 3":
                        mealCost = 50 * totalNights;
                        break;
                    default:
                        mealCost = 0;
                        break;
                }

                decimal extrasCost = 0;
                if (request.RequiredCarParking)
                {
                    extrasCost += 10 * totalNights;
                }
                extrasCost += request.NoOfSpecialRequests * 25;

                decimal totalCost = accommodationCost + mealCost + extrasCost;

                var breakdown = $"Accommodation: ${accommodationCost:F2} ({totalNights} nights × ${request.AvgPricePerRoom:F2})\n" +
                               $"Meals: ${mealCost:F2} ({request.MealPlan ?? "Not selected"})\n" +
                               $"Extras: ${extrasCost:F2} (Parking: {request.RequiredCarParking}, Special requests: {request.NoOfSpecialRequests})";

                var reply = new CostReply
                {
                    TotalCost = (float)totalCost,
                    AccommodationCost = (float)accommodationCost,
                    MealCost = (float)mealCost,
                    ExtrasCost = (float)extrasCost,
                    Breakdown = breakdown
                };

                return Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CalculateTotalCost");
                throw new RpcException(new Status(StatusCode.Internal, "Calculation failed"));
            }
        }

        public override async Task<StatsReply> GetBookingStats(StatsRequest request, ServerCallContext context)
        {
            try
            {
                var allBookings = await _context.Bookings.ToListAsync();

                int totalBookings = allBookings.Count;
                int notCanceledBookings = allBookings.Count(b => b.BookingStatus != "Canceled");
                int canceledBookings = allBookings.Count(b => b.BookingStatus == "Canceled");

                float cancellationRate = totalBookings > 0
                    ? (float)canceledBookings / totalBookings * 100
                    : 0;

                decimal avgPrice = allBookings.Any()
                    ? allBookings.Average(b => b.AvgPricePerRoom)
                    : 0;

                int totalGuests = allBookings.Sum(b => b.NoOfAdults + b.NoOfChildren);

                var reply = new StatsReply
                {
                    TotalBookings = totalBookings,
                    NotCanceledBookings = notCanceledBookings,
                    CanceledBookings = canceledBookings,
                    CancellationRate = cancellationRate,
                    AveragePrice = (float)avgPrice,
                    TotalGuests = totalGuests
                };

                return reply;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBookingStats");
                throw new RpcException(new Status(StatusCode.Internal, "Stats retrieval failed"));
            }
        }

        public override async Task<BookingListReply> GetAllBookings(BookingListRequest request, ServerCallContext context)
        {
            try
            {
                var query = _context.Bookings
                    .Include(b => b.MealPlan)
                    .AsQueryable();

                if (!request.IncludeCanceled)
                {
                    query = query.Where(b => b.BookingStatus != "Canceled");
                }

                if (request.MaxResults > 0)
                {
                    query = query.Take(request.MaxResults);
                }

                var bookings = await query.ToListAsync();

                var reply = new BookingListReply
                {
                    TotalCount = bookings.Count
                };

                foreach (var booking in bookings)
                {
                    reply.Bookings.Add(new BookingSummary
                    {
                        BookingId = booking.BookingId,
                        NoOfAdults = booking.NoOfAdults,
                        NoOfChildren = booking.NoOfChildren,
                        NoOfWeekendNights = booking.NoOfWeekendNights,
                        NoOfWeekNights = booking.NoOfWeekNights,
                        MealPlan = booking.MealPlan?.PlanName ?? "Not selected",
                        AvgPricePerRoom = (float)booking.AvgPricePerRoom,
                        BookingStatus = booking.BookingStatus
                    });
                }

                return reply;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBookings");
                throw new RpcException(new Status(StatusCode.Internal, "Bookings retrieval failed"));
            }
        }

        public override async Task<PredictionReply> PredictCancellation(
            PredictionRequest request,
            ServerCallContext context)
        {
            try
            {
                var mlApiUrl = "https://localhost:50490/predict";

                var mlRequest = new
                {
                    no_of_adults = (float)request.NoOfAdults,
                    no_of_children = (float)request.NoOfChildren,
                    no_of_weekend_nights = (float)request.NoOfWeekendNights,
                    no_of_week_nights = (float)request.NoOfWeekNights,
                    type_of_meal_plan = request.TypeOfMealPlan.ToString(),
                    required_car_parking_space = (float)request.RequiredCarParkingSpace,
                    room_type_reserved = "Room_Type " + request.RoomTypeReserved,
                    lead_time = (float)request.LeadTime,
                    arrival_month = (float)request.ArrivalMonth,
                    market_segment_type = request.MarketSegmentType,
                    repeated_guest = (float)request.IsRepeatedGuest,
                    no_of_previous_cancellations = (float)request.NoPreviousCancellations,
                    no_of_previous_bookings_not_canceled = (float)request.NoPreviousBookingsNotCanceled,
                    avg_price_per_room = request.AvgPricePerRoom,
                    no_of_special_requests = (float)request.NoOfSpecialRequests,
                    booking_status = "Not_Canceled"
                };

                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var httpClient = new HttpClient(handler);

                var json = JsonSerializer.Serialize(mlRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(mlApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("ML API returned {StatusCode}: {Error}", response.StatusCode, errorContent);
                    throw new RpcException(new Status(StatusCode.Internal,
                        $"ML API call failed: {response.StatusCode}"));
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                var mlResponse = JsonSerializer.Deserialize<MlApiResponse>(
                    responseJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                float probability = 0f;

                if (mlResponse.Score != null && mlResponse.Score.Length > 1)
                {
                    probability = mlResponse.Score[1];
                }

                string riskLevel;
                string baseRecommendation;

                if (probability > 0.1f)
                {
                    riskLevel = "High Risk";
                    baseRecommendation = "Consider requesting deposit or implementing stricter cancellation policy.";
                }
                else if (probability > 0.03f)
                {
                    riskLevel = "Medium Risk";
                    baseRecommendation = "Monitor booking closely and send confirmation reminders.";
                }
                else
                {
                    riskLevel = "Low Risk";
                    baseRecommendation = "Standard processing - booking appears stable.";
                }

                string customerInsight = "New customer - no historical data available.";
                string enhancedRecommendation = baseRecommendation;

                if (!string.IsNullOrEmpty(request.CustomerEmail))
                {
                    var customer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.Email == request.CustomerEmail);

                    if (customer != null)
                    {
                        customerInsight = $"Customer History: {customer.TotalPreviousBookings} completed bookings, " +
                                         $"{customer.TotalPreviousCancellations} cancellations. " +
                                         $"Repeated guest: {(customer.IsRepeatedGuest ? "Yes" : "No")}";

                        if (customer.TotalPreviousCancellations >= 3)
                        {
                            riskLevel = "High Risk";
                            enhancedRecommendation = baseRecommendation +
                                " ⚠️ WARNING: Customer has high cancellation history (" +
                                customer.TotalPreviousCancellations + " times). REQUIRE DEPOSIT.";
                        }
                        else if (customer.TotalPreviousCancellations >= 1 && probability > 0.05f)
                        {
                            enhancedRecommendation = baseRecommendation +
                                " Note: Customer has " + customer.TotalPreviousCancellations +
                                " previous cancellation(s). Consider stricter policy.";
                        }
                        else if (customer.IsRepeatedGuest && customer.TotalPreviousCancellations == 0)
                        {
                            enhancedRecommendation = baseRecommendation +
                                " ✓ Good customer: " + customer.TotalPreviousBookings +
                                " completed bookings with no cancellations.";
                        }
                    }
                }

                var predictionHistory = new PredictionHistory
                {
                    PredictedLabel = mlResponse.PredictedLabel ?? "Unknown",
                    Probability = probability,
                    RiskLevel = riskLevel,
                    Recommendation = enhancedRecommendation,
                    RequestDataJson = json,
                    CustomerEmail = request.CustomerEmail ?? "Unknown",
                    CreatedAt = DateTime.Now
                };

                _context.PredictionHistories.Add(predictionHistory);
                await _context.SaveChangesAsync();

                return new PredictionReply
                {
                    PredictedLabel = mlResponse.PredictedLabel ?? "Unknown",
                    Probability = probability,
                    RiskLevel = riskLevel,
                    Recommendation = enhancedRecommendation,
                    HistoryRecordId = predictionHistory.Id,
                    CustomerInsight = customerInsight
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PredictCancellation");
                throw new RpcException(new Status(StatusCode.Internal,
                    $"Prediction failed: {ex.Message}"));
            }
        }

        private class MlApiResponse
        {
            public string PredictedLabel { get; set; }
            public float[] Score { get; set; }
        }
    }
}