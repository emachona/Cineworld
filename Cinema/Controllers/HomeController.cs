using Cinema.Areas.Identity.Data;
using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaContext _context;
        private readonly UserManager<CinemaUser> _userManager;

        public HomeController(ILogger<HomeController> logger, CinemaContext context, UserManager<CinemaUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Authorize(Roles = "Admin")]
        //public IActionResult RegisterClient(string userID)
        //{
        //    if (userID == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Client = userID;
        //    ViewData["ClientId"] = new SelectList(_context.User, "UserId", "FullName");
        //    return View();
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RegisterClient(string? userID, int? ClientId)
        //{
        //    if (userID == null)
        //    {
        //        return NotFound();
        //    }
        //    var client = _context.User.Where(m => m.UserId == ClientId).First();
        //    client.userId = userID;
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}