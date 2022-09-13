using Cinema.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.ViewModels
{
    public class ScreeningFilterVM
    {
        public IList<Screening> screenings { get; set; }
        public string searchMovies { get; set; }
        public SelectList moviesList { get; set; }
        public String searchTime { get; set; }
        public String searchDate { get; set; }
        public String searchCinema { get; set; }
    }
}
