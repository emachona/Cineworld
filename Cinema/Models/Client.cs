using Cinema.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Client
    {
        public int? ClientId { get; set; }

        [Required]
        [Display(Name ="First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public ICollection<Reservation>? Reservations { get; set; }

        public string? user { get; set; }
    }
}
