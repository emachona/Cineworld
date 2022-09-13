using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;
using Cinema.ViewModels;

namespace Cinema.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly CinemaContext _context;

        public ScreeningsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Screenings
        public async Task<IActionResult> Index(string searchMovies, string searchTime, string searchDate, string searchCinema)
        {
            IQueryable<Screening> screenings = _context.Screening.Include(s => s.Movie).AsQueryable();
            IQueryable<string> moviesQuery = _context.Screening.OrderBy(m => m.Movie).Select(m => m.Movie.Title).Distinct();

            if (!string.IsNullOrEmpty(searchMovies))
            {
                screenings = screenings.Where(s => s.Movie.Title.ToLower().Contains(searchMovies.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchTime))
            {
                screenings = screenings.Where(s => s.Time.ToString().ToLower().Contains(searchTime.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchDate))
            {
                screenings = screenings.Where(s => s.Date.ToString().ToLower().Contains(searchDate.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchCinema))
            {
                screenings = screenings.Where(s => s.Cinema.ToLower().Contains(searchCinema.ToLower()));
            }

            //var todaysDate = DateTime.Today;
            //screenings = screenings.Where(s => s.Date >= todaysDate);
            var VM = new ScreeningFilterVM
            {
                moviesList = new SelectList(await moviesQuery.ToListAsync()),
                screenings = await screenings.ToListAsync()
            };

            return View(VM);
        }

        // GET: Screenings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Screening == null)
            {
                return NotFound();
            }

            var screening = await _context.Screening
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ScreeningId == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // GET: Screenings/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Title");
            return View();
        }

        // POST: Screenings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScreeningId,MovieId,Date,Time,Cinema,AvailablePlaces,Price,Type")] Screening screening)
        {
            if (ModelState.IsValid)
            {
                _context.Add(screening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Title", screening.MovieId);
            return View(screening);
        }

        // GET: Screenings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Screening == null)
            {
                return NotFound();
            }

            var screening = await _context.Screening.FindAsync(id);
            if (screening == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Title", screening.MovieId);
            return View(screening);
        }

        // POST: Screenings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScreeningId,MovieId,Time,Cinema,AvailablePlaces,Price,Type")] Screening screening)
        {
            if (id != screening.ScreeningId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreeningExists(screening.ScreeningId))
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
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Genre", screening.MovieId);
            return View(screening);
        }

        // GET: Screenings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Screening == null)
            {
                return NotFound();
            }

            var screening = await _context.Screening
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ScreeningId == id);
            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        // POST: Screenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Screening == null)
            {
                return Problem("Entity set 'CinemaContext.Screening'  is null.");
            }
            var screening = await _context.Screening.FindAsync(id);
            if (screening != null)
            {
                _context.Screening.Remove(screening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreeningExists(int id)
        {
            return (_context.Screening?.Any(e => e.ScreeningId == id)).GetValueOrDefault();
        }
    }
}
