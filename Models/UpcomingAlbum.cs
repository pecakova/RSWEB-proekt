using System;
using System.ComponentModel.DataAnnotations;

namespace Music.Models
{
    public class UpcomingAlbum
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Artist { get; set; }

        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public string? FrontPage { get; set; }
    }
}
