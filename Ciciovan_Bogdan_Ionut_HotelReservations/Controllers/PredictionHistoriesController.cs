using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Controllers
{
    public class PredictionHistoriesController : Controller
    {
        private readonly HotelReservationsDbContext _context;

        public PredictionHistoriesController(HotelReservationsDbContext context)
        {
            _context = context;
        }

        // GET: PredictionHistories
        public async Task<IActionResult> Index(string searchEmail, DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.PredictionHistories.AsQueryable();

            if (!string.IsNullOrEmpty(searchEmail))
            {
                query = query.Where(p => p.CustomerEmail.Contains(searchEmail));
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= toDate.Value.AddDays(1));
            }

            var predictions = await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewData["SearchEmail"] = searchEmail;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;

            return View(predictions);
        }

        // GET: PredictionHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predictionHistory = await _context.PredictionHistories
                .FirstOrDefaultAsync(m => m.Id == id);

            if (predictionHistory == null)
            {
                return NotFound();
            }

            return View(predictionHistory);
        }

        // GET: PredictionHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predictionHistory = await _context.PredictionHistories
                .FirstOrDefaultAsync(m => m.Id == id);

            if (predictionHistory == null)
            {
                return NotFound();
            }

            return View(predictionHistory);
        }

        // POST: PredictionHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var predictionHistory = await _context.PredictionHistories.FindAsync(id);
            if (predictionHistory != null)
            {
                _context.PredictionHistories.Remove(predictionHistory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}