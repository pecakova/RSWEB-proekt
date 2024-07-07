using Microsoft.AspNetCore.Mvc.Rendering;
using Music.Models;

namespace Music.ViewModels
{
    public class AlbumGenresCreateViewModel
    {
        public Album? Album { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
        public IEnumerable<SelectListItem>? ArtistsList { get; set; }
        public IFormFile? FrontPageFile { get; set; }
        
    }
}
