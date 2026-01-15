using BookingCalculationsGrpcService;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;
using Microsoft.EntityFrameworkCore;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Controllers
{
    public class GrpcBookingController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly ILogger<GrpcBookingController> _logger;
        private readonly HotelReservationsDbContext _context;

        public GrpcBookingController(
            ILogger<GrpcBookingController> logger,
            IConfiguration configuration,
            HotelReservationsDbContext context)
        {
            _logger = logger;
            _context = context;

            var grpcUrl = configuration.GetValue<string>("GrpcServices:BookingCalculations")
                          ?? "https://localhost:7020";

            _channel = GrpcChannel.ForAddress(grpcUrl);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CalculateCost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalculateCost(CostRequest request)
        {
            try
            {
                var client = new BookingCalculations.BookingCalculationsClient(_channel);
                var reply = await client.CalculateTotalCostAsync(request);

                ViewBag.Result = reply;
                return View(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling gRPC CalculateTotalCost");
                ViewBag.Error = $"Error: {ex.Message}";
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var client = new BookingCalculations.BookingCalculationsClient(_channel);
                var reply = await client.GetBookingStatsAsync(new StatsRequest());

                return View(reply);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling gRPC GetBookingStats");
                ViewBag.Error = $"Error: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> BookingsList(bool includeCanceled = false, int maxResults = 50)
        {
            try
            {
                var client = new BookingCalculations.BookingCalculationsClient(_channel);
                var request = new BookingListRequest
                {
                    IncludeCanceled = includeCanceled,
                    MaxResults = maxResults
                };

                var reply = await client.GetAllBookingsAsync(request);

                ViewBag.IncludeCanceled = includeCanceled;
                ViewBag.MaxResults = maxResults;

                return View(reply);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling gRPC GetAllBookings");
                ViewBag.Error = $"Error: {ex.Message}";
                return View(new BookingListReply());
            }
        }

        [HttpGet]
        public async Task<IActionResult> PredictViaGrpc()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PredictViaGrpc(
            string customerEmail,
            int noOfAdults,
            int noOfChildren,
            int noOfWeekendNights,
            int noOfWeekNights,
            int typeOfMealPlan,
            int requiredCarParkingSpace,
            int roomTypeReserved,
            int leadTime,
            int arrivalMonth,
            float avgPricePerRoom,
            int noOfSpecialRequests,
            string marketSegmentType,
            int isRepeatedGuest,
            int noPreviousCancellations,
            int noPreviousBookingsNotCanceled)
        {
            try
            {
                var client = new BookingCalculations.BookingCalculationsClient(_channel);

                var request = new PredictionRequest
                {
                    CustomerEmail = customerEmail ?? "",
                    NoOfAdults = noOfAdults,
                    NoOfChildren = noOfChildren,
                    NoOfWeekendNights = noOfWeekendNights,
                    NoOfWeekNights = noOfWeekNights,
                    TypeOfMealPlan = typeOfMealPlan,
                    RequiredCarParkingSpace = requiredCarParkingSpace,
                    RoomTypeReserved = roomTypeReserved,
                    LeadTime = leadTime,
                    ArrivalMonth = arrivalMonth,
                    AvgPricePerRoom = avgPricePerRoom,
                    NoOfSpecialRequests = noOfSpecialRequests,
                    MarketSegmentType = marketSegmentType,
                    IsRepeatedGuest = isRepeatedGuest,
                    NoPreviousCancellations = noPreviousCancellations,
                    NoPreviousBookingsNotCanceled = noPreviousBookingsNotCanceled
                };

                var reply = await client.PredictCancellationAsync(request);

                ViewBag.Result = reply;
                await PopulateDropdowns();

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error: {ex.Message}";
                await PopulateDropdowns();

                return View();
            }
        }

        private async Task PopulateDropdowns()
        {
            var customers = await _context.Customers
                .Select(c => new {
                    Email = c.Email,
                    DisplayText = c.FirstName + " " + c.LastName + " (" + c.Email + ")"
                })
                .ToListAsync();

            ViewBag.Customers = new SelectList(customers, "Email", "DisplayText");

            var roomTypes = await _context.RoomTypes
                .Where(r => r.IsActive)
                .Select(r => new {
                    Value = r.RoomTypeId,
                    Text = r.RoomTypeName + " - " + r.Description
                })
                .ToListAsync();

            ViewBag.RoomTypes = new SelectList(roomTypes, "Value", "Text");

            var mealPlans = await _context.MealPlans
                .Where(m => m.IsActive)
                .Select(m => new {
                    Value = m.MealPlanId - 1,
                    Text = m.PlanName + " - " + m.Description
                })
                .ToListAsync();

            ViewBag.MealPlans = new SelectList(mealPlans, "Value", "Text");

            ViewBag.MarketSegments = new SelectList(new[] {
                "Online", "Offline", "Corporate", "Aviation", "Complementary"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _channel?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}