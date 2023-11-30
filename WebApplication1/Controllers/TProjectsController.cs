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
    public class TProjectsController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public TProjectsController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: TProjects
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TProjects.Include(t => t.FMember).Include(t => t.FProjectStatus).Include(t => t.FSite).Include(t => t.FSiteNavigation);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: TProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TProjects == null)
            {
                return NotFound();
            }

            var tProject = await _context.TProjects
                .Include(t => t.FMember)
                .Include(t => t.FProjectStatus)
                .Include(t => t.FSite)
                .Include(t => t.FSiteNavigation)
                .FirstOrDefaultAsync(m => m.FProjectId == id);
            if (tProject == null)
            {
                return NotFound();
            }

            return View(tProject);
        }

        // GET: TProjects/Create
        public IActionResult Create()
        {
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            ViewData["FProjectStatusId"] = new SelectList(_context.TProjectStatuses, "FProjectStatusId", "FProjectStatusId");
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId");
            ViewData["FSiteId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId");
            return View();
        }

        // POST: TProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FProjectId,FMemberId,FName,FBudget,FStartdate,FEnddate,FDescription,FProjectStatusId,FSiteId,FThumbnailPath,FDescription2,FStyleId")] TProject tProject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tProject.FMemberId);
            ViewData["FProjectStatusId"] = new SelectList(_context.TProjectStatuses, "FProjectStatusId", "FProjectStatusId", tProject.FProjectStatusId);
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tProject.FSiteId);
            ViewData["FSiteId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tProject.FSiteId);
            return View(tProject);
        }

        // GET: TProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TProjects == null)
            {
                return NotFound();
            }

            var tProject = await _context.TProjects.FindAsync(id);
            if (tProject == null)
            {
                return NotFound();
            }
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tProject.FMemberId);
            ViewData["FProjectStatusId"] = new SelectList(_context.TProjectStatuses, "FProjectStatusId", "FProjectStatusId", tProject.FProjectStatusId);
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tProject.FSiteId);
            ViewData["FSiteId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tProject.FSiteId);
            return View(tProject);
        }

        // POST: TProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FProjectId,FMemberId,FName,FBudget,FStartdate,FEnddate,FDescription,FProjectStatusId,FSiteId,FThumbnailPath,FDescription2,FStyleId")] TProject tProject)
        {
            if (id != tProject.FProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TProjectExists(tProject.FProjectId))
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
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tProject.FMemberId);
            ViewData["FProjectStatusId"] = new SelectList(_context.TProjectStatuses, "FProjectStatusId", "FProjectStatusId", tProject.FProjectStatusId);
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tProject.FSiteId);
            ViewData["FSiteId"] = new SelectList(_context.TStyles, "FStyleId", "FStyleId", tProject.FSiteId);
            return View(tProject);
        }

        // GET: TProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TProjects == null)
            {
                return NotFound();
            }

            var tProject = await _context.TProjects
                .Include(t => t.FMember)
                .Include(t => t.FProjectStatus)
                .Include(t => t.FSite)
                .Include(t => t.FSiteNavigation)
                .FirstOrDefaultAsync(m => m.FProjectId == id);
            if (tProject == null)
            {
                return NotFound();
            }

            return View(tProject);
        }

        // POST: TProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TProjects == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TProjects'  is null.");
            }
            var tProject = await _context.TProjects.FindAsync(id);
            if (tProject != null)
            {
                _context.TProjects.Remove(tProject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TProjectExists(int id)
        {
          return (_context.TProjects?.Any(e => e.FProjectId == id)).GetValueOrDefault();
        }
    }
}
