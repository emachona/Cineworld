using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Reservation
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int ScreeningId { get; set; }
        public Screening? Screening{ get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int Seat { get; set; }
    }
}
