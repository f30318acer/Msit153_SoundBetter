using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using prjMusicBetter.Models;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Controllers
{
    public class TCommentsController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public TCommentsController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: TComments
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TComments.Include(t => t.FArticleId).Include(t => t.FMemberId);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: TComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TComments == null)
            {
                return NotFound();
            }

            var tComment = await _context.TComments
                .Include(t => t.FArticleId)
                .Include(t => t.FMemberId)
                .FirstOrDefaultAsync(m => m.FArticleId == id);
            if (tComment == null)
            {
                return NotFound();
            }

            return View(tComment);
        }

        // GET: TComments/tComment
        public IActionResult Create()
        {
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticle");
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMember");
            return View();
        }

        // POST: TComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(TComment? tComment, TArticle tArticle)
        {
            //if (tArticle != null)
            //{
                if (tComment != null)
                {
                    _context.Add(tComment);
                    _context.SaveChanges();

                    _context.SaveChanges();
                    return Content("新增成功");
                }
            //}
            return Content("錯誤");
        }
        // GET: TComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TComments == null)
            {
                return NotFound();
            }

            var tComment = await _context.TComments.FindAsync(id);
            if (tComment == null)
            {
                return NotFound();
            }
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticle", tComment.FArticleId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tComment.FMemberId);
            
            return View(tComment);
        }

        // POST: TComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FArticleId,FMemberId,FCommentContent,FCommentTime")] TComment tComment)
        {
            if (id != tComment.FCommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TCommentExists(tComment.FCommentId))
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
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticle", tComment.FArticleId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tComment.FMemberId);
            
            return View(tComment);
        }

        // GET: TComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TComments == null)
            {
                return NotFound();
            }

            var tComment = await _context.TComments
                .Include(t => t.FArticle)
                .Include(t => t.FMember)
                .FirstOrDefaultAsync(m => m.FCommentId == id);
            if (tComment == null)
            {
                return NotFound();
            }

            return View(tComment);
        }

        // POST: TComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TComments == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TComments'  is null.");
            }
            var tComment = await _context.TComments.FindAsync(id);
            if (tComment != null)
            {
                _context.TComments.Remove(tComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TCommentExists(int id)
        {
            return (_context.TComments?.Any(e => e.FCommentId == id)).GetValueOrDefault();
        }
    }
}
