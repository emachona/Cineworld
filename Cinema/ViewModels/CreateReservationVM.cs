using Cinema.Models;
using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class CreateReservationVM
    {
        public Reservation? reservation { get; set; }

        [Display(Name = "Name")]
        public string loggedUser { get; set; }

        [Display(Name = "Movie")]
        public string movieTitle { get; set; }

        [Display(Name = "Cinema Hall")]
        public string cinemaHall { get; set; }
    }
}
