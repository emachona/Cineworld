using Cinema.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.ViewModels
{
    public class ReservationFilterVM
    {
        public IList<Reservation> reservations { get; set; }
        public SelectList movieTitleList { get; set; }
        public string searchMovie { get; set; }
    }
}
