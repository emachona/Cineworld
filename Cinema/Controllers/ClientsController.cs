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
using Cinema.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Controllers
{
    public class ClientsController : Controller
    {
        private readonly CinemaContext _context;
        private readonly UserManager<CinemaUser> _userManager;

        public ClientsController(CinemaContext context, UserManager<CinemaUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Index(string searchUser)
        {
            IEnumerable<Client> users = _context.Client.AsEnumerable();


            if (!string.IsNullOrEmpty(searchUser))
            {
                users = users.Where(s => s.FullName.ToLower().Contains(searchUser.ToLower()));
            }

            var VM = new UsersFilterVM
            {
                users = users
            };

            return View(VM);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Profile()
        {
            var userIdentity = _userManager.GetUserAsync(User).Result.user_ID;
            var client = await _context.Client.Where(x => x.ClientId == userIdentity).FirstOrDefaultAsync();

            ViewBag.id = userIdentity; 
            ViewBag.client = client.FullName;

            return View(client);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var user = await _context.Client
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,FirstName,LastName,Age")] Client user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var user = await _context.Client.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,FirstName,LastName,Age")] Client user)
        {
            if (id != user.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists((int)user.ClientId))
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
            return View(user);
        }

        public async Task<IActionResult> CreateAccount(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var user = await _context.Client.FindAsync(id);
            ViewData["ClientId"] = id;
            ViewData["userId"] = user.user;
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(int id, [Bind("ClientId,FirstName,LastName,Age,user")] Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var role = "Client";
                    CinemaUser user = _userManager.Users.FirstOrDefault(u => u.Id == client.user);
                    await _userManager.AddToRoleAsync(user, role);
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists((int)client.ClientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(client);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var user = await _context.Client
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Client == null)
            {
                return Problem("Entity set 'CinemaContext.User'  is null.");
            }
            var user = await _context.Client.FindAsync(id);
            if (user != null)
            {
                _context.Client.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Client?.Any(e => e.ClientId == id)).GetValueOrDefault();
        }
    }
}
