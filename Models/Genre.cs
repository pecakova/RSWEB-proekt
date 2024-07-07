using System.ComponentModel.DataAnnotations;
namespace Music.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? GenreName { get; set; }
        public ICollection<AlbumGenre>? AlbumGenres { get; set; }
    }
}
