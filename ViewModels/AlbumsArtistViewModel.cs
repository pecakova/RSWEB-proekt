using Microsoft.AspNetCore.Mvc.Rendering;
using Music.Models;
using System.Collections.Generic;



namespace Music.ViewModels
{
    public class AlbumsArtistViewModel
    {
        public IList<Artist> Artists { get; set; }

        public string SearchName { get; set; }
        public string SearchCategory { get; set; }
    }
}
