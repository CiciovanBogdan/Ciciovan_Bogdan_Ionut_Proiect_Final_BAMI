using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using BookingCalculationsGrpcService;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Controllers
{
    public class GrpcBookingController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly ILogger<GrpcBookingController> _logger;

        public GrpcBookingController(ILogger<GrpcBookingController> logger, IConfiguration configuration)
        {
            _logger = logger;

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