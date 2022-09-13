using Microsoft.OData.Edm;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Screening
    {
        [Required]
        public int ScreeningId { get; set; }

        [Required]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        [Display(Name = "Time")] 
        [DataType(DataType.Time)]
        public DateTime? Time { get; set; }

        public string? Cinema { get; set; }

        public int AvailablePlaces { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public int Price { get; set; }

        public string? Type { get; set; } // 2D ili 3D

        public ICollection<Reservation>? Audience { get; set; }
    }
}
