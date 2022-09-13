using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }

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
    }
}
