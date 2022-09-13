using Cinema.Models;
using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class MoviePosterVM
    {
        public Movie? movie { get; set; }

        [Display(Name = "Movie Poster")]
        public IFormFile? posterImage { get; set; }

        [Display(Name = "Movie Title")]
        public string? title { get; set; }
    }
}
