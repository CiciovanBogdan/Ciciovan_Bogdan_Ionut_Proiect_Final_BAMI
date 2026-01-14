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
    public class MealPlansController : Controller
    {
        private readonly HotelReservationsDbContext _context;

        public MealPlansController(HotelReservationsDbContext context)
        {
            _context = context;
        }

        // GET: MealPlans
        public async Task<IActionResult> Index(string searchString, decimal? minPrice, decimal? maxPrice, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentMinPrice"] = minPrice;
            ViewData["CurrentMaxPrice"] = maxPrice;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";

            var mealPlans = _context.MealPlans.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                mealPlans = mealPlans.Where(m =>
                    m.PlanName.ToLower().Contains(searchString)
                    || (m.Description != null && m.Description.ToLower().Contains(searchString)));
            }

            if (minPrice.HasValue)
            {
                mealPlans = mealPlans.Where(m => m.PricePerPerson >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                mealPlans = mealPlans.Where(m => m.PricePerPerson <= maxPrice.Value);
            }

            mealPlans = sortOrder switch
            {
                "name_desc" => mealPlans.OrderByDescending(m => m.PlanName),
                "price" => mealPlans.OrderBy(m => m.PricePerPerson),
                "price_desc" => mealPlans.OrderByDescending(m => m.PricePerPerson),
                _ => mealPlans.OrderBy(m => m.PlanName),
            };

            return View(await mealPlans.ToListAsync());
        }

        // GET: MealPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlans
                .FirstOrDefaultAsync(m => m.MealPlanId == id);
            if (mealPlan == null)
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // GET: MealPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MealPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MealPlanId,PlanName,Description,PricePerPerson,IsActive")] MealPlan mealPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mealPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mealPlan);
        }

        // GET: MealPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlans.FindAsync(id);
            if (mealPlan == null)
            {
                return NotFound();
            }
            return View(mealPlan);
        }

        // POST: MealPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MealPlanId,PlanName,Description,PricePerPerson,IsActive")] MealPlan mealPlan)
        {
            if (id != mealPlan.MealPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealPlanExists(mealPlan.MealPlanId))
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
            return View(mealPlan);
        }

        // GET: MealPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlans
                .FirstOrDefaultAsync(m => m.MealPlanId == id);
            if (mealPlan == null)
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // POST: MealPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealPlan = await _context.MealPlans.FindAsync(id);
            if (mealPlan != null)
            {
                _context.MealPlans.Remove(mealPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealPlanExists(int id)
        {
            return _context.MealPlans.Any(e => e.MealPlanId == id);
        }
    }
}
