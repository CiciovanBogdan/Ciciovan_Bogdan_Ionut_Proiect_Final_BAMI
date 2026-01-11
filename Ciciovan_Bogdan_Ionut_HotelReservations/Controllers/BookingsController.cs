using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Controllers
{
    public class BookingsController : Controller
    {
        private readonly HotelReservationsDbContext _context;

        public BookingsController(HotelReservationsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string statusFilter, string sortOrder, int? roomTypeFilter)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentStatus"] = statusFilter;
            ViewData["CurrentRoomType"] = roomTypeFilter;
            ViewData["BookingCodeSortParm"] = sortOrder == "code" ? "code_desc" : "code";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["CustomerSortParm"] = sortOrder == "customer" ? "customer_desc" : "customer";
            ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";

            var bookings = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.RoomType)
                .Include(b => b.MealPlan)
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b => b.BookingCode.Contains(searchString)
                                            || b.Customer.FirstName.Contains(searchString)
                                            || b.Customer.LastName.Contains(searchString)
                                            || b.Customer.Email.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(statusFilter))
            {
                bookings = bookings.Where(b => b.BookingStatus == statusFilter);
            }

            if (roomTypeFilter.HasValue)
            {
                bookings = bookings.Where(b => b.RoomTypeId == roomTypeFilter.Value);
            }

            bookings = sortOrder switch
            {
                "code_desc" => bookings.OrderByDescending(b => b.BookingCode),
                "code" => bookings.OrderBy(b => b.BookingCode),
                "date" => bookings.OrderBy(b => b.ArrivalDate),
                "date_desc" => bookings.OrderByDescending(b => b.ArrivalDate),
                "price" => bookings.OrderBy(b => b.AvgPricePerRoom),
                "price_desc" => bookings.OrderByDescending(b => b.AvgPricePerRoom),
                "customer" => bookings.OrderBy(b => b.Customer.LastName),
                "customer_desc" => bookings.OrderByDescending(b => b.Customer.LastName),
                "status" => bookings.OrderBy(b => b.BookingStatus),
                "status_desc" => bookings.OrderByDescending(b => b.BookingStatus),
                _ => bookings.OrderByDescending(b => b.CreatedDate),
            };

            ViewBag.StatusList = new SelectList(new[] { "Confirmed", "Canceled", "Pending", "Completed" });
            ViewBag.RoomTypeList = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");

            return View(await bookings.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.MealPlan)
                .Include(b => b.RoomType)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            ViewData["MealPlanId"] = new SelectList(_context.MealPlans, "MealPlanId", "PlanName");
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
            ViewData["MarketSegmentTypes"] = new SelectList(new[] { "Online", "Offline", "Corporate", "Aviation", "Complementary" });
            ViewData["BookingStatuses"] = new SelectList(new[] { "Confirmed", "Pending", "Canceled", "Completed" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,RoomTypeId,MealPlanId,ArrivalDate,NoOfWeekendNights,NoOfWeekNights,NoOfAdults,NoOfChildren,RequiredCarParking,NoOfSpecialRequests,LeadTime,MarketSegmentType,BookingStatus")] Booking booking)
        {
            ModelState.Remove("BookingCode");
            ModelState.Remove("AvgPricePerRoom");

            if (ModelState.IsValid)
            {
                var roomType = await _context.RoomTypes.FindAsync(booking.RoomTypeId);

                booking.BookingCode = GenerateBookingCode();
                booking.AvgPricePerRoom = roomType.BasePrice;
                booking.CreatedDate = DateTime.Now;
                booking.LastModifiedDate = DateTime.Now;

                _context.Add(booking);
                await _context.SaveChangesAsync();

                var history = new BookingHistory
                {
                    BookingId = booking.BookingId,
                    ChangeType = "Created",
                    ChangeDescription = "Booking created",
                    NewStatus = booking.BookingStatus,
                    ChangedDate = DateTime.Now,
                    ChangedByUser = "System"
                };
                _context.BookingHistories.Add(history);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", booking.CustomerId);
            ViewData["MealPlanId"] = new SelectList(_context.MealPlans, "MealPlanId", "PlanName", booking.MealPlanId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", booking.RoomTypeId);
            ViewData["MarketSegmentTypes"] = new SelectList(new[] { "Online", "Offline", "Corporate", "Aviation", "Complementary" }, booking.MarketSegmentType);
            ViewData["BookingStatuses"] = new SelectList(new[] { "Confirmed", "Pending", "Canceled", "Completed" }, booking.BookingStatus);
            return View(booking);
        }

        private string GenerateBookingCode()
        {
            var random = new Random();
            var year = DateTime.Now.Year;
            var randomNumber = random.Next(10000, 99999);
            return $"BK{year}{randomNumber}";
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", booking.CustomerId);
            ViewData["MealPlanId"] = new SelectList(_context.MealPlans, "MealPlanId", "PlanName", booking.MealPlanId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", booking.RoomTypeId);
            ViewData["MarketSegmentTypes"] = new SelectList(new[] { "Online", "Offline", "Corporate", "Aviation", "Complementary" }, booking.MarketSegmentType);
            ViewData["BookingStatuses"] = new SelectList(new[] { "Confirmed", "Pending", "Canceled", "Completed" }, booking.BookingStatus);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,BookingCode,CustomerId,RoomTypeId,MealPlanId,ArrivalDate,NoOfWeekendNights,NoOfWeekNights,NoOfAdults,NoOfChildren,RequiredCarParking,NoOfSpecialRequests,AvgPricePerRoom,LeadTime,MarketSegmentType,BookingStatus,CreatedDate")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldBooking = await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == id);
                    booking.LastModifiedDate = DateTime.Now;
                    _context.Update(booking);
                    await _context.SaveChangesAsync();

                    if (oldBooking.BookingStatus != booking.BookingStatus)
                    {
                        var history = new BookingHistory
                        {
                            BookingId = booking.BookingId,
                            ChangeType = "Modified",
                            ChangeDescription = $"Status changed from {oldBooking.BookingStatus} to {booking.BookingStatus}",
                            OldStatus = oldBooking.BookingStatus,
                            NewStatus = booking.BookingStatus,
                            ChangedDate = DateTime.Now,
                            ChangedByUser = "System"
                        };
                        _context.BookingHistories.Add(history);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", booking.CustomerId);
            ViewData["MealPlanId"] = new SelectList(_context.MealPlans, "MealPlanId", "PlanName", booking.MealPlanId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", booking.RoomTypeId);
            ViewData["MarketSegmentTypes"] = new SelectList(new[] { "Online", "Offline", "Corporate", "Aviation", "Complementary" }, booking.MarketSegmentType);
            ViewData["BookingStatuses"] = new SelectList(new[] { "Confirmed", "Pending", "Canceled", "Completed" }, booking.BookingStatus);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.MealPlan)
                .Include(b => b.RoomType)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}