using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;
using Ciciovan_Bogdan_Ionut_HotelReservations.Services;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly IPredictionService _predictionService;
        private readonly HotelReservationsDbContext _context;

        public PredictionsController(IPredictionService predictionService, HotelReservationsDbContext context)
        {
            _predictionService = predictionService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var defaultRequest = new PredictionRequest
            {
                NoOfAdults = 2,
                NoOfChildren = 0,
                IsRepeatedGuest = false,
                NoPreviousCancellations = 0,
                NoPreviousBookings = 0,
                NoOfWeekendNights = 2,
                NoOfWeekNights = 3,
                LeadTime = 30,
                AvgPricePerRoom = 100.00m,
                NoOfSpecialRequests = 0,
                RequiredCarParking = false,
                ArrivalMonth = DateTime.Now.Month,
                RoomTypeReserved = "Room_Type 1",
                TypeOfMealPlan = "Not Selected",
                MarketSegmentType = "Online"
            };

            ViewBag.RoomTypes = new SelectList(new[] {
                "Room_Type 1", "Room_Type 2", "Room_Type 3", "Room_Type 4",
                "Room_Type 5", "Room_Type 6", "Room_Type 7"
            });

            ViewBag.MealPlans = new SelectList(new[] {
                "Not Selected", "Meal Plan 1", "Meal Plan 2", "Meal Plan 3"
            });

            ViewBag.MarketSegments = new SelectList(new[] {
                "Online", "Offline", "Corporate", "Aviation", "Complementary"
            });

            return View(defaultRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PredictionRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _predictionService.PredictCancellationAsync(request);
                ViewBag.Result = result;
            }

            ViewBag.RoomTypes = new SelectList(new[] {
                "Room_Type 1", "Room_Type 2", "Room_Type 3", "Room_Type 4",
                "Room_Type 5", "Room_Type 6", "Room_Type 7"
            }, request.RoomTypeReserved);

            ViewBag.MealPlans = new SelectList(new[] {
                "Not Selected", "Meal Plan 1", "Meal Plan 2", "Meal Plan 3"
            }, request.TypeOfMealPlan);

            ViewBag.MarketSegments = new SelectList(new[] {
                "Online", "Offline", "Corporate", "Aviation", "Complementary"
            }, request.MarketSegmentType);

            return View(request);
        }
    }
}