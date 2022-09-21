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
using System.Security.Claims;
using Cinema.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly CinemaContext _context;
        private readonly UserManager<CinemaUser> _userManager;

        public ReservationsController(CinemaContext context, UserManager<CinemaUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        [Authorize(Roles = "Admin")]
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

        [Authorize (Roles="Client")]
        public async Task<IActionResult> ClientReservations()
        {
            var userIdentity = _userManager.GetUserAsync(User).Result.user_ID;
            var client = await _context.Client.Where(x => x.ClientId == userIdentity).FirstOrDefaultAsync();

            ViewBag.client = client.FullName;

            IQueryable<Reservation> reservations = _context.Reservation.Where(x => x.ClientId == client.ClientId)
            .Include(e => e.User)
            .Include(e => e.Screening).ThenInclude(e => e.Movie);
            await _context.SaveChangesAsync();

            if (reservations == null)
            {
                return NotFound();
            }

            return View(await reservations.ToListAsync());
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
            var userIdentity = _userManager.GetUserAsync(User).Result.user_ID;
            var client = await _context.Client.Where(x => x.ClientId == userIdentity).FirstOrDefaultAsync();
            ViewData["ClientId"] = client.ClientId;
            ViewData["User"] =client.FullName;

            return View();
        }

        //POST: Reservations/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation([Bind("ReservationId,ScreeningId,ClientId,Seat")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScreeningId"] = new SelectList(_context.Screening, "ScreeningId", "Cinema", reservation.ScreeningId);
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "FirstName", reservation.ClientId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize (Roles = "Client")]
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "FirstName", reservation.ClientId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,ScreeningId,ClientId,Seat")] Reservation reservation)
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "FirstName", reservation.ClientId);
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
