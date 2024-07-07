using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Music.Models;
using static System.Reflection.Metadata.BlobBuilder;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Music.Data;
using Microsoft.AspNetCore.Identity;
using Music.Areas.Identity;


namespace Music.Data
{
    public class MusicContext : IdentityDbContext<ApplicationUser>
    {
        public MusicContext (DbContextOptions<MusicContext> options)
            : base(options)
        {
        }

        public DbSet<Music.Models.Artist> Artist { get; set; } = default!;
        public DbSet<Music.Models.Album> Album { get; set; } = default!;
        public DbSet<Music.Models.AlbumGenre> AlbumGenre { get; set; } = default!;
        public DbSet<Music.Models.Genre> Genre { get; set; } = default!;
        public DbSet<Music.Models.Review> Review { get; set; } = default!;
        public DbSet<Music.Models.UserAlbum> UserAlbum { get; set; } = default!;
        public DbSet<Music.Models.UpcomingAlbum> UpcomingAlbum { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure one-to-many relationship between Author and Book
            builder.Entity<Artist>()
                .HasMany(a => a.Albums)
                .WithOne(b => b.Artist)
                .HasForeignKey(b => b.ArtistId);

            builder.Entity<AlbumGenre>()
                .HasKey(ag => new { ag.AlbumId, ag.GenreId });

            builder.Entity<AlbumGenre>()
            .HasOne(ag => ag.Album)
            .WithMany(b => b.AlbumGenres)
            .HasForeignKey(ag => ag.AlbumId);

            builder.Entity<AlbumGenre>()
            .HasOne(ag => ag.Genre)
            .WithMany(g => g.AlbumGenres)
            .HasForeignKey(ag => ag.GenreId);

            builder.Entity<Review>()
            .HasOne(r => r.Album)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.AlbumId);

            builder.Entity<UserAlbum>()
            .HasOne<Album>(ua => ua.Album)
            .WithMany(b => b.UserAlbums)
            .HasForeignKey(ua => ua.AlbumId);
    

            

            base.OnModelCreating(builder);

        }
    }
}
