using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music.Data;
using Music.Models;
using Music.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;


namespace Music.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MusicContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlbumsController(MusicContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        // GET: Albums
        public async Task<IActionResult> Index(string albumGenre, string searchString, string artistSearchString)
        {
            var genreQuery = _context.Genre
               .OrderBy(g => g.GenreName)
               .Select(g => g.GenreName)
               .Distinct();

            var albumsQuery = _context.Album
                .Include(b => b.Reviews)
                .Include(b => b.AlbumGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(b => b.Artist)
                .AsQueryable();

            var artistsQuery = _context.Artist.AsQueryable();

            if (!string.IsNullOrEmpty(albumGenre))
            {
                albumsQuery = albumsQuery.Where(b => b.AlbumGenres.Any(bg => bg.Genre.GenreName == albumGenre));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                albumsQuery = albumsQuery.Where(b => b.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(artistSearchString))
            {
                albumsQuery = albumsQuery.Where(a => (a.Artist.StageName).Contains(artistSearchString));
            }

            var genres = await genreQuery.ToListAsync();
            var albums = await albumsQuery.ToListAsync();
            var authors = await artistsQuery.ToListAsync();

            var viewModel = new AlbumGenresViewModel
            {
                Albums = albums,
                Genres = new SelectList(genres),
                AlbumGenre = albumGenre,
                SearchString = searchString,
                Artists = authors,
                ArtistSearchString = artistSearchString
            };

            return View(viewModel);
        }
        public async Task<IActionResult> IndexById(int id)
        {
            //albumot shto treba da se dodade
            var album = await _context.Album
                .Include(b => b.Artist)
                .Include(b => b.AlbumGenres).ThenInclude(bg => bg.Genre)
                .Include(albums => albums.Reviews)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var name = user.Email;
            var userAlbums = await _context.UserAlbum
                .Include(u => u.Album)
                    .ThenInclude(b => b.AlbumGenres)
                        .ThenInclude(bg => bg.Genre)
                .Include(u => u.Album)
                    .ThenInclude(b => b.Artist)
                .Where(ub => ub.AppUser == name)
                .ToListAsync();

            var exist = userAlbums.Any(b => b.Album.Id == id);

            if (exist)
            {
                return RedirectToAction(nameof(Index));
            }

            var newMyBook = new UserAlbum
            {
                AlbumId = id,
                AppUser = name
            };

            _context.UserAlbum.Add(newMyBook);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Artist)
                .Include(b => b.AlbumGenres)
                .ThenInclude(ag => ag.Genre)
                .Include(albums => albums.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            var genres = _context.Genre.OrderBy(g => g.GenreName).ToList();

            var viewModel = new AlbumGenresCreateViewModel
            {
                Album = new Album(),
                ArtistsList = _context.Artist
                    .OrderBy(a => a.StageName)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.FullName,
                    }).ToList(),
                GenreList = new SelectList(genres, "Id", "GenreName")
            };

            return View(viewModel);
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumGenresCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.FrontPageFile != null && viewModel.FrontPageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.FrontPageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.FrontPageFile.CopyToAsync(fileStream);
                    }

                    // Save file path in the database
                    viewModel.Album.FrontPage = "/images/" + uniqueFileName;
                    //book.FrontPage = filePath;
                }
                try
                {
                    _context.Update(viewModel.Album);
                    await _context.SaveChangesAsync();

                    foreach (int genreId in viewModel.SelectedGenres)
                    {
                        _context.AlbumGenre.Add(new AlbumGenre { GenreId = genreId, AlbumId = viewModel.Album.Id });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while saving the entity changes.");
                    Console.WriteLine(ex.InnerException?.Message);
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Albums/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }

            var albums = await _context.Album
                .Where(m => m.Id == id)
                .Include(m => m.AlbumGenres)
                .FirstOrDefaultAsync();
            if (albums == null)
            {
                return NotFound();
            }
            var genres = _context.Genre.AsEnumerable();
            genres = genres.OrderBy(s => s.GenreName);

            AlbumGenresEditViewModel viewModel = new AlbumGenresEditViewModel
            {
                Album = albums,
                GenreList = new MultiSelectList(genres, "Id", "GenreName"),
                SelectedGenres = albums.AlbumGenres.Select(sa => sa.GenreId).ToList()
            };

            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "FullName", albums.ArtistId);
            return View(viewModel);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlbumGenresEditViewModel viewModel)
        {
            if (id != viewModel?.Album?.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewModel.FrontPageFile != null && viewModel.FrontPageFile.Length > 0)
                    {
                        // Save FrontPageFile
                        string uniqueFrontPageFileName = Guid.NewGuid().ToString() + "_" + viewModel.FrontPageFile.FileName;
                        string frontPageFilePath = Path.Combine(_environment.WebRootPath, "images", uniqueFrontPageFileName);

                        using (var fileStream = new FileStream(frontPageFilePath, FileMode.Create))
                        {
                            await viewModel.FrontPageFile.CopyToAsync(fileStream);
                        }

                        viewModel.Album.FrontPage = "/images/" + uniqueFrontPageFileName; // Update file path
                    }

                    // If FrontPageFile and PdfFile are not uploaded, retain the existing values
                    if (viewModel.FrontPageFile == null)
                    {
                        var existingBook = _context.Album.AsNoTracking().FirstOrDefault(b => b.Id == id);
                        if (existingBook != null)
                        {
                            viewModel.Album.FrontPage = existingBook.FrontPage;
                        
                        }
                    }
                    _context.Update(viewModel.Album);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> newGenreList = viewModel.SelectedGenres ?? new List<int>();
                    IEnumerable<int> prevGenreList = _context.AlbumGenre.Where(s => s.AlbumId == id).Select(s => s.GenreId).ToList();

                    IQueryable<AlbumGenre> toBeRemoved = _context.AlbumGenre.Where(s => s.AlbumId == id);
                    if (newGenreList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));
                        foreach (int actorId in newGenreList)
                        {
                            if (!prevGenreList.Any(s => s == actorId))
                            {
                                _context.AlbumGenre.Add(new AlbumGenre { GenreId = actorId, AlbumId = id });
                            }
                        }
                    }

                    _context.AlbumGenre.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(viewModel.Album.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "FullName", viewModel.Album.ArtistId);
            return View(viewModel);
        }

        // GET: Albums/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Album == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(b => b.Artist)
                .Include(b => b.Reviews)
                .Include(b => b.AlbumGenres)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Album == null)
            {
                return Problem("Entity set 'AlbumStoreContext.Albums'  is null.");
            }


            var album = await _context.Album.FindAsync(id);
            if (album != null)
            {
                _context.Album.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.Id == id);
        }
        public IActionResult ViewFile(string fileName)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
            if (System.IO.File.Exists(filePath))
            {
                var provider = new FileExtensionContentTypeProvider();
                if (provider.TryGetContentType(fileName, out var contentType))
                {
                    return PhysicalFile(filePath, contentType);
                }
                else
                {
                    return PhysicalFile(filePath, "application/octet-stream"); // Default to octet-stream if MIME type cannot be determined
                }
            }
            else
            {
                return NotFound(); // Return 404 if the file does not exist
            }
        }
    }
}
