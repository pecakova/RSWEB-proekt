using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music.Data;
using Music.Models;

namespace Music.Controllers
{
    public class UpcomingAlbumsController : Controller
    {
        private readonly MusicContext _context;

        public UpcomingAlbumsController(MusicContext context)
        {
            _context = context;
        }

        // GET: UpcomingAlbums
        public async Task<IActionResult> Index()
        {
            var upcomingAlbums = await _context.UpcomingAlbum.ToListAsync();
            return View(upcomingAlbums);
        }

        // GET: UpcomingAlbums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upcomingAlbum = await _context.UpcomingAlbum
                .FirstOrDefaultAsync(m => m.Id == id);
            if (upcomingAlbum == null)
            {
                return NotFound();
            }

            return View(upcomingAlbum);
        }

        // GET: UpcomingAlbums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UpcomingAlbums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Artist,ReleaseDate")] UpcomingAlbum upcomingAlbum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(upcomingAlbum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(upcomingAlbum);
        }

        // GET: UpcomingAlbums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upcomingAlbum = await _context.UpcomingAlbum.FindAsync(id);
            if (upcomingAlbum == null)
            {
                return NotFound();
            }
            return View(upcomingAlbum);
        }

        // POST: UpcomingAlbums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Artist,ReleaseDate")] UpcomingAlbum upcomingAlbum)
        {
            if (id != upcomingAlbum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(upcomingAlbum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UpcomingAlbumExists(upcomingAlbum.Id))
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
            return View(upcomingAlbum);
        }

        // GET: UpcomingAlbums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var upcomingAlbum = await _context.UpcomingAlbum
                .FirstOrDefaultAsync(m => m.Id == id);
            if (upcomingAlbum == null)
            {
                return NotFound();
            }

            return View(upcomingAlbum);
        }

        // POST: UpcomingAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var upcomingAlbum = await _context.UpcomingAlbum.FindAsync(id);
            if (upcomingAlbum != null)
            {
                _context.UpcomingAlbum.Remove(upcomingAlbum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UpcomingAlbumExists(int id)
        {
            return _context.UpcomingAlbum.Any(e => e.Id == id);
        }
    }
}
