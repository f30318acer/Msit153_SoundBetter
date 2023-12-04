using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class TArticlePicturesController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public TArticlePicturesController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: TArticlePictures
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TArticlePictures.Include(t => t.FArticle);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: TArticlePictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TArticlePictures == null)
            {
                return NotFound();
            }

            var tArticlePicture = await _context.TArticlePictures
                .Include(t => t.FArticle)
                .FirstOrDefaultAsync(m => m.FArticlePictureId == id);
            if (tArticlePicture == null)
            {
                return NotFound();
            }

            return View(tArticlePicture);
        }

        // GET: TArticlePictures/Create
        public IActionResult Create()
        {
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticleId");
            return View();
        }

        // POST: TArticlePictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FArticlePictureId,FArticleId,FPicturePath")] TArticlePicture tArticlePicture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tArticlePicture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticleId", tArticlePicture.FArticleId);
            return View(tArticlePicture);
        }

        // GET: TArticlePictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TArticlePictures == null)
            {
                return NotFound();
            }

            var tArticlePicture = await _context.TArticlePictures.FindAsync(id);
            if (tArticlePicture == null)
            {
                return NotFound();
            }
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticleId", tArticlePicture.FArticleId);
            return View(tArticlePicture);
        }

        // POST: TArticlePictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FArticlePictureId,FArticleId,FPicturePath")] TArticlePicture tArticlePicture)
        {
            if (id != tArticlePicture.FArticlePictureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tArticlePicture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TArticlePictureExists(tArticlePicture.FArticlePictureId))
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
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticleId", tArticlePicture.FArticleId);
            return View(tArticlePicture);
        }

        // GET: TArticlePictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TArticlePictures == null)
            {
                return NotFound();
            }

            var tArticlePicture = await _context.TArticlePictures
                .Include(t => t.FArticle)
                .FirstOrDefaultAsync(m => m.FArticlePictureId == id);
            if (tArticlePicture == null)
            {
                return NotFound();
            }

            return View(tArticlePicture);
        }

        // POST: TArticlePictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TArticlePictures == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TArticlePictures'  is null.");
            }
            var tArticlePicture = await _context.TArticlePictures.FindAsync(id);
            if (tArticlePicture != null)
            {
                _context.TArticlePictures.Remove(tArticlePicture);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TArticlePictureExists(int id)
        {
          return (_context.TArticlePictures?.Any(e => e.FArticlePictureId == id)).GetValueOrDefault();
        }
    }
}
