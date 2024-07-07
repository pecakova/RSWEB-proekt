using System.ComponentModel.DataAnnotations;

namespace Music.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        [StringLength(450)]
        public string? AppUser { get; set; }
        [StringLength(500)]
        public string? Comment { get; set; }
        public int? Rating { get; set; }
        public Album? Album { get; set; }
    }
}
