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
    public class MoviesController : Controller
    {
        private readonly CinemaContext _context;

        public MoviesController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string searchTitle, string searchGenres)
        {
            IQueryable<Movie> movies = _context.Movie.AsQueryable();
            IQueryable<string> genreQuery = _context.Movie.OrderBy(m => m.Genre).Select(m => m.Genre).Distinct();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                movies = movies.Where(s => s.Title.ToLower().Contains(searchTitle.ToLower()));
            }
            if(!string.IsNullOrEmpty(searchGenres))
            {
                movies = movies.Where(s => s.Genre.ToLower().Contains(searchGenres.ToLower()));
            }

            var VM = new MoviesFilterVM
            {
                genresList = new SelectList(await genreQuery.ToListAsync()),
                movies = await movies.ToListAsync()
            };


            return View(VM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            MoviePosterVM VM = new MoviePosterVM
            {
                movie = movie,
                title = movie.Title
            };

            return View(VM);
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPoster(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _context.Movie.Where(m => m.MovieId == id).First();
            if (movie == null)
            {
                return NotFound();
            }

            MoviePosterVM viewmodel = new MoviePosterVM
            {
                movie = movie,
                title = movie.Title
            };

            return View(viewmodel);
        }

        private string UploadedFile(MoviePosterVM viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.posterImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.posterImage.FileName);
                string fileNameWithPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewmodel.posterImage.CopyTo(stream);
                }
            }
            return uniqueFileName;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPoster(int id, MoviePosterVM viewmodel)
        {
            if (id != viewmodel.movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewmodel.posterImage != null)
                    {
                        string uniqueFileName = UploadedFile(viewmodel);
                        viewmodel.movie.Poster = uniqueFileName;
                    }
                    else
                    {
                        viewmodel.movie.Poster = viewmodel.title;
                    }

                    _context.Update(viewmodel.movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(viewmodel.movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = viewmodel.movie.MovieId });
            }
            return View(viewmodel);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Title,Poster,Genre,Duration,ReleaseDate,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Poster,Genre,Duration,ReleaseDate,Rating")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'CinemaContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.MovieId == id)).GetValueOrDefault();
        }
    }
}
