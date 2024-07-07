using System.ComponentModel.DataAnnotations;

namespace Music.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string? Title { get; set; }
        [Display(Name = "Released")]
        public int? YearPublished { get; set; }
        public int? NumSongs { get; set; }
        [StringLength(int.MaxValue)]
        public string? Playlist { get; set; }
        [StringLength(int.MaxValue)]
        public string? FrontPage { get; set; }
        public int? ArtistId { get; set; }
        public Artist? Artist { get; set; }
        public ICollection<AlbumGenre>? AlbumGenres { get; set; }

        public ICollection<Review>? Reviews { get; set; } //One-to-many relationship with Review
        public ICollection<UserAlbum>? UserAlbums { get; set; }
        public double AverageRating()
        {
            if (Reviews != null && Reviews.Any())
            {
                int totalRating = (int)Reviews.Sum(r => r.Rating);
                return (double)totalRating / Reviews.Count;
            }
            return 0;
        }


    }
}
