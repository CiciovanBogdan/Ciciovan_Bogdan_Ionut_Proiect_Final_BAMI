using Ciciovan_Bogdan_Ionut_HotelReservations.Controllers;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;
using Ciciovan_Bogdan_Ionut_HotelReservations.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index()
        {
            ViewData["RoomTypes"] = new SelectList(new[] {
                "Room_Type 1", "Room_Type 2", "Room_Type 3", "Room_Type 4",
                "Room_Type 5", "Room_Type 6", "Room_Type 7"
            });

            ViewData["MealPlans"] = new SelectList(new[] {
                "Not Selected", "Meal Plan 1", "Meal Plan 2", "Meal Plan 3"
            });

            ViewData["MarketSegments"] = new SelectList(new[] {
                "Online", "Offline", "Corporate", "Aviation", "Complementary"
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PredictionRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _predictionService.PredictCancellationAsync(request);
                ViewBag.Result = result;
            }

            ViewData["RoomTypes"] = new SelectList(new[] {
                "Room_Type 1", "Room_Type 2", "Room_Type 3", "Room_Type 4",
                "Room_Type 5", "Room_Type 6", "Room_Type 7"
            });

            ViewData["MealPlans"] = new SelectList(new[] {
                "Not Selected", "Meal Plan 1", "Meal Plan 2", "Meal Plan 3"
            });

            ViewData["MarketSegments"] = new SelectList(new[] {
                "Online", "Offline", "Corporate", "Aviation", "Complementary"
            });

            return View(request);
        }
    }
}