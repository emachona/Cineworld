using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Movie
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        public string? Poster { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(30)]
        public string? Genre { get; set; }

        [Display(Name = "Duration")]
        public int? Duration { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? Rating { get; set; }

        public ICollection<Screening>? Projections { get; set; }
    }
}
