using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;
using Ciciovan_Bogdan_Ionut_HotelReservations.Models;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Controllers
{
    public class RoomTypesController : Controller
    {
        private readonly HotelReservationsDbContext _context;

        public RoomTypesController(HotelReservationsDbContext context)
        {
            _context = context;
        }

        // GET: RoomTypes
        public async Task<IActionResult> Index(string searchString, int? capacityFilter, decimal? minPrice, decimal? maxPrice, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCapacity"] = capacityFilter;
            ViewData["CurrentMinPrice"] = minPrice;
            ViewData["CurrentMaxPrice"] = maxPrice;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CapacitySortParm"] = sortOrder == "capacity" ? "capacity_desc" : "capacity";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";

            var roomTypes = _context.RoomTypes.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                roomTypes = roomTypes.Where(r =>
                    r.RoomTypeName.ToLower().Contains(searchString)
                    || (r.Description != null && r.Description.ToLower().Contains(searchString)));
            }

            if (capacityFilter.HasValue)
            {
                roomTypes = roomTypes.Where(r => r.MaxOccupancy == capacityFilter.Value);
            }

            if (minPrice.HasValue)
            {
                roomTypes = roomTypes.Where(r => r.BasePrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                roomTypes = roomTypes.Where(r => r.BasePrice <= maxPrice.Value);
            }

            roomTypes = sortOrder switch
            {
                "name_desc" => roomTypes.OrderByDescending(r => r.RoomTypeName),
                "capacity" => roomTypes.OrderBy(r => r.MaxOccupancy),
                "capacity_desc" => roomTypes.OrderByDescending(r => r.MaxOccupancy),
                "price" => roomTypes.OrderBy(r => r.BasePrice),
                "price_desc" => roomTypes.OrderByDescending(r => r.BasePrice),
                _ => roomTypes.OrderBy(r => r.RoomTypeName),
            };

            var capacities = await _context.RoomTypes
                .Select(r => r.MaxOccupancy)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
            ViewBag.CapacityList = capacities;

            return View(await roomTypes.ToListAsync());
        }

        // GET: RoomTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.RoomTypeId == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // GET: RoomTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomTypeId,RoomTypeName,Description,MaxOccupancy,BasePrice,IsActive")] RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        // GET: RoomTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }
            return View(roomType);
        }

        // POST: RoomTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomTypeId,RoomTypeName,Description,MaxOccupancy,BasePrice,IsActive")] RoomType roomType)
        {
            if (id != roomType.RoomTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomTypeExists(roomType.RoomTypeId))
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
            return View(roomType);
        }

        // GET: RoomTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.RoomTypeId == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // POST: RoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType != null)
            {
                _context.RoomTypes.Remove(roomType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomTypeExists(int id)
        {
            return _context.RoomTypes.Any(e => e.RoomTypeId == id);
        }
    }
}
