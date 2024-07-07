using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Music.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Music.Areas.Identity.Data;

namespace Music.Models;
public class SeedData
    {
    public static async Task CreateUserRoles(IServiceProvider serviceProvider)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        IdentityResult roleResult;
        //Add Admin Role
        var roleCheck = await RoleManager.RoleExistsAsync("Admin");
        if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
        ApplicationUser user = await UserManager.FindByEmailAsync("admin@mvcmovie.com");
        if (user == null)
        {
            var User = new ApplicationUser();
            User.Email = "admin@mvcmovie.com";
            User.UserName = "admin@mvcmovie.com";
            string userPWD = "Admin123";
            IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
            //Add default User to Role Admin
            if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
        }
    }
    public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MusicContext(
                serviceProvider.GetRequiredService<DbContextOptions<MusicContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                
                if (context.Artist.Any() || context.Album.Any() || context.AlbumGenre.Any() || context.Genre.Any() || context.Review.Any() || context.UserAlbum.Any() || context.UpcomingAlbum.Any())
                {
                    return;
                }
                context.Artist.AddRange(
                    new Artist
                    {
                        /*Id = 1, */
                        FirstName = "Billie",
                        LastName = "Eilish",
                        StageName = "Billie Eilish",
                        Category = "Singer-songwriter",
                        BirthDate = DateTime.Parse("2001-12-18"),
                        Nationality = "American",
                        Gender = "Female"
                    },
                    new Artist
                    {
                        /*Id = 2, */
                        FirstName = "Abel Makkonen",
                        LastName = "Tesfaye",
                        StageName = "The Weekend",
                        Category = "Singer-songwriter",
                        BirthDate = DateTime.Parse("1990-02-16"),
                        Nationality = "Canadian",
                        Gender = "Male"
                    },
                    new Artist
                    {
                        /*Id = 3, */
                        FirstName = "Solána",
                        LastName = "Imani Rowe",
                        StageName = "SZA",
                        Category = "Solo singer",
                        BirthDate = DateTime.Parse("1989-11-08"),
                        Nationality = "American",
                        Gender = "Female"
                    },
                    new Artist
                    {
                        /*Id = 4, */
                        FirstName = "Michael Joseph",
                        LastName = "Jackson",
                        StageName = "Michael Jackson",
                        Category = "Singer-songwriter",
                        BirthDate = DateTime.Parse("1958-08-29"),
                        Nationality = "American",
                        Gender = "Male"
                    },
                    new Artist
                    {
                        /*Id = 5, */
                        FirstName = "Adele Laurie Blue",
                        LastName = "Adkins",
                        StageName = "Adele",
                        Category = "Singer",
                        BirthDate = DateTime.Parse("1988-05-05"),
                        Nationality = "English",
                        Gender = "Female"
                    },
                    new Artist
                    {
                        /*Id = 6, */
                        FirstName = "",
                        LastName = "",
                        StageName = "Coldplay",
                        Category = "Rock band",
                        BirthDate = DateTime.Parse("1997-2-4"),
                        Nationality = "English",
                        Gender = "Male"
                    },
                    new Artist
                    {
                        /*Id = 7, */
                        FirstName = "Robert Nesta",
                        LastName = "Marley",
                        StageName = "Bob Marley",
                        Category = "Reggae singer, songwriter",
                        BirthDate = DateTime.Parse("1945-02-06"),
                        Nationality = "Jamaican",
                        Gender = "Male"
                    },
                    new Artist
                    {
                        /*Id = 8, */
                        FirstName = "Taylor Alison",
                        LastName = "Swift",
                        StageName = "Taylor Swift",
                        Category = "Singer-songwriter",
                        BirthDate = DateTime.Parse("1989-12-13"),
                        Nationality = "American",
                        Gender = "Female"
                    },
                    new Artist
                    {
                        /*Id = 9, */
                        FirstName = "",
                        LastName = "",
                        StageName = "AC/DC",
                        Category = "Rock band",
                        BirthDate = DateTime.Parse("1973-09-09"),
                        Nationality = "Australian",
                        Gender = "Male"
                    }
                );
                context.SaveChanges();
                context.Album.AddRange(
                    new Album
                    {
                        /*Id = 1, */
                        Title = "Happier Than Ever",
                        YearPublished = 2021,
                        NumSongs = 7,
                        Playlist = "My Future, Therefore I Am, Your Power, Lost Cause, NDA, Happier Than Ever, Male Fantasy",
                        ArtistId = 1,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/4/45/Billie_Eilish_-_Happier_Than_Ever.png"
                    },
                    new Album
                    {
                        /*Id = 2, */
                        Title = "Starboy",
                        YearPublished = 2016,
                        NumSongs = 8,
                        Playlist = "Starboy, False Alarm, I Feel It Coming, Party Monster, Reminder, Rockin', Die For You, Secrets",
                        ArtistId = 2,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/3/39/The_Weeknd_-_Starboy.png"
                    },
                    new Album
                    {
                        /*Id = 3, */
                        Title = "SOS",
                        YearPublished = 2022,
                        NumSongs = 6,
                        Playlist = "Good Days, I Hate U, Shirt, Nobody Gets Me, Kill Bill, Snooze",
                        ArtistId = 3,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/2/2c/SZA_-_S.O.S.png"
                    },
                    new Album
                    {
                        /*Id = 4, */
                        Title = "Thriller",
                        YearPublished = 1982,
                        NumSongs = 7,
                        Playlist = "The Girl Is Mine, Billie Jean, Beat It, Wanna Be Startin' Somethin, Human Nature, P.Y.T, Thriller",
                        ArtistId = 4,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/5/55/Michael_Jackson_-_Thriller.png"
                    },
                    new Album
                    {
                        /*Id = 5, */
                        Title = "30",
                        YearPublished = 2021,
                        NumSongs = 3,
                        Playlist = "Easy On Me, Oh My God, I Drink Wine",
                        ArtistId = 5,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/7/76/Adele_-_30.png"
                    },
                    new Album
                    {
                        /*Id = 6, */
                        Title = "Parachutes",
                        YearPublished = 2000,
                        NumSongs = 4,
                        Playlist = "Shiver, Yellow, Trouble, Don't Panic",
                        ArtistId = 6,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/f/fd/Coldplay_-_Parachutes.png"
                    },
                    new Album
                    {
                        /*Id = 7, */
                        Title = "Legend",
                        YearPublished = 1984,
                        NumSongs = 7,
                        Playlist = "Is This Love, No Woman, No Cry, Could You Be Loved, Three Little Birds, Buffalo Soldier, Get Up,Stand up, Stir It Up ",
                        ArtistId = 7,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/thumb/c/c2/BobMarley-Legend.jpg/220px-BobMarley-Legend.jpg"
                    },
                    new Album
                    {
                        /*Id = 8, */
                        Title = "1989",
                        YearPublished = 2014,
                        NumSongs = 7,
                        Playlist = "Shake It Off, Blank Space, Style, Bad Blood, Wildest Dreams, Out of the Woods, New Romantics",
                        ArtistId = 8,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/f/f6/Taylor_Swift_-_1989.png"
                    },
                    new Album
                    {
                        /*Id = 9, */
                        Title = "Highway to Hell",
                        YearPublished = 1979,
                        NumSongs = 3,
                        Playlist = "Highway to Hell, Girls Got Rhythm, Touch Too Much",
                        ArtistId = 9,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/a/ac/Acdc_Highway_to_Hell.JPG"
                    },
                    new Album
                    {
                        /*Id = 10, */
                        Title = "When We All Fall Asleep, Where Do We Go?",
                        YearPublished = 2019,
                        NumSongs = 7,
                        Playlist = "You Should See Me In a Crown, When the Party's Over, Bury a Friend, Wish You Were Gay, Bad Guy, All the Good Girls Go to Hell, Ilomilo",
                        ArtistId = 1,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/3/38/When_We_All_Fall_Asleep%2C_Where_Do_We_Go%3F.png"
                    },
                    new Album
                    {
                        /*Id = 11, */
                        Title = "Midnights",
                        YearPublished = 2022,
                        NumSongs = 3,
                        Playlist = "Anti-Hero, Lavender Haze, Karma",
                        ArtistId = 8,
                        FrontPage = "https://upload.wikimedia.org/wikipedia/en/9/9f/Midnights_-_Taylor_Swift.png"
                    }
                );
                context.SaveChanges();
                context.Genre.AddRange(
                    new Genre {/* Id = 1 */ GenreName = "Electropop"},
                    new Genre { /* Id = 2 */ GenreName = "Dream pop" },
                    new Genre { /* Id = 3 */ GenreName = "Synth-pop"},
                    new Genre { /* Id = 4 */ GenreName = "Downtempo" },
                    new Genre { /* Id = 5 */ GenreName = "Pop"},
                    new Genre { /* Id = 6 */ GenreName = "Rock"},
                    new Genre { /* Id = 7 */ GenreName = "R&B"},
                    new Genre { /* Id = 8 */ GenreName = "Hip hop"},
                    new Genre { /* Id = 9 */ GenreName = "Alternative rock" },
                    new Genre { /* Id = 10 */ GenreName = "Indie rock" },
                    new Genre { /* Id = 11 */ GenreName = "Roots reggae" },
                    new Genre { /* Id = 12 */ GenreName = "Hard rock" },
                    new Genre { /* Id = 13 */ GenreName = "Jazz"}
                    );
                context.SaveChanges();
                context.AlbumGenre.AddRange(
                    new AlbumGenre { AlbumId = 1, GenreId = 1 },
                    new AlbumGenre { AlbumId = 1, GenreId = 4 },
                    new AlbumGenre { AlbumId = 2, GenreId = 7 },
                    new AlbumGenre { AlbumId = 2, GenreId = 5 },
                    new AlbumGenre { AlbumId = 3, GenreId = 7 },
                    new AlbumGenre { AlbumId = 3, GenreId = 8 },
                    new AlbumGenre { AlbumId = 3, GenreId = 5 },
                    new AlbumGenre { AlbumId = 4, GenreId = 5 },
                    new AlbumGenre { AlbumId = 4, GenreId = 6 },
                    new AlbumGenre { AlbumId = 4, GenreId = 3 },
                    new AlbumGenre { AlbumId = 4, GenreId = 7 },
                    new AlbumGenre { AlbumId = 5, GenreId = 13 },
                    new AlbumGenre { AlbumId = 5, GenreId = 5 },
                    new AlbumGenre { AlbumId = 6, GenreId = 10 },
                    new AlbumGenre { AlbumId = 6, GenreId = 9 },
                    new AlbumGenre { AlbumId = 7, GenreId = 11 },
                    new AlbumGenre { AlbumId = 8, GenreId = 3 },
                    new AlbumGenre { AlbumId = 9, GenreId = 12 },
                    new AlbumGenre { AlbumId = 10, GenreId = 1 },
                    new AlbumGenre { AlbumId = 10, GenreId = 5 },
                    new AlbumGenre { AlbumId = 11, GenreId = 1 },
                    new AlbumGenre { AlbumId = 11, GenreId = 2 },
                    new AlbumGenre { AlbumId = 11, GenreId = 3 }
                    );
                context.SaveChanges();
                context.Review.AddRange(
                    new Review
                    {
                        /*Id = 1, */
                        AlbumId = 1,
                        AppUser = "AnneM",
                        Comment = "I love it!",
                        Rating = 10,
                    },
                    new Review
                    {
                        /*Id = 2, */
                        AlbumId = 6,
                        AppUser = "Tod",
                        Comment = "All time favourite!",
                        Rating = 10,
                    },
                    new Review
                    {
                        /*Id = 3, */
                        AlbumId = 3,
                        AppUser = "Marie",
                        Comment = "Great!",
                        Rating = 10,
                    },
                    new Review
                    {
                        /*Id = 4, */
                        AlbumId = 2,
                        AppUser = "Tom",
                        Comment = "Good one!",
                        Rating = 9,
                    },
                    new Review
                    {
                        /*Id = 5, */
                        AlbumId = 2,
                        AppUser = "T.M",
                        Comment = "Cool",
                        Rating = 10,
                    },
                    new Review
                    {
                        /*Id = 6, */
                        AlbumId = 9,
                        AppUser = "Suzy",
                        Comment = "All time favourite!",
                        Rating = 10,
                    },
                    new Review
                    {
                        /*Id = 7, */
                        AlbumId = 11,
                        AppUser = "Anna",
                        Comment = "Love her, but the album it's okay",
                        Rating = 9,
                    }
                    );
                context.SaveChanges();
                context.UserAlbum.AddRange(
                      new UserAlbum
                      {
                          AlbumId = 1,
                          AppUser = "User 1"
                      },
                      new UserAlbum
                      {
                          AlbumId = 1,
                          AppUser = "User 2"
                      },
                      new UserAlbum
                      {
                          AlbumId = 2,
                          AppUser = "User 3"
                      },
                      new UserAlbum
                      {
                          AlbumId = 3,
                          AppUser = "User 4"
                      },
                      new UserAlbum
                      {
                          AlbumId = 3,
                          AppUser = "User 5"
                      },
                      new UserAlbum
                      {
                          AlbumId = 3,
                          AppUser = "User 5"
                      },
                      new UserAlbum
                      {
                          AlbumId = 4,
                          AppUser = "User 6"
                      },
                      new UserAlbum
                      {
                          AlbumId = 5,
                          AppUser = "User 7"
                      },
                      new UserAlbum
                      {
                          AlbumId = 5,
                          AppUser = "TodT"
                      },
                      new UserAlbum
                      {
                          AlbumId = 1,
                          AppUser = "Nicole"
                      }
                      
                  );
                context.SaveChanges(); 
            context.UpcomingAlbum.AddRange(
                new UpcomingAlbum
                {
                    /* Id = 1 */
                    Title = "Short n' Sweet",
                    ReleaseDate = DateTime.Parse("2024-08-23"),
                    Artist = "Sabrina Carpenter",
                    FrontPage = "https://upload.wikimedia.org/wikipedia/en/f/fd/Short_n%27_Sweet_-_Sabrina_Carpenter.png"
                },
                new UpcomingAlbum
                {
                    /* Id = 2 */
                    Title = "Don't Be Dumb",
                    ReleaseDate = DateTime.Parse("2024-08-30"),
                    Artist = "ASAP Rocky",
                    FrontPage = "https://images.genius.com/189f3b965c231a90221c35dc11580008.1000x1000x1.jpg"
                },
                new UpcomingAlbum
                {
                    /* Id = 3 */
                    Title = "Memoir of a Sparklemuffin\r\n",
                    ReleaseDate = DateTime.Parse("2024-09-13"),
                    Artist = "Suki Waterhouse",
                    FrontPage = "https://megamart.subpop.com/cdn/shop/files/SukiWaterhouse_MOAS_MockUp_LP_USLoser_2000x1417_68a38202-6517-4073-835b-1c32be29004a_740x.jpg?v=1718729220"
                }
                );
            context.SaveChanges();
            }
        }
    }
