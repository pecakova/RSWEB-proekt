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

namespace Music.Controllers
{
    public class AlbumGenresController : Controller
    {
        private readonly MusicContext _context;

        public AlbumGenresController(MusicContext context)
        {
            _context = context;
        }

        // GET: AlbumGenres
        public async Task<IActionResult> Index()
        {
            var musicContext = _context.AlbumGenre.Include(a => a.Album).Include(a => a.Genre);
            return View(await musicContext.ToListAsync());
        }

        // GET: AlbumGenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumGenre = await _context.AlbumGenre
                .Include(a => a.Album)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (albumGenre == null)
            {
                return NotFound();
            }

            return View(albumGenre);
        }

        // GET: AlbumGenres/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Title");
            ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "Id", "Id");
            return View();
        }

        // POST: AlbumGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AlbumId,GenreId")] AlbumGenre albumGenre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(albumGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Title", albumGenre.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "Id", "Id", albumGenre.GenreId);
            return View(albumGenre);
        }

        // GET: AlbumGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumGenre = await _context.AlbumGenre.FindAsync(id);
            if (albumGenre == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Title", albumGenre.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "Id", "Id", albumGenre.GenreId);
            return View(albumGenre);
        }

        // POST: AlbumGenres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AlbumId,GenreId")] AlbumGenre albumGenre)
        {
            if (id != albumGenre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(albumGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumGenreExists(albumGenre.Id))
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
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Title", albumGenre.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "Id", "Id", albumGenre.GenreId);
            return View(albumGenre);
        }

        // GET: AlbumGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumGenre = await _context.AlbumGenre
                .Include(a => a.Album)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (albumGenre == null)
            {
                return NotFound();
            }

            return View(albumGenre);
        }

        // POST: AlbumGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albumGenre = await _context.AlbumGenre.FindAsync(id);
            if (albumGenre != null)
            {
                _context.AlbumGenre.Remove(albumGenre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumGenreExists(int id)
        {
            return _context.AlbumGenre.Any(e => e.Id == id);
        }
    }
}
