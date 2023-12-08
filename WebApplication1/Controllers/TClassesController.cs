using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;

namespace prjMusicBetter.Controllers
{
    public class TClassesController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        public TClassesController(dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _userInfoService = userInfoService;//抓使用者
        }

        // GET: TClasses
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TClasses.Include(t => t.FSite).Include(t => t.FTeacher).Include(t => t.FSkill);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: TClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TClasses == null)
            {
                return NotFound();
            }

            var tClass = await _context.TClasses
                .Include(t => t.FSite)
                .Include(t => t.FTeacher)
                .Include(t => t.FSkill)
                .FirstOrDefaultAsync(m => m.FClassId == id);
            if (tClass == null)
            {
                return NotFound();
            }

            return View(tClass);
        }

        // GET: TClasses/Create
        public IActionResult Create()
        {
            TMember member = _userInfoService.GetMemberInfo();
            ViewBag.MemberId = member.FMemberId;

            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId");
            ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            return View();
        }

        // POST: TClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FClassId,FTeacherId,FClassName,FPrice,FDescription,FStartdate,FEnddate,FSiteId,FThumbnailPath")] TClass tClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tClass.FSiteId);
            ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClass.FTeacherId);
            return View(tClass);
        }

        // GET: TClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TClasses == null)
            {
                return NotFound();
            }

            var tClass = await _context.TClasses.FindAsync(id);
            if (tClass == null)
            {
                return NotFound();
            }
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tClass.FSiteId);
            ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClass.FTeacherId);

            ViewBag.SiteId = tClass.FSiteId;//原本的地址

            return View(tClass);
        }

        // POST: TClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FClassId,FTeacherId,FClassName,FPrice,FDescription,FStartdate,FEnddate,FSiteId,FThumbnailPath")] TClass tClass)
        {
            if (id != tClass.FClassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TClassExists(tClass.FClassId))
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
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tClass.FSiteId);
            ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClass.FTeacherId);
            return View(tClass);
        }

        // GET: TClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TClasses == null)
            {
                return NotFound();
            }

            var tClass = await _context.TClasses
                .Include(t => t.FSite)
                .Include(t => t.FTeacher)
                 .Include(t => t.FSkill)
                .FirstOrDefaultAsync(m => m.FClassId == id);
            if (tClass == null)
            {
                return NotFound();
            }

            return View(tClass);
        }

        // POST: TClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TClasses == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TClasses'  is null.");
            }
            var tClass = await _context.TClasses.FindAsync(id);
            if (tClass != null)
            {
                _context.TClasses.Remove(tClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TClassExists(int id)
        {
          return (_context.TClasses?.Any(e => e.FClassId == id)).GetValueOrDefault();
        }
    }
}
