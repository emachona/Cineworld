using Cinema.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.ViewModels
{
    public class MoviesFilterVM
    {
        public IList<Movie> movies { get; set; }
        public string searchGenres { get; set; }
        public SelectList genresList { get; set; }
        public String searchTitle { get; set; }
    }
}
