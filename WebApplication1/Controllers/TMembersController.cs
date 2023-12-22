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
    public class TMembersController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _environment;//注入

        public TMembersController(dbSoundBetterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: TMembers
        //public async Task<IActionResult> Index()
        //{
        //    var dbSoundBetterContext = _context.TMembers.Include(t => t.FPermission);
        //    return View(await dbSoundBetterContext.ToListAsync());
        //}

        // GET: TMembers
        //===========================================================================
        public async Task<IActionResult> Index(string search)
        {
            ViewData["CurrentFilter"] = search;

            var members = from m in _context.TMembers
                          select m;

            if (!String.IsNullOrEmpty(search))
            {
                members = members.Where(s => s.FName.Contains(search));
            }

            return View(await members.ToListAsync());
        }
      

        //===========================================================================
        // GET: TMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TMembers == null)
            {
                return NotFound();
            }

            var tMember = await _context.TMembers
                .Include(t => t.FPermission)
                .FirstOrDefaultAsync(m => m.FMemberId == id);
            if (tMember == null)
            {
                return NotFound();
            }

            return View(tMember);
        }

        // GET: TMembers/Create
        public IActionResult Create()
        {
            var permissionOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "一般會員" },
        new SelectListItem { Value = "2", Text = "管理員" }
    };

            ViewBag.FPermissionId = permissionOptions;
            return View();
        }

        // POST: TMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FMemberId,FUserame,FName,FPassword,FPhone,FEmail,FGender,FBirthday,FCreationTime,FIntroduction,FPermissionId,FPhotoPath")] TMember tMember, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    var fileName = Path.GetFileName(uploadedFile.FileName);
                    var filePath = Path.Combine(_environment.WebRootPath, "img", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    tMember.FPhotoPath = fileName; // 儲存檔案名
                }

                _context.Add(tMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId", tMember.FPermissionId);
            return View(tMember);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("FMemberId,FUsername,FName,FPasswod,FPhone,FEmail,FGender,FBirthday,FCreationTime,FIntroduction,FPermissionId,FPhotoPath")] TMember tMember, IFormFile FPicture)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (FPicture != null && FPicture.Length > 0)
        //        {
        //            var path = Path.Combine(_environment.WebRootPath, "img", FPicture.FileName);
        //            using (var stream = new FileStream(path, FileMode.Create))
        //            {
        //                await FPicture.CopyToAsync(stream);
        //            }

        //            tMember.FPhotoPath = FPicture.FileName; // 儲存檔案名
        //        }

        //        _context.Add(tMember);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(tMember);
        //}

        // GET: TMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TMembers == null)
            {
                return NotFound();
            }

            var tMember = await _context.TMembers.FindAsync(id);
            if (tMember == null)
            {
                return NotFound();
            }
            ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId", tMember.FPermissionId);
            return View(tMember);
        }

        // POST: TMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("FMemberId,FUserame,FName,FPassword,FPhone,FEmail,FGender,FBirthday,FCreationTime,FIntroduction,FPermissionId,FPhotoPath")] TMember tMember)
        //{
        //    if (id != tMember.FMemberId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tMember);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TMemberExists(tMember.FMemberId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId", tMember.FPermissionId);
        //    return View(tMember);
        //}
        //圖片更新功能
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FMemberId,FUserame,FName,FPassword,FPhone,FEmail,FGender,FBirthday,FCreationTime,FIntroduction,FPermissionId,FPhotoPath")] TMember tMember, IFormFile FPhotoPath)
        {
            if (id != tMember.FMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var memberToUpdate = await _context.TMembers.FirstOrDefaultAsync(c => c.FMemberId == id);

                    if (memberToUpdate == null)
                    {
                        return NotFound();
                    }

                    if (FPhotoPath != null && FPhotoPath.Length > 0)
                    {
                        var fileName = Path.GetFileName(FPhotoPath.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "img/Member/", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await FPhotoPath.CopyToAsync(fileStream);
                        }

                        // 只有在上傳新圖片時才更新圖片路徑
                        memberToUpdate.FPhotoPath = fileName;
                    }

                    // 更新除圖片以外的其他屬性
                    if (await TryUpdateModelAsync<TMember>(
                        memberToUpdate,
                        "",
                c => c.FUsername, c => c.FName, c => c.FPassword, c => c.FPhone, c => c.FEmail, c => c.FGender, c => c.FBirthday, c => c.FCreationTime, c => c.FIntroduction, c => c.FPermissionId))
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TMemberExists(tMember.FMemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(tMember);
        }
        // GET: TMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TMembers == null)
            {
                return NotFound();
            }

            var tMember = await _context.TMembers
                .Include(t => t.FPermission)
                .FirstOrDefaultAsync(m => m.FMemberId == id);
            if (tMember == null)
            {
                return NotFound();
            }

            return View(tMember);
        }

        // POST: TMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TMembers == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TMembers'  is null.");
            }
            var tMember = await _context.TMembers.FindAsync(id);
            if (tMember != null)
            {
                _context.TMembers.Remove(tMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TMemberExists(int id)
        {
            return (_context.TMembers?.Any(e => e.FMemberId == id)).GetValueOrDefault();
        }
    }
}
