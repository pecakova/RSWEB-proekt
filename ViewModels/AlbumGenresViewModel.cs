using Microsoft.AspNetCore.Mvc.Rendering;
using Music.Models;

namespace Music.ViewModels
{
    public class AlbumGenresViewModel
    {
        public IList<Album> Albums { get; set; }
        public SelectList Genres { get; set; }
        public string AlbumGenre { get; set; }
        public string SearchString { get; set; }
        public IList<Artist> Artists { get; set; }
        public string ArtistSearchString { get; set; }
        public int Reviews { get; set; }
    }
}
