using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Music.Models
{
    public class UserAlbum
    {
        public int Id { get; set; }
        [StringLength(450)]

        public string AppUser { get; set; }
        public int AlbumId { get; set; }
        public Album? Album { get; set; }
    }
}
