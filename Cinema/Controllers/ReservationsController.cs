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
    public class ReservationsController : Controller
    {
        private readonly CinemaContext _context;

        public ReservationsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index(string searchMovie)
        {
            IQueryable<Reservation> reservations = _context.Reservation.Include(r => r.User).Include(r => r.Screening).ThenInclude(r => r.Movie).AsQueryable();
            IQueryable<string> moviesQuery = _context.Reservation.OrderBy(m => m.Screening).Select(m => m.Screening.Movie.Title).Distinct();

            if (!string.IsNullOrEmpty(searchMovie))
            {
                reservations = reservations.Where(s => s.Screening.Movie.Title.ToLower().Contains(searchMovie.ToLower()));
            }

            var VM = new ReservationFilterVM
            {
                movieTitleList = new SelectList(await moviesQuery.ToListAsync()),
                reservations = await reservations.ToListAsync()
            };

            return View(VM);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Screening)
                .ThenInclude(r => r.Movie)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> CreateReservationAsync(int? id)
        {
            var screening = await _context.Screening
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ScreeningId == id);
            ViewData["ScreeningId"] = id;
            ViewData["MovieTitle"] = screening.Movie.Title;
            ViewData["CinemaHall"] = screening.Cinema;
            ViewData["Time"] = screening.Date;
            int currentUserId = 1;                 //string currentUserId = User.Identity.GetUserId();
            var currUser = _context.User.Where(m => m.UserId == currentUserId).First();
            ViewData["UserId"] = currentUserId;
            ViewData["User"] =currUser.FullName;

            return View();
        }

        //POST: Reservations/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation([Bind("ReservationId,ScreeningId,UserId,Seat")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScreeningId"] = new SelectList(_context.Screening, "ScreeningId", "Cinema", reservation.ScreeningId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "FirstName", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ScreeningId"] = new SelectList(_context.Screening, "ScreeningId", "Cinema", reservation.ScreeningId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "FirstName", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,ScreeningId,UserId,Seat")] Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
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
            ViewData["ScreeningId"] = new SelectList(_context.Screening, "ScreeningId", "Cinema", reservation.ScreeningId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "FirstName", reservation.UserId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Screening)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'CinemaContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return (_context.Reservation?.Any(e => e.ReservationId == id)).GetValueOrDefault();
        }
    }
}
