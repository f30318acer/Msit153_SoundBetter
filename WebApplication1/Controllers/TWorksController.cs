using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class TWorksController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public TWorksController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: TWorks
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TWorks.Include(t => t.FMember).Include(t => t.FStyle).Include(t => t.FWorkType);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: TWorks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TWorks == null)
            {
                return NotFound();
            }

            var tWork = await _context.TWorks
                .Include(t => t.FMember)
                .Include(t => t.FStyle)
                .Include(t => t.FWorkType)
                .FirstOrDefaultAsync(m => m.FWorkId == id);
            if (tWork == null)
            {
                return NotFound();
            }

            return View(tWork);
        }

        // GET: TWorks/Create
        public IActionResult Create1()
        {
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId");
            ViewData["FWorkTypeId"] = new SelectList(_context.TWorkTypes, "FWorkTypeId", "FWorkTypeId");
            return View();
        }

        // POST: TWorks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FWorkId,FMemberId,FWorkTypeId,FWorkName,FStyleId,FUpdateTime,FDescription,FThumbnail,FFilePath")] TWork tWork, IFormFile file)
        {
            // 將檔案上傳邏輯放在這裡
            if (file != null && file.Length > 0)
            {
                // 處理檔案上傳邏輯，例如保存到伺服器或數據庫
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }



                // 在這裡執行其他邏輯，例如將檔案路徑保存到數據庫中
            }

            if (ModelState.IsValid)
            {
                _context.Add(tWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tWork.FMemberId);
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tWork.FStyleId);
            ViewData["FWorkTypeId"] = new SelectList(_context.TWorkTypes, "FWorkTypeId", "FWorkTypeId", tWork.FWorkTypeId);
            return View(tWork);

     
        }

        // GET: TWorks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TWorks == null)
            {
                return NotFound();
            }

            var tWork = await _context.TWorks.FindAsync(id);
            if (tWork == null)
            {
                return NotFound();
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tWork.FMemberId);
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tWork.FStyleId);
            ViewData["FWorkTypeId"] = new SelectList(_context.TWorkTypes, "FWorkTypeId", "FWorkTypeId", tWork.FWorkTypeId);
            return View(tWork);
        }

        // POST: TWorks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FWorkId,FMemberId,FWorkTypeId,FWorkName,FStyleId,FUpdateTime,FDescription,FThumbnail,FFilePath")] TWork tWork)
        {
            if (id != tWork.FWorkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TWorkExists(tWork.FWorkId))
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
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tWork.FMemberId);
            ViewData["FStyleId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tWork.FStyleId);
            ViewData["FWorkTypeId"] = new SelectList(_context.TWorkTypes, "FWorkTypeId", "FWorkTypeId", tWork.FWorkTypeId);
            return View(tWork);
        }

        // GET: TWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TWorks == null)
            {
                return NotFound();
            }

            var tWork = await _context.TWorks
                .Include(t => t.FMember)
                .Include(t => t.FStyle)
                .Include(t => t.FWorkType)
                .FirstOrDefaultAsync(m => m.FWorkId == id);
            if (tWork == null)
            {
                return NotFound();
            }

            return View(tWork);
        }

        // POST: TWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TWorks == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TWorks'  is null.");
            }
            var tWork = await _context.TWorks.FindAsync(id);
            if (tWork != null)
            {
                _context.TWorks.Remove(tWork);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TWorkExists(int id)
        {
            return (_context.TWorks?.Any(e => e.FWorkId == id)).GetValueOrDefault();
        }
    }
}