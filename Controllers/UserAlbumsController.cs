using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music.Data;
using Music.Models;
using Microsoft.AspNetCore.Identity;
using Music.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Music.Controllers
{
    public class UserAlbumsController : Controller
    {
        private readonly MusicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAlbumsController(MusicContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserAlbums
        public async Task<IActionResult> Index()
        {
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
                .Include(u => u.Album)
                    .ThenInclude(b => b.Reviews)
                .Where(ub => ub.AppUser == name)
                .ToListAsync();

            return View(userAlbums);
        }

 
       

        // GET: UserAlbums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
                .Include(u => u.Album)
                    .ThenInclude(b => b.Reviews)
                .Where(ub => ub.AppUser == name)
                .FirstOrDefaultAsync(b => b.Album.Id == id);

            return View(userAlbums);
        }

        // GET: UserAlbums/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Title");
            return View();
        }

        // POST: UserAlbums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,AlbumId")] UserAlbum userAlbum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAlbum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Title", userAlbum.AlbumId);
            return View(userAlbum);
        }

        // GET: UserAlbums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAlbums = await _context.UserAlbum.FindAsync(id);
            if (userAlbums == null)
            {
                return NotFound();
            }
           
            return View(userAlbums);
        }

        // POST: UserAlbums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUser,AlbumId")] UserAlbum userAlbum)
        {
            if (id != userAlbum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAlbum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAlbumExists(userAlbum.Id))
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
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Title", userAlbum.AlbumId);
            return View(userAlbum);
        }

        // GET: UserAlbums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAlbum = await _context.UserAlbum
                .Include(u => u.Album)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAlbum == null)
            {
                return NotFound();
            }

            return View(userAlbum);
        }

        // POST: UserAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAlbum = await _context.UserAlbum.FindAsync(id);
            if (userAlbum != null)
            {
                _context.UserAlbum.Remove(userAlbum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAlbumExists(int id)
        {
            return _context.UserAlbum.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddComment(int id, string input, int rating)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var name = user.Email;

            var album = await _context.Album.FirstOrDefaultAsync(e => e.Id == id);

            var review = new Review
            {
                AlbumId = album.Id,
                AppUser = name,
                Comment = input,
                Rating = rating,
                Album = album
            };

            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
     
    }
}

