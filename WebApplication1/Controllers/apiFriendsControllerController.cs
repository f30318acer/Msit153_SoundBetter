using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiFriendsControllerController : ControllerBase
    {
        private readonly dbSoundBetterContext _context;

        public apiFriendsControllerController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: api/FriendsController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TMember>>> GetTMembers()
        {
          if (_context.TMembers == null)
          {
              return NotFound();
          }
            return await _context.TMembers.ToListAsync();
        }

        // GET: api/FriendsController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TMember>> GetTMember(int id)
        {
          if (_context.TMembers == null)
          {
              return NotFound();
          }
            var tMember = await _context.TMembers.FindAsync(id);

            if (tMember == null)
            {
                return NotFound();
            }

            return tMember;
        }


        // PUT: api/FriendsController/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTMember(int id, TMember tMember)
        {
            if (id != tMember.FMemberId)
            {
                return BadRequest();
            }

            _context.Entry(tMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TMemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FriendsController
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TMember>> PostTMember(TMember tMember)
        {
          if (_context.TMembers == null)
          {
              return Problem("Entity set 'dbSoundBetterContext.TMembers'  is null.");
          }
            _context.TMembers.Add(tMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTMember", new { id = tMember.FMemberId }, tMember);
        }

        // DELETE: api/FriendsController/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTMember(int id)
        {
            if (_context.TMembers == null)
            {
                return NotFound();
            }
            var tMember = await _context.TMembers.FindAsync(id);
            if (tMember == null)
            {
                return NotFound();
            }

            _context.TMembers.Remove(tMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TMemberExists(int id)
        {
            return (_context.TMembers?.Any(e => e.FMemberId == id)).GetValueOrDefault();
        }
    }
}
