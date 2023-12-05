using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class apiTSkillsController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public apiTSkillsController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: apiTSkills
        public IActionResult Index()
        {
            var dbSoundBetterContext = _context.TSkills
                .Include(t => t.FSkillCategory);
            return Json(dbSoundBetterContext);
        }

        public IActionResult OnlyIdName()
        {
            var dbSoundBetterContext = _context.TSkills
                .Include(t => t.FSkillCategory)
                .Select(t => new
                {
                    fSkillId = t.FSkillId,
                    fName = t.FName
                });
            return Json(dbSoundBetterContext);
        }

        // GET: apiTSkills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TSkills == null)
            {
                return NotFound();
            }

            var tSkill = await _context.TSkills
                .Include(t => t.FSkillCategory)
                .FirstOrDefaultAsync(m => m.FSkillId == id);
            if (tSkill == null)
            {
                return NotFound();
            }

            return View(tSkill);
        }

        // GET: apiTSkills/Create
        public IActionResult Create()
        {
            ViewData["FSkillCategoryId"] = new SelectList(_context.TSkillCategories, "FSkillCategoryId", "FSkillCategoryId");
            return View();
        }

        // POST: apiTSkills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FSkillId,FName,FSkillCategoryId,FDescription,FThumbnailPath")] TSkill tSkill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FSkillCategoryId"] = new SelectList(_context.TSkillCategories, "FSkillCategoryId", "FSkillCategoryId", tSkill.FSkillCategoryId);
            return View(tSkill);
        }

        // GET: apiTSkills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TSkills == null)
            {
                return NotFound();
            }

            var tSkill = await _context.TSkills.FindAsync(id);
            if (tSkill == null)
            {
                return NotFound();
            }
            ViewData["FSkillCategoryId"] = new SelectList(_context.TSkillCategories, "FSkillCategoryId", "FSkillCategoryId", tSkill.FSkillCategoryId);
            return View(tSkill);
        }

        // POST: apiTSkills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FSkillId,FName,FSkillCategoryId,FDescription,FThumbnailPath")] TSkill tSkill)
        {
            if (id != tSkill.FSkillId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tSkill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TSkillExists(tSkill.FSkillId))
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
            ViewData["FSkillCategoryId"] = new SelectList(_context.TSkillCategories, "FSkillCategoryId", "FSkillCategoryId", tSkill.FSkillCategoryId);
            return View(tSkill);
        }

        // GET: apiTSkills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TSkills == null)
            {
                return NotFound();
            }

            var tSkill = await _context.TSkills
                .Include(t => t.FSkillCategory)
                .FirstOrDefaultAsync(m => m.FSkillId == id);
            if (tSkill == null)
            {
                return NotFound();
            }

            return View(tSkill);
        }

        // POST: apiTSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TSkills == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TSkills'  is null.");
            }
            var tSkill = await _context.TSkills.FindAsync(id);
            if (tSkill != null)
            {
                _context.TSkills.Remove(tSkill);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TSkillExists(int id)
        {
          return (_context.TSkills?.Any(e => e.FSkillId == id)).GetValueOrDefault();
        }
    }
}
