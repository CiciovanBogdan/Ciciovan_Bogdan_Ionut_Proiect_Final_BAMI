using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;

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
                var totalNights = request.NoOfWeekendNights + request.NoOfWeekNights;
                var accommodationCost = totalNights * request.AvgPricePerRoom;

                var mealCostPerNight = request.MealPlan switch
                {
                    "Meal Plan 1" => 15.0f,
                    "Meal Plan 2" => 25.0f,
                    "Meal Plan 3" => 35.0f,
                    _ => 0.0f
                };
                var totalMealCost = mealCostPerNight * totalNights;

                var specialRequestsCost = request.NoOfSpecialRequests * 10.0f;
                var parkingCost = request.RequiredCarParking ? 50.0f : 0.0f;
                var totalExtrasCost = specialRequestsCost + parkingCost;

                var totalCost = accommodationCost + totalMealCost + totalExtrasCost;

                var breakdown = $"Accommodation: ${accommodationCost:F2} ({totalNights} nights × ${request.AvgPricePerRoom:F2})\n" +
                               $"Meals: ${totalMealCost:F2} ({request.MealPlan})\n" +
                               $"Extras: ${totalExtrasCost:F2} (Special requests: {request.NoOfSpecialRequests}, Parking: {request.RequiredCarParking})";

                _logger.LogInformation("Calculated total cost: ${Cost:F2}", totalCost);

                return Task.FromResult(new CostReply
                {
                    TotalCost = totalCost,
                    AccommodationCost = accommodationCost,
                    MealCost = totalMealCost,
                    ExtrasCost = totalExtrasCost,
                    Breakdown = breakdown
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total cost");
                throw new RpcException(new Status(StatusCode.Internal, "Error calculating cost"));
            }
        }

        public override async Task<StatsReply> GetBookingStats(StatsRequest request, ServerCallContext context)
        {
            try
            {
                var bookings = await _context.Bookings.ToListAsync();

                var totalBookings = bookings.Count;

                var canceledBookings = bookings.Count(b => b.BookingStatus == "Canceled");

                var notCanceledBookings = bookings.Count(b => b.BookingStatus != "Canceled");

                var cancellationRate = totalBookings > 0
                    ? (float)canceledBookings / totalBookings * 100
                    : 0f;

                var averagePrice = bookings.Any()
                    ? (float)bookings.Average(b => (double)b.AvgPricePerRoom)
                    : 0f;

                var totalGuests = bookings.Sum(b => b.NoOfAdults + b.NoOfChildren);

                _logger.LogInformation("Retrieved booking stats: Total={Total}, Active={Active}, Canceled={Canceled}, Rate={Rate:F2}%",
                    totalBookings, notCanceledBookings, canceledBookings, cancellationRate);

                return new StatsReply
                {
                    TotalBookings = totalBookings,
                    NotCanceledBookings = notCanceledBookings,
                    CanceledBookings = canceledBookings,
                    CancellationRate = cancellationRate,
                    AveragePrice = averagePrice,
                    TotalGuests = totalGuests
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking stats");
                throw new RpcException(new Status(StatusCode.Internal, "Error retrieving statistics"));
            }
        }

        public override async Task<BookingListReply> GetAllBookings(BookingListRequest request, ServerCallContext context)
        {
            try
            {
                var query = _context.Bookings
                    .Include(b => b.MealPlan)
                    .Include(b => b.RoomType)
                    .AsQueryable();

                if (!request.IncludeCanceled)
                {
                    query = query.Where(b => b.BookingStatus != "Canceled");
                }

                var maxResults = request.MaxResults > 0 ? request.MaxResults : 100;
                var bookings = await query.Take(maxResults).ToListAsync();

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
                        MealPlan = booking.MealPlan?.PlanName ?? "Not Selected",
                        AvgPricePerRoom = (float)booking.AvgPricePerRoom,
                        BookingStatus = booking.BookingStatus ?? "Unknown"
                    });
                }

                _logger.LogInformation("Retrieved {Count} bookings", bookings.Count);

                return reply;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings");
                throw new RpcException(new Status(StatusCode.Internal, "Error retrieving bookings"));
            }
        }
    }
}