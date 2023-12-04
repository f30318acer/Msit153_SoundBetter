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
    public class TArticlesController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public TArticlesController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: TArticles
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TArticles.Include(t => t.FMember).Include(t => t.FStyle);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: TArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = await _context.TArticles
                .Include(t => t.FMember)
                .Include(t => t.FStyle)
                .FirstOrDefaultAsync(m => m.FArticleId == id);
            if (tArticle == null)
            {
                return NotFound();
            }

            return View(tArticle);
        }

        // GET: TArticles/Create
        public IActionResult Create()
        {
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId");
            return View();
        }

        // POST: TArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FArticleId,FMemberId,FContent,FStyleId,FUpdateTime,FTitle,FPhotoPath")] TArticle tArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tArticle.FMemberId);
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tArticle.FStyleId);
            return View(tArticle);
        }

        // GET: TArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = await _context.TArticles.FindAsync(id);
            if (tArticle == null)
            {
                return NotFound();
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tArticle.FMemberId);
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tArticle.FStyleId);
            return View(tArticle);
        }

        // POST: TArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FArticleId,FMemberId,FContent,FStyleId,FUpdateTime,FTitle,FPhotoPath")] TArticle tArticle)
        {
            if (id != tArticle.FArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TArticleExists(tArticle.FArticleId))
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
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tArticle.FMemberId);
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tArticle.FStyleId);
            return View(tArticle);
        }

        // GET: TArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = await _context.TArticles
                .Include(t => t.FMember)
                .Include(t => t.FStyle)
                .FirstOrDefaultAsync(m => m.FArticleId == id);
            if (tArticle == null)
            {
                return NotFound();
            }

            return View(tArticle);
        }

        // POST: TArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TArticles == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TArticles'  is null.");
            }
            var tArticle = await _context.TArticles.FindAsync(id);
            if (tArticle != null)
            {
                _context.TArticles.Remove(tArticle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TArticleExists(int id)
        {
          return (_context.TArticles?.Any(e => e.FArticleId == id)).GetValueOrDefault();
        }
    }
}
